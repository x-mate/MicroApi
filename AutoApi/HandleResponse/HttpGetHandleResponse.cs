using System;
using System.Data.Common;
using System.Data.SqlClient;
using FirebirdSql.Data.FirebirdClient;
using FreeSql;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using MySqlConnector;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace AutoApi.HandleResponse
{
    public class HttpGetHandleResponse:BaseHandleResponse
    {
        public HttpGetHandleResponse(HttpContext context) : base(context)
        {
        }

        private QueryFactory GetQueryFactory()
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


            var db = new QueryFactory(connection, compiler);
            return db;
        }

        public override string Execute()
        {

            var table = GetTableName();

            var db = GetQueryFactory();
            var query = db.Query(table);


            //var sql = new StringBuilder();
            //var where = new StringBuilder();
            //var parameters = new List<DbParameter>();
            //sql.Append($"select * from {table} where 1=1");

            
            foreach (var item in Context.Request.Query)
            {
                #region 分页

                if ("page".Equals(item.Key, StringComparison.OrdinalIgnoreCase))
                {
                    var page = item.Value.ToString().ToInt(1);
                    query = query.Take(page);
                    continue;
                }
                if ("size".Equals(item.Key, StringComparison.OrdinalIgnoreCase))
                {
                    var size = item.Value.ToString().ToInt(10);
                    query = query.Limit(size);
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

                #endregion

                query = query.Where(item.Key, item.Value[0]);
            }
            
            var dt = query.Get();
            return HandleResponseContent(dt);
        }
    }
}
