using FirebirdSql.Data.FirebirdClient;
using Microsoft.Extensions.DependencyInjection;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Data.Common;

namespace AutoApi.Firebird
{
    public static class AutoApiMiddlewareExtensions
    {
        public static IServiceCollection UseSqlFirebird(this IServiceCollection services, string connectionString, Action<SqlResult> logger = null)
        {

            DbConnection connection = new FbConnection(connectionString);
            Compiler compiler = new FirebirdCompiler();

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
