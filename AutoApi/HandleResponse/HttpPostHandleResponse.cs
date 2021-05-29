using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlKata.Execution;

namespace AutoApi.HandleResponse
{
    public class HttpPostHandleResponse:BaseHandleResponse
    {
        public HttpPostHandleResponse(HttpContext context) : base(context)
        {
        }

        public override object Execute()
        {
            var query = GetQuery();

            //获取body
            var sr = new StreamReader(Context.Request.Body);
            var body = sr.ReadToEndAsync().Result;
            Context.Request.Body.Seek(0, SeekOrigin.Begin);

            var entity = (JObject)JsonConvert.DeserializeObject(body);

            var t = new List<KeyValuePair<string,object>>();
            foreach (var p in entity.Properties())
            {
                t.Add(new KeyValuePair<string, object>(p.Name, ((JValue)entity[p.Name]).Value));
            }
            
            var ret = query.Insert(t);

            return ret;
        }
    }
}
