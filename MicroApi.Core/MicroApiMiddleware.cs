using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using MicroApi.Core.HandleResponse;

namespace MicroApi.Core
{
    public class MicroApiMiddleware : BaseMiddleware
    {
        public MicroApiMiddleware(RequestDelegate next) : base(next)
        {
        }

        public override async Task Invoke(HttpContext context)
        {
            context.Request.EnableBuffering();
            await _next(context);

            var controller = context.GetRouteData().Values["controller"]?.ToString();
            var action = context.GetRouteData().Values["action"]?.ToString();
            if (!string.IsNullOrEmpty(controller) && !string.IsNullOrEmpty(action))
                return;

            var pathBase = context.Request.PathBase;
            var path = context.Request.Path;
            if (!pathBase.HasValue || !"/api".Equals(pathBase.Value, StringComparison.OrdinalIgnoreCase) || !path.HasValue)
            {
                return;
            }

            var settings = (JsonSerializerSettings)context.RequestServices.GetService(typeof(JsonSerializerSettings));

            try
            {
                var result = HandleResponseFactory.CreateHandleResponse(context).Execute();
                var response = new { success = true, data = result };

                context.Response.StatusCode = (int)HttpStatusCode.OK;

                await context.Response.WriteAsync(HandleResponseContent(response, settings), Encoding.Default);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                var response = new
                {
                    success = false,
                    data = e
                };
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync(HandleResponseContent(response, settings), Encoding.Default);
            }

        }

        private string HandleResponseContent(object content, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(content, Formatting.Indented, settings);
        }
    }

    public static class AutoApiMiddlewareExtensions
    {
        public static IServiceCollection AddMicroApi(this IServiceCollection services, JsonSerializerSettings settings = null)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IHttpGetHandleResponse, HttpGetHandleResponse>();
            services.AddScoped<IHttpPostHandleResponse, HttpPostHandleResponse>();
            services.AddScoped<IHttpPutHandleResponse, HttpPutHandleResponse>();
            services.AddScoped<IHttpDeleteHandleResponse, HttpDeleteHandleResponse>();

            if (settings == null)
                settings = new JsonSerializerSettings()
                {
                    DateFormatString = "yyyy-MM-dd",
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
            services.AddSingleton(settings);

            return services;
        }

        public static IApplicationBuilder UseMicroApi(this IApplicationBuilder app)
        {
            app.Map("/api", config =>
            {
                config.UseMiddleware<MicroApiMiddleware>();
            });
            return app;
        }
    }
}
