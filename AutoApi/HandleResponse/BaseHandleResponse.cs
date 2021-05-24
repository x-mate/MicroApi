using System;
using System.Data.Common;
using System.Data.SqlClient;
using FirebirdSql.Data.FirebirdClient;
using FreeSql;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using MySqlConnector;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace AutoApi.HandleResponse
{
    public abstract class BaseHandleResponse:IHandleResponse
    {
        public HttpContext Context { get; set; }
        public abstract object Execute();

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



        protected QueryFactory GetQueryFactory()
        {
            var freeSql = (IFreeSql)Context.RequestServices.GetService(typeof(IFreeSql));

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

            return db;
        }
    }
}
