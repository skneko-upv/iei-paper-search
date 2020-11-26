using IEIPaperSearch.DataExtractors.BDLP;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace IEIPaperSearch
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new DblpDataExtractor().Extract(File.ReadAllText(@"C:\Users\Neko\Desktop\dblp-solo-article-1.json"));
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
