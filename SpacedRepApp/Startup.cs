using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SpacedRepApp.Infrastructure;
using SpacedRepApp.Infrastructure.Domain;

namespace SpacedRepApp
{
    public class Startup
    {
        private string _connectionString = null;
        private string _connectionStringAppDb = null;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SpacedRepApp", Version = "v1" });
            });

#if DEBUG

            _connectionString = Configuration.GetConnectionString("SpacedRepAppDb");
            _connectionStringAppDb = Configuration.GetConnectionString("ApplicationDb");
#else
            _connectionString = Configuration.GetConnectionString("SpacedRepAppDb");
            _connectionStringAppDb = Configuration.GetConnectionString("ApplicationDb");
#endif
            services.AddDbContext<SpacedRepAppDbContext>(cfg =>
            {
                cfg.UseSqlServer(_connectionString);
            });

            services.AddScoped<INoteRepository, NoteRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<ICacheService, CacheService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {           
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SpacedRepApp v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
