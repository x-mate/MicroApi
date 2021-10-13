using Microsoft.Extensions.DependencyInjection;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace AutoApi.SqlServer
{
    public static class AutoApiMiddlewareExtensions
    {
        public static IServiceCollection UseSqlServer(this IServiceCollection services, string connectionString, Action<SqlResult> logger = null)
        {

            DbConnection connection = new SqlConnection(connectionString);
            Compiler compiler = new SqlServerCompiler();

            var db = new QueryFactory(connection, compiler)
            {
                Logger = logger??(q =>
                {
                    var oldColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine(q.Sql);
                    Console.ForegroundColor = oldColor;
                })               
            };
            
            services.AddSingleton(db);
            return services;
        }
    }
}
