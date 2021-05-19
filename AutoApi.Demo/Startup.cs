using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Zxw.Framework.NetCore.DbContextCore;
using Zxw.Framework.NetCore.Extensions;
using Zxw.Framework.NetCore.IDbContext;
using Zxw.Framework.NetCore.Options;

namespace AutoApi.Demo
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
            //需要从加载配置文件appsettings.json
            services.AddOptions();
            services.AddSingleton(Configuration);
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AutoApi.Demo", Version = "v1" });
            });

            InitWorks(services);
        }

        private IServiceProvider InitWorks(IServiceCollection services)
        {
            var dbConnectionString = Configuration.GetConnectionString("MsSqlServer");
            services.Configure<DbContextOption>(options =>
            {
                options.ConnectionString = dbConnectionString;
                options.IsOutputSql = true;
            });
            services.AddSingleton(Configuration)
                .AddDbContext<IDbContextCore, SqlServerDbContext>();
            services.AddAutoRestfulApi();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return services.AddCoreX();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AutoApi.Demo v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseAutoRestfulApi();
        }
    }
}
