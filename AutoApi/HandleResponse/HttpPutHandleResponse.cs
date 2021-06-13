using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlKata.Execution;

namespace AutoApi.HandleResponse
{
    public class HttpPutHandleResponse : BaseHandleResponse
    {
        public HttpPutHandleResponse(HttpContext context) : base(context)
        {
        }

        public override object Execute()
        {
            var query = GetQuery();

            foreach (var item in Context.Request.Query)
            {
                #region 特殊运算，比如大于等于、小于等于、大于、小于、IN查询

                //大于等于
                if (item.Key.EndsWith(".ge", StringComparison.OrdinalIgnoreCase))
                {
                    query.Where(item.Key.Replace(".ge", "", StringComparison.OrdinalIgnoreCase), ">=",
                        item.Value.ToString());
                    continue;
                }
                //大于
                if (item.Key.EndsWith(".gt", StringComparison.OrdinalIgnoreCase))
                {
                    query.Where(item.Key.Replace(".gt", "", StringComparison.OrdinalIgnoreCase), ">",
                        item.Value.ToString());
                    continue;
                }
                //小于等于
                if (item.Key.EndsWith(".le", StringComparison.OrdinalIgnoreCase))
                {
                    query.Where(item.Key.Replace(".ge", "", StringComparison.OrdinalIgnoreCase), "<=",
                        item.Value.ToString());
                    continue;
                }
                //小于
                if (item.Key.EndsWith(".lt", StringComparison.OrdinalIgnoreCase))
                {
                    query.Where(item.Key.Replace(".ge", "", StringComparison.OrdinalIgnoreCase), "<",
                        item.Value.ToString());
                    continue;
                }
                //在范围内
                if (item.Key.EndsWith(".in", StringComparison.OrdinalIgnoreCase))
                {
                    query.WhereIn(item.Key.Replace(".in", "", StringComparison.OrdinalIgnoreCase),
                        item.Value.ToString().Split(","));
                    continue;
                }
                //模糊查询
                if (item.Key.EndsWith(".like", StringComparison.OrdinalIgnoreCase))
                {
                    query.WhereLike(item.Key.Replace(".like", "", StringComparison.OrdinalIgnoreCase),
                        item.Value.ToString());
                    continue;
                }

                #endregion

                query = query.Where(item.Key, item.Value.ToString());
            }

            //获取body
            var sr = new StreamReader(Context.Request.Body);
            var body = sr.ReadToEndAsync().Result;
            Context.Request.Body.Seek(0, SeekOrigin.Begin);

            var entity = (JObject)JsonConvert.DeserializeObject(body);
            var t = new List<KeyValuePair<string, object>>();
            foreach (var p in entity.Properties())
            {
                t.Add(new KeyValuePair<string, object>(p.Name, ((JValue)entity[p.Name]).Value));
            }


            var ret = query.Update(t);

            return ret;
        }
    }
}
