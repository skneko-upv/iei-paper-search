using IEIPaperSearch.DataExtractors.BDLP;
using IEIPaperSearch.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace IEIPaperSearch
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //new DblpDataExtractor().Extract(File.ReadAllText(@"C:\Users\Neko\Desktop\dblp-solo-article-1.json")); // FIXME remove this

            var host = CreateHostBuilder(args).Build();

            CreateDbIfNotExists(host);

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void CreateDbIfNotExists(IHost host)
        {
            using var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<PaperSearchContext>();
                context.Database.EnsureCreated();
            }
            catch (Exception e)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(e, "An error ocurred while ensuring database creation.");

                throw;
            }
        }
    }
}
