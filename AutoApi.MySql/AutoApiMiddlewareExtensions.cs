using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Data.Common;

namespace AutoApi.MySql
{
    public static class AutoApiMiddlewareExtensions
    {
        public static IServiceCollection UseMySql(this IServiceCollection services, string connectionString, Action<SqlResult> logger = null)
        {

            DbConnection connection = new MySqlConnection(connectionString);
            Compiler compiler = new MySqlCompiler();

            var db = new QueryFactory(connection, compiler)
            {
                Logger = logger ?? (q =>
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
