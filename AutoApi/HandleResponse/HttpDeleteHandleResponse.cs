using System;
using Microsoft.AspNetCore.Http;
using SqlKata.Execution;

namespace AutoApi.Core.HandleResponse
{
    public interface IHttpDeleteHandleResponse:IHandleResponse
    {

    }
    
    public class HttpDeleteHandleResponse : BaseHandleResponse, IHttpDeleteHandleResponse
    {
        public HttpDeleteHandleResponse(IHttpContextAccessor contextAccessor, QueryFactory query) : base(contextAccessor, query)
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

            return query.Delete();
        }
    }
}
