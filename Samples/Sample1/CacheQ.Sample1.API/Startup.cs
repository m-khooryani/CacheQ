using CacheQ.Sample1.API.MediatorPipelines;
using CacheQ.Sample1.Application.PrimeNumbersCount;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace CacheQ.Sample1.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddLogging(builder =>
            {
                builder.AddConsole();
            });
            services.AddControllers(); 
            services.AddMediatR(typeof(PrimeNumbersCountQuery).Assembly);


            services.AddCacheQ(typeof(PrimeNumbersCountQuery).Assembly, 
                options =>
                {
                    //options.UseDistributedSqlServerCache(x=>x.SystemClock)
                    options.UseDistributedMemoryCache();
                    options.UsePrefixKey(type =>
                    {
                        return type.Name;
                    });
                });


            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(QueryLoggingBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(QueryCachingBehavior<,>));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CacheQ.Sample1.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CacheQ.Sample1.API v1"));
            }

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
