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

        public void LoadFromDblp(string xml) =>
            ExtractFromJsonSource(
                new DblpDataExtractor(context, "#text"),
                new DblpXmlConverterWrapper().ExtractFromXml(xml));

        public void LoadFromIeeeXplore()
        {
            var wrapper = new IeeeXploreApiWrapper();

            var articles = wrapper.ExtractFromApi(1000, IeeeXploreSubmissionKind.Articles).Result;
            ExtractFromJsonSource(new IeeeXploreDataExtractor(context), articles);

            var books = wrapper.ExtractFromApi(1000, IeeeXploreSubmissionKind.Books).Result;
            ExtractFromJsonSource(new IeeeXploreDataExtractor(context), books);

            var inProceedings = wrapper.ExtractFromApi(1000, IeeeXploreSubmissionKind.InProceedings).Result;
            ExtractFromJsonSource(new IeeeXploreDataExtractor(context), inProceedings);
        }

        public void LoadFromGoogleScholar()
        {
            ICollection<GoogleScholarSeleniumScrapper.ScrapperResult> scrapped;
            using (var webDriver = new EdgeDriver())
            using (var scrapper = new GoogleScholarSeleniumScrapper(webDriver, 1))
            {
                scrapped = scrapper.Scrap("time travel");
            }

            var bibtex = scrapped
                    .Where(e => e.Text is not null)
                    .Select(e => e.Text!);
            var json = new BibtexJsonConverter().BibtexToJson(bibtex);

            ExtractFromJsonSource(new BibtexDataExtractor(context), json);
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
