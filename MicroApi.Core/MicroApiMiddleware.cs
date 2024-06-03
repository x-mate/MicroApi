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
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace MicroApi.Core
{
    public class MicroApiMiddleware : BaseMiddleware
    {
        private readonly IAuthorizationService _authService;
        private readonly MicroApiOption _apiOption;
        public MicroApiMiddleware(RequestDelegate next, 
            IAuthorizationService authorizationService,
            MicroApiOption apiOption) : base(next)
        {
            _authService = authorizationService;
            _apiOption = apiOption;
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
            if (!pathBase.HasValue || !pathBase.Value.Contains(_apiOption.ApiRoutePrefix, StringComparison.OrdinalIgnoreCase) || !path.HasValue)
            {
                return;
            }

            try
            {
                var handler = HandleResponseFactory.CreateHandleResponse(context);
                var gb2312 = Encoding.GetEncoding("GB2312");
                if (_apiOption.AuthorizeType != AuthorizeType.None 
                    && !(await _authService.AuthorizeAsync(context.User, handler, MicroApiOperations.GetOperation(context.Request.Method))).Succeeded)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await context.Response.WriteAsync(HandleResponseContent(new { success = false, error = "Unauthorized" }), gb2312);
                }
                else
                {
                    var result = handler.Execute();
                    var response = new { success = true, data = result };
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    await context.Response.WriteAsync(HandleResponseContent(response), gb2312);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                var response = new
                {
                    success = false,
                    error = e
                };
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync(HandleResponseContent(response), Encoding.GetEncoding("GB2312"));
            }

        }

        private string HandleResponseContent(object content)
        {
            return JsonConvert.SerializeObject(content, Formatting.Indented, _apiOption.JsonSerializerSettings);
        }
    }

    public static class AutoApiMiddlewareExtensions
    {
        public static IServiceCollection AddMicroApi(this IServiceCollection services, Action<MicroApiOption> option = null)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            var defaultOption = new MicroApiOption();
            if (option != null)
                option.Invoke(defaultOption);
            if (defaultOption.ApiRoutePrefix.IsNullOrWhiteSpace())
                throw new Exception("The option MicroApiOption.ApiRoutePrefix can not be null or empty.");
            services.AddSingleton(defaultOption);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IHttpGetHandleResponse, HttpGetHandleResponse>();
            services.AddScoped<IHttpPostHandleResponse, HttpPostHandleResponse>();
            services.AddScoped<IHttpPutHandleResponse, HttpPutHandleResponse>();
            services.AddScoped<IHttpDeleteHandleResponse, HttpDeleteHandleResponse>();

            return services;
        }

        public static IApplicationBuilder UseMicroApi(this IApplicationBuilder app)
        {
            var option = (IOptions<MicroApiOption>)app.ApplicationServices.GetService(typeof(IOptions<MicroApiOption>));
            var pathMatch = $"/{option?.Value?.ApiRoutePrefix}/" + "{controller}";
            app.Map(pathMatch, config =>
            {
                config.UseMiddleware<MicroApiMiddleware>();
            });
            return app;
        }
    }
}
