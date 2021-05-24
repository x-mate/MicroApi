using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoApi.HandleResponse;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AutoApi
{
    public class AutoApiMiddleware : BaseMiddleware
    {
        public AutoApiMiddleware(RequestDelegate next) : base(next)
        {
        }

        public override async Task Invoke(HttpContext context)
        {
            context.Request.EnableBuffering();
            await this._next(context);

            var controller = context.GetRouteData().Values["controller"]?.ToString();
            var action = context.GetRouteData().Values["action"]?.ToString();
            if(!string.IsNullOrEmpty(controller) && !string.IsNullOrEmpty(action))
                return;
            
            var pathBase = context.Request.PathBase;
            var path = context.Request.Path;
            if (!pathBase.HasValue || !"/api".Equals(pathBase.Value, StringComparison.OrdinalIgnoreCase) || !path.HasValue)
            {
                return;
            }

            try
            {
                var result = HandleResponseFactory.CreateHandleResponse(context).Execute();
                var response = new {success = true, data = result};

                context.Response.StatusCode = (int)HttpStatusCode.OK;

                await context.Response.WriteAsync(HandleResponseContent(response), Encoding.GetEncoding("GB2312"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                var response = new
                {
                    success = false,
                    data = e
                };
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync(HandleResponseContent(response), Encoding.GetEncoding("GB2312"));
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
