using Microsoft.Extensions.DependencyInjection;
using Oracle.ManagedDataAccess.Client;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Data.Common;

namespace AutoApi.Oracle
{
    public static class AutoApiMiddlewareExtensions
    {
        public static IServiceCollection UseOracle(this IServiceCollection services, string connectionString, Action<SqlResult> logger = null)
        {

            DbConnection connection = new OracleConnection(connectionString);
            Compiler compiler = new OracleCompiler();

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
