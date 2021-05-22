using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AutoApi.HandleResponse
{
    public abstract class BaseHandleResponse:IHandleResponse
    {
        public HttpContext Context { get; set; }
        public abstract string Execute();

        public BaseHandleResponse(HttpContext context)
        {
            this.Context = context;
        }

        protected string GetTableName()
        {
            var path = Context.Request.Path;

            var paths = path.Value.Split("/");

            return paths[1];
        }

        protected string HandleResponseContent(object content, string dateFormat = "yyyy-MM-dd")
        {
            return JsonConvert.SerializeObject(content, Formatting.Indented, settings: new JsonSerializerSettings()
            {
                DateFormatString = dateFormat,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }

    }
}
