using SpacedRepApp.Infrastructure.Domain;
using System;
using System.Threading.Tasks;
using StackExchange.Redis;
using Microsoft.Extensions.Configuration;
using System.Net.Sockets;
using System.Threading;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SpacedRepApp.Infrastructure
{
    public class CacheService : ICacheService
    {
        private static Lazy<ConnectionMultiplexer> lazyConnection = CreateConnection();
        private static IConfigurationRoot Configuration { get; set; }
        private readonly IDatabase _cacheDatabase;

        private static long lastReconnectTicks = DateTimeOffset.MinValue.UtcTicks;
        private static DateTimeOffset firstErrorTime = DateTimeOffset.MinValue;
        private static DateTimeOffset previousErrorTime = DateTimeOffset.MinValue;

        private static readonly object reconnectLock = new object();

        public CacheService()
        {
            InitializeConfiguration();
            _cacheDatabase = GetDatabase();
        }

        private static void InitializeConfiguration()
        {
            var builder = new ConfigurationBuilder().AddEnvironmentVariables().AddUserSecrets<CacheService>();

            Configuration = builder.Build();
        }

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }

        private static Lazy<ConnectionMultiplexer> CreateConnection()
        {
            return new Lazy<ConnectionMultiplexer>(() =>
            {
                string cacheConnection = Configuration.GetConnectionString("CacheConnection");
                return ConnectionMultiplexer.Connect(cacheConnection);
            });
        }

        public static TimeSpan ReconnectMinFrequency => TimeSpan.FromSeconds(60);

        // If errors continue for longer than the below threshold, then the
        // multiplexer seems to not be reconnecting, so ForceReconnect() will
        // re-create the multiplexer.
        public static TimeSpan ReconnectErrorThreshold => TimeSpan.FromSeconds(30);

        public static int RetryMaxAttempts => 5;

        private static void CloseConnection(Lazy<ConnectionMultiplexer> oldConnection)
        {
            if (oldConnection == null)
                return;

            try
            {
                oldConnection.Value.Close();
            }
            catch (Exception)
            {
                // Example error condition: if accessing oldConnection.Value causes a connection attempt and that fails.
            }
        }

        /// <summary>
        /// Force a new ConnectionMultiplexer to be created.
        /// NOTES:
        ///     1. Users of the ConnectionMultiplexer MUST handle ObjectDisposedExceptions, which can now happen as a result of calling ForceReconnect().
        ///     2. Don't call ForceReconnect for Timeouts, just for RedisConnectionExceptions or SocketExceptions.
        ///     3. Call this method every time you see a connection exception. The code will:
        ///         a. wait to reconnect for at least the "ReconnectErrorThreshold" time of repeated errors before actually reconnecting
        ///         b. not reconnect more frequently than configured in "ReconnectMinFrequency"
        /// </summary>
        public static void ForceReconnect()
        {
            var utcNow = DateTimeOffset.UtcNow;
            long previousTicks = Interlocked.Read(ref lastReconnectTicks);
            var previousReconnectTime = new DateTimeOffset(previousTicks, TimeSpan.Zero);
            TimeSpan elapsedSinceLastReconnect = utcNow - previousReconnectTime;

            if (elapsedSinceLastReconnect < ReconnectMinFrequency)
                return;

            lock (reconnectLock)
            {
                utcNow = DateTimeOffset.UtcNow;
                elapsedSinceLastReconnect = utcNow - previousReconnectTime;

                if (firstErrorTime == DateTimeOffset.MinValue)
                {
                    firstErrorTime = utcNow;
                    previousErrorTime = utcNow;
                    return;
                }

                if (elapsedSinceLastReconnect < ReconnectMinFrequency)
                    return; // Some other thread made it through the check and the lock, so nothing to do.

                TimeSpan elapsedSinceFirstError = utcNow - firstErrorTime;
                TimeSpan elapsedSinceMostRecentError = utcNow - previousErrorTime;

                bool shouldReconnect =
                    elapsedSinceFirstError >= ReconnectErrorThreshold // Make sure we gave the multiplexer enough time to reconnect on its own if it could.
                    && elapsedSinceMostRecentError <= ReconnectErrorThreshold; // Make sure we aren't working on stale data (e.g. if there was a gap in errors, don't reconnect yet).

                previousErrorTime = utcNow;

                if (!shouldReconnect)
                    return;

                firstErrorTime = DateTimeOffset.MinValue;
                previousErrorTime = DateTimeOffset.MinValue;

                Lazy<ConnectionMultiplexer> oldConnection = lazyConnection;
                CloseConnection(oldConnection);
                lazyConnection = CreateConnection();
                Interlocked.Exchange(ref lastReconnectTicks, utcNow.UtcTicks);
            }
        }

        private static T BasicRetry<T>(Func<T> func)
        {
            int reconnectRetry = 0;
            int disposedRetry = 0;

            while (true)
            {
                try
                {
                    return func();
                }
                catch (Exception ex) when (ex is RedisConnectionException || ex is SocketException)
                {
                    reconnectRetry++;
                    if (reconnectRetry > RetryMaxAttempts)
                        throw;
                    ForceReconnect();
                }
                catch (ObjectDisposedException)
                {
                    disposedRetry++;
                    if (disposedRetry > RetryMaxAttempts)
                        throw;
                }
            }
        }

        public IDatabase GetDatabase()
        {
            return BasicRetry(() => Connection.GetDatabase());
        }

        public System.Net.EndPoint[] GetEndPoints()
        {
            return BasicRetry(() => Connection.GetEndPoints());
        }

        public IServer GetServer(string host, int port)
        {
            return BasicRetry(() => Connection.GetServer(host, port));
        }

        public async Task<T> Get<T>(string key)
        {
            var result = await _cacheDatabase.StringGetAsync(key);

            if(!string.IsNullOrEmpty(result))
            {
                return JsonSerializer.Deserialize<T>(result, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, ReferenceHandler = ReferenceHandler.Preserve });
            }
            
            return default;
        }

        public async Task Set<T>(string key, T value)
        {
            await _cacheDatabase.StringSetAsync(key, JsonSerializer.Serialize(value, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, ReferenceHandler = ReferenceHandler.Preserve }));
        }

        public async Task Remove(string key)
        {
            await _cacheDatabase.KeyDeleteAsync(key);
        }
    }
}
