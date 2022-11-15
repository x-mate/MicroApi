using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Data.Common;

namespace MicroApi.Sqlite
{
    public static class MicroApiMiddlewareExtensions
    {
        public static IServiceCollection UseSqlite(this IServiceCollection services, string connectionString, Action<SqlResult> logger = null)
        {

            DbConnection connection = new SqliteConnection(connectionString);
            Compiler compiler = new SqliteCompiler();

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
