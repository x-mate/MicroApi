using MicroApi.Core.HandleResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace MicroApi.Core
{
    public interface IMicroApiBuilder
    {
        IServiceCollection Services { get; set; }
    }

    internal class MicroApiBuilder : IMicroApiBuilder
    {
        public IServiceCollection Services { get; set; }

        internal MicroApiBuilder(IServiceCollection services)
        {
            Services = services;
        }
    }

    public static class MicroApiMiddlewareExtensions
    {
        public static IMicroApiBuilder PrepareApiBuilder(this IServiceCollection services)
        {
            return new MicroApiBuilder(services);
        }

        public static IMicroApiBuilder AddMicroApi(this IMicroApiBuilder builder, Action<MicroApiOption> option = null)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            var defaultOption = new MicroApiOption()
            {
                AuthorizeType = AuthorizeType.None,
                JsonSerializerSettings = new JsonSerializerSettings()
                {
                    DateFormatString = "yyyy-MM-dd HH:mm:ss",
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }
            };
            if (option != null)
                option.Invoke(defaultOption);
            builder.Services.AddSingleton(defaultOption);
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            builder.Services.AddScoped<IHttpGetHandleResponse, HttpGetHandleResponse>();
            builder.Services.AddScoped<IHttpPostHandleResponse, HttpPostHandleResponse>();
            builder.Services.AddScoped<IHttpPutHandleResponse, HttpPutHandleResponse>();
            builder.Services.AddScoped<IHttpDeleteHandleResponse, HttpDeleteHandleResponse>();

            return builder;
        }

        public static IServiceCollection Build(this IMicroApiBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            return builder.Services;
        }
    }
}
