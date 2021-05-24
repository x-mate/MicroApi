using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SqlKata.Execution;

namespace AutoApi.HandleResponse
{
    public class HttpDeleteHandleResponse : BaseHandleResponse
    {
        public HttpDeleteHandleResponse(HttpContext context) : base(context)
        {
        }

        public override object Execute()
        {
            var db = GetQueryFactory();

            var table = GetTableName();

            var query = db.Query(table);

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

            return query.Delete();
        }
    }
}
