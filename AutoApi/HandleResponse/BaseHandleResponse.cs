using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Net;
using FirebirdSql.Data.FirebirdClient;
using FreeSql;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using MySqlConnector;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace AutoApi.HandleResponse
{
    public abstract class BaseHandleResponse : IHandleResponse
    {
        public HttpContext Context { get; set; }
        public abstract object Execute();
        public string TableName { get; }
        public AutoApiOption ApiOption { get; set; }

        public BaseHandleResponse(HttpContext context)
        {
            this.Context = context;
            this.TableName = GetTableName();
            this.ApiOption =
                ((IOptions<AutoApiOption>) context.RequestServices.GetService(typeof(IOptions<AutoApiOption>)))?.Value;
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



        protected Query GetQuery()
        {
            var freeSql = (IFreeSql) Context.RequestServices.GetService(typeof(IFreeSql));

            var conStr = freeSql.Ado.ConnectionString;

            DbConnection connection;
            Compiler compiler;

            switch (freeSql.Ado.DataType)
            {
                case DataType.Firebird:
                    connection = new FbConnection(conStr);
                    compiler = new FirebirdCompiler();
                    break;
                case DataType.SqlServer:
                    connection = new SqlConnection(conStr);
                    compiler = new SqlServerCompiler();
                    break;
                case DataType.Sqlite:
                    connection = new SqliteConnection(conStr);
                    compiler = new SqliteCompiler();
                    break;
                case DataType.MySql:
                    connection = new MySqlConnection(conStr);
                    compiler = new MySqlCompiler();
                    break;
                case DataType.Oracle:
                    connection = new OracleConnection(conStr);
                    compiler = new OracleCompiler();
                    break;
                case DataType.PostgreSQL:
                    connection = new NpgsqlConnection(conStr);
                    compiler = new PostgresCompiler();
                    break;
                default:
                    throw new NotSupportedException("暂不支持当前数据库");
            }


            var db = new QueryFactory(connection, compiler)
            {
                Logger = q =>
                {
                    var oldColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine(q.Sql);
                    Console.ForegroundColor = oldColor;
                }
            };

            return db.Query(this.TableName);
        }
    }
}
