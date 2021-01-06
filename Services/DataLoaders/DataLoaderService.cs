using IEIPaperSearch.DataExtractors;
using IEIPaperSearch.DataExtractors.BDLP;
using IEIPaperSearch.DataExtractors.Bibtex;
using IEIPaperSearch.DataExtractors.IeeeXplore;
using IEIPaperSearch.DataSourceWrappers.DBLP;
using IEIPaperSearch.DataSourceWrappers.GoogleScholar;
using IEIPaperSearch.DataSourceWrappers.IeeeXplore;
using IEIPaperSearch.Persistence;
using OpenQA.Selenium.Edge;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IEIPaperSearch.Services.DataLoaders
{
    internal class DataLoaderService : IDataLoaderService
    {
        private readonly PaperSearchContext context;

        public DataLoaderService(PaperSearchContext context)
        {
            this.context = context;
        }

        public void LoadFromAllSources()
        {
            //LoadFromDblp();
            //LoadFromIeeeXplore();
            //LoadFromGoogleScholar();
        }

        public void LoadFromDblp()
        {
            Console.WriteLine("Started DBLP data extraction...");

            var path = Environment.GetEnvironmentVariable("IEI_DBLP_FILE_PATH");
            if (path is null)
            {
                throw new InvalidOperationException("Can't find DBLP data: environment variable IEI_DBLP_FILE_PATH not set.");
            }
            var xml = File.ReadAllText(path);

            Console.WriteLine("Inserting DBLP data into database...");
            ExtractFromJsonSource(
                new DblpDataExtractor(context, "#text"),
                new DblpXmlConverterWrapper().ExtractFromXml(xml));

            Console.WriteLine("done extracting DBLP data.");
        }

        public void LoadFromIeeeXplore()
        {
            Console.WriteLine("Started IEEE Xplore data extraction...");

            var wrapper = new IeeeXploreApiWrapper();

            Console.WriteLine("Inserting IEEE Xplore articles data into database (1/3)...");
            var articles = wrapper.ExtractFromApi(1000, IeeeXploreSubmissionKind.Articles).Result;
            ExtractFromJsonSource(new IeeeXploreDataExtractor(context), articles);

            Console.WriteLine("Inserting IEEE Xplore books data into database (2/3)...");
            var books = wrapper.ExtractFromApi(1000, IeeeXploreSubmissionKind.Books).Result;
            ExtractFromJsonSource(new IeeeXploreDataExtractor(context), books);

            Console.WriteLine("Inserting IEEE Xplore inproceedings data into database (3/3)...");
            var inProceedings = wrapper.ExtractFromApi(1000, IeeeXploreSubmissionKind.InProceedings).Result;
            ExtractFromJsonSource(new IeeeXploreDataExtractor(context), inProceedings);

            Console.WriteLine("done extracting IEEE Xplore data.");
        }

        public void LoadFromGoogleScholar()
        {
            Console.WriteLine("Started Google Scholar data extraction...");

            ICollection<GoogleScholarSeleniumScrapper.ScrapperResult> scrapped;
            using (var webDriver = new EdgeDriver())
            using (var scrapper = new GoogleScholarSeleniumScrapper(webDriver, 1))
            {
                scrapped = scrapper.Scrap("time travel");
            }

            Console.WriteLine("Coverting Google Scholar Bibtex data to JSON...");
            var bibtex = scrapped
                    .Where(e => e.Text is not null)
                    .Select(e => e.Text!);
            var json = new BibtexJsonConverter().BibtexToJson(bibtex);

            Console.WriteLine("Inserting Google Scholar data into database...");
            ExtractFromJsonSource(new BibtexDataExtractor(context), json);

            Console.WriteLine("done extracting Google Scholar data.");
        }

        private void ExtractFromJsonSource(IJsonDataExtractor<SubmissionDataExtractorResult> extractor, string json)
        {
            var data = extractor.Extract(json);

            context.Books.AddRange(data.Books);
            context.Articles.AddRange(data.Articles);
            context.InProceedings.AddRange(data.InProceedings);
            context.SaveChanges();
        }
    }
}
