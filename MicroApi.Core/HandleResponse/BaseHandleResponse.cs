using System;
using Microsoft.AspNetCore.Http;
using SqlKata;
using SqlKata.Execution;

namespace MicroApi.Core.HandleResponse
{
    public abstract class BaseHandleResponse : IHandleResponse
    {
        public HttpContext Context { get; set; }
        public abstract object Execute();
        public string TableName { get; }

        public QueryFactory Query { get; set; }

        public BaseHandleResponse(IHttpContextAccessor contextAccessor, QueryFactory query)
        {
            Context = contextAccessor.HttpContext;
            Query = query;
            TableName = GetTableName();
        }

        private string GetTableName()
        {
            var path = Context.Request.Path;

            var paths = path.Value.Split("/");

            if (paths.Length < 2)
            {
                throw new Exception("非法请求，api请求路径格式为/api/{TableName},且TableName不能为空。");//api请求路径格式为/api/{TableName}
            }
            var table = paths[1];
            if (table.IsNullOrWhiteSpace())
            {
                throw new Exception("非法请求，api请求路径格式为/api/{TableName},且TableName不能为空。");//api请求路径格式为/api/{TableName}
            }

            return table;
        }
        protected Query GetQuery() => Query.Query(TableName);
    }
}
