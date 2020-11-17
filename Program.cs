using IEIPaperSearch.DataExtraction.DBLP;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace IEIPaperSearch
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new DblpDataExtractor().ExtractData(@"C:\Users\Neko\Desktop\dblp.xml");
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
