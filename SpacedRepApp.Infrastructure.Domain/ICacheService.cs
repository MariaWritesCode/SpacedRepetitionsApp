using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacedRepApp.Infrastructure.Domain
{
    public interface ICacheService
    {
        IDatabase GetDatabase();
        System.Net.EndPoint[] GetEndPoints();
        IServer GetServer(string host, int port);
        Task<T> Get<T>(string key);
        Task Set<T>(string key, T value);
        Task Remove(string key);
    }
}
