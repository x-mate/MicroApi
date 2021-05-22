using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Text;
using System.Threading.Tasks;
using AutoApi.HandleResponse;
using Humanizer;
using Microsoft.Extensions.Options;

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
            if(!string.IsNullOrEmpty(controller) && !string.IsNullOrEmpty(action))
                return;
            
            var method = context.Request.Method;

            var pathBase = context.Request.PathBase;
            var path = context.Request.Path;
            if (!pathBase.HasValue || !"/api".Equals(pathBase.Value, StringComparison.OrdinalIgnoreCase) || !path.HasValue)
            {
                return;
            }

            var reponse = HandleResponseFactory.CreateHandleResponse(context).Execute();

            await context.Response.WriteAsync(reponse, Encoding.GetEncoding("GB2312"));                
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
