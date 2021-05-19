using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreRateLimit;
using Castle.Core.Internal;
using Microsoft.Extensions.Options;
using Zxw.Framework.NetCore.IDbContext;
using Zxw.Framework.NetCore.Extensions;

namespace AutoApi
{
    public class AutoApiMiddleware : BaseMiddleware
    {
        public AutoApiMiddleware(RequestDelegate next) : base(next)
        {
        }

        public override async Task Invoke(HttpContext context)
        {
            await this._next(context);

            var controller = context.GetRouteData().Values["controller"]?.ToString();
            var action = context.GetRouteData().Values["action"]?.ToString();
            if(!controller.IsNullOrEmpty() && !action.IsNullOrEmpty())
                return;
            
            var method = context.Request.Method;

            var pathBase = context.Request.PathBase;
            var path = context.Request.Path;
            if (!pathBase.HasValue || !"/api".Equals(pathBase.Value, StringComparison.OrdinalIgnoreCase) || !path.HasValue)
            {
                return;
            }

            var paths = path.Value.Split("/");

            controller = paths[1];

            var dbcontext = (IDbContextCore)context.RequestServices.GetService(typeof(IDbContextCore));

            if ("GET".Equals(method, StringComparison.OrdinalIgnoreCase))
            {
                var sql = $"select * from {controller}";
                var dt = dbcontext.GetDataTable(sql);


                await context.Response.WriteAsync(HandleResponseContent(dt), Encoding.GetEncoding("GB2312"));                
            }
        }

        private string HandleResponseContent(object content, string dateFormat = "yyyy-MM-dd")
        {
            return JsonConvert.SerializeObject(content, Formatting.Indented, settings: new JsonSerializerSettings()
            {
                DateFormatString = dateFormat,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }
    }

    public static class AutoApiMiddlewareExtensions
    {
        public static IServiceCollection AddAutoRestfulApi(this IServiceCollection services, AutoRestfulApiOption option = null)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (option != null)
            {
                services.ConfigureOptions(option);
            }

            return services;
        }

        public static IApplicationBuilder UseAutoRestfulApi(this IApplicationBuilder app)
        {
            app.Map("/api", config => {
                config.UseMiddleware<AutoApiMiddleware>();
            });
            return app;
        }
    }
}
