using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MicroApi.Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseKestrel(config =>
                    {
                        //config.ListenAnyIP(8188);
                    })
                    .ConfigureAppConfiguration((context, config) =>
                    {
                        // Configure the app here.
                    })
                    .UseStartup<Startup>();
                });
    }
}
