using System;
using System.Data.Common;
using System.Data.SqlClient;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using MySqlConnector;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace AutoApi.Core.HandleResponse
{
    public abstract class BaseHandleResponse : IHandleResponse
    {
        public HttpContext Context { get; set; }
        public abstract object Execute();
        public string TableName { get; }

        public QueryFactory Query { get; set; }

        public BaseHandleResponse(IHttpContextAccessor contextAccessor, QueryFactory query)
        {
            this.Context = contextAccessor.HttpContext;
            this.Query = query;
            this.TableName = GetTableName();
        }

        private string GetTableName()
        {
            var path = this.Context.Request.Path;

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
        protected Query GetQuery()=> Query.Query(this.TableName);
    }
}
