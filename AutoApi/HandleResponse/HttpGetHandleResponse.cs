using System;
using Microsoft.AspNetCore.Http;
using SqlKata.Execution;

namespace AutoApi.Core.HandleResponse
{
    public class HttpGetHandleResponse:BaseHandleResponse
    {
        public HttpGetHandleResponse(HttpContext context) : base(context)
        {
        }

        public override object Execute()
        {

            var query = GetQuery();

            int? page = null;
            int? size = null;
            int? offset = null;

            foreach (var item in Context.Request.Query)
            {
                #region 分页

                if ("page".Equals(item.Key, StringComparison.OrdinalIgnoreCase))
                {
                    page = item.Value.ToString().ToInt(1);
                    continue;
                }
                if ("offset".Equals(item.Key, StringComparison.OrdinalIgnoreCase))
                {
                    offset = item.Value.ToString().ToInt(10);
                    continue;
                }
                if ("size".Equals(item.Key, StringComparison.OrdinalIgnoreCase)|| "limit".Equals(item.Key, StringComparison.OrdinalIgnoreCase))
                {
                    size = item.Value.ToString().ToInt(10);
                    continue;
                }

                #endregion

                #region 排序

                //正序
                if ("orderAsc".Equals(item.Key, StringComparison.OrdinalIgnoreCase))
                {
                    query = query.OrderBy(item.Value.ToString().Split(","));
                    continue;
                }
                //倒序
                if ("orderDesc".Equals(item.Key, StringComparison.OrdinalIgnoreCase))
                {
                    query = query.OrderByDesc(item.Value.ToString().Split(","));
                    continue;
                }

                #endregion

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

                query = query.Where(item.Key, item.Value[0]);
            }

            if (page.HasValue || size.HasValue)
            {
                if (!offset.HasValue)
                    offset = ((page ?? 1) - 1) * (size ?? 10);
                query = query.Skip(offset.Value).Take(size ?? 10);
            }
            
            var dt = query.Get();
            return dt;
        }
    }
}
