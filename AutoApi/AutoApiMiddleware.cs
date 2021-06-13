using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoApi.HandleResponse;
using FreeRedis;
using FreeSql.DataAnnotations;
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
        public static IServiceCollection AddAutoRestfulApi([NotNull] this IServiceCollection services,
            [NotNull] AutoApiOption option)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (option == null)
            {
                throw new ArgumentNullException(nameof(option));
            }

            if (option.DbMasterConnectionString.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(option.DbMasterConnectionString));

            var freeSqlBuilder = new FreeSql.FreeSqlBuilder()
                .UseConnectionString(option.DbType, option.DbMasterConnectionString)
                ;
            if (option.DbSlaveConnectionStrings?.Length > 0)
            {
                freeSqlBuilder = freeSqlBuilder.UseSlave(option.DbSlaveConnectionStrings);
            }

            var freeSql = freeSqlBuilder.Build();
            freeSql.Aop.CurdBefore += (s, e) =>
            {
                //记录sql
                Console.WriteLine(e.Sql);
            };

            services.AddSingleton<IFreeSql>(freeSql);

            if (option.EnableGetCache && option.RedisConnectionStrings?.Length>0)
            {
                var redisClient = new RedisClient(option.RedisConnectionStrings[0])
                {
                    Serialize = JsonConvert.SerializeObject,
                    Deserialize = JsonConvert.DeserializeObject
                };

                services.AddSingleton<RedisClient>(redisClient);
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
