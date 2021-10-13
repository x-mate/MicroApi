using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoApi.Core.HandleResponse;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AutoApi.Core
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

                await context.Response.WriteAsync(HandleResponseContent(response), Encoding.Default);
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
                await context.Response.WriteAsync(HandleResponseContent(response), Encoding.Default);
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
        public static IServiceCollection AddAutoRestfulApi([NotNull] this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            services.AddScoped<IHttpGetHandleResponse, HttpGetHandleResponse>();
            services.AddScoped<IHttpPostHandleResponse, HttpPostHandleResponse>();
            services.AddScoped<IHttpPutHandleResponse, HttpPutHandleResponse>();
            services.AddScoped<IHttpDeleteHandleResponse, HttpDeleteHandleResponse>();

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
