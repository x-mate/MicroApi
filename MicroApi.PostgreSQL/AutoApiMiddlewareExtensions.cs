using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Data.Common;

namespace MicroApi.PostgreSQL
{
    public static class MicroApiMiddlewareExtensions
    {
        public static IServiceCollection UsePostgreSQL(this IServiceCollection services, string connectionString, Action<SqlResult> logger = null)
        {

            DbConnection connection = new NpgsqlConnection(connectionString);
            Compiler compiler = new PostgresCompiler();

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
