﻿using IEIPaperSearch.DataExtractors;
using IEIPaperSearch.DataExtractors.BDLP;
using IEIPaperSearch.DataExtractors.Bibtex;
using IEIPaperSearch.DataExtractors.IeeeXplore;
using IEIPaperSearch.DataSourceWrappers.DBLP;
using IEIPaperSearch.DataSourceWrappers.GoogleScholar;
using IEIPaperSearch.DataSourceWrappers.IeeeXplore;
using IEIPaperSearch.Models;
using IEIPaperSearch.Persistence;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static IEIPaperSearch.Services.DataLoaders.IDataLoaderService;

namespace IEIPaperSearch.Services.DataLoaders
{
    internal class DataLoaderService : IDataLoaderService
    {
        private readonly PaperSearchContext context;

        const int IEEE_XPLORE_LIMIT_PER_TYPE = 1000;
        const int GSCHLOAR_PAGE_LIMIT = 10;

        const string SELENIUM_WEBDRIVER_CHROME = "chrome";
        const string SELENIUM_WEBDRIVER_EDGE = "edge";
        const string SELENIUM_WEBDRIVER_IE = "ie";
        const string SELENIUM_WEBDRIVER_GECKO = "gecko";

        public DataLoaderService(PaperSearchContext context)
        {
            this.context = context;
        }

        public DataLoaderResult LoadFromDblp(int? startYear, int? endYear)
        {
            Console.WriteLine("Started DBLP data extraction...");

            var path = Environment.GetEnvironmentVariable("IEI_DBLP_FILE_PATH");
            if (path is null)
            {
                throw new InvalidOperationException("Can't find DBLP data: environment variable IEI_DBLP_FILE_PATH not set.");
            }
            var xml = File.ReadAllText(path);

            Console.WriteLine("Inserting DBLP data into database...");
            var count = ExtractFromJsonSource(
                new DblpDataExtractor(context, "#text"),
                new DblpXmlConverterWrapper().ExtractFromXml(xml),
                startYear,
                endYear);

            Console.WriteLine($"done extracting DBLP data ({count} entries).");

            return new DataLoaderResult(count);
        }

        public DataLoaderResult LoadFromIeeeXplore(int? startYear, int? endYear)
        {
            Console.WriteLine("Started IEEE Xplore data extraction...");

            var wrapper = new IeeeXploreApiWrapper();

            Console.WriteLine("Inserting IEEE Xplore articles data into database (1/3)...");
            var articles = wrapper.ExtractFromApi(IEEE_XPLORE_LIMIT_PER_TYPE, IeeeXploreSubmissionKind.Articles, startYear, endYear).Result;
            ExtractFromJsonSource(new IeeeXploreDataExtractor(context), articles);

            Console.WriteLine("Inserting IEEE Xplore books data into database (2/3)...");
            var books = wrapper.ExtractFromApi(IEEE_XPLORE_LIMIT_PER_TYPE, IeeeXploreSubmissionKind.Books, startYear, endYear).Result;
            var count = ExtractFromJsonSource(new IeeeXploreDataExtractor(context), books);

            Console.WriteLine("Inserting IEEE Xplore inproceedings data into database (3/3)...");
            var inProceedings = wrapper.ExtractFromApi(IEEE_XPLORE_LIMIT_PER_TYPE, IeeeXploreSubmissionKind.InProceedings, startYear, endYear).Result;
            ExtractFromJsonSource(new IeeeXploreDataExtractor(context), inProceedings);

            Console.WriteLine($"done extracting IEEE Xplore data ({count} entries).");

            return new DataLoaderResult(count);
        }

        public DataLoaderResult LoadFromGoogleScholar(int? startYear, int? endYear, string? query)
        {
            Console.WriteLine("Started Google Scholar data extraction...");

            var desiredWebDriver = Environment.GetEnvironmentVariable("IEI_SELENIUM_WEBDRIVER")?.ToLower();
            IWebDriver webDriver = desiredWebDriver switch
            {
                SELENIUM_WEBDRIVER_CHROME => new ChromeDriver(),
                SELENIUM_WEBDRIVER_EDGE => new EdgeDriver(),
                SELENIUM_WEBDRIVER_IE => new InternetExplorerDriver(),
                SELENIUM_WEBDRIVER_GECKO => new FirefoxDriver(),
                _ => throw new InvalidElementStateException("No WebDriver for Selenium has been configured."),
            };

            ICollection<GoogleScholarSeleniumScraper.ScrapperResult> scrapped;
            using (webDriver)
            using (var scrapper = new GoogleScholarSeleniumScraper(webDriver, GSCHLOAR_PAGE_LIMIT))
            {
                scrapped = scrapper.Scrap(startYear, endYear, query);
            }

            Console.WriteLine("Coverting Google Scholar Bibtex data to JSON...");
            var bibtex = scrapped
                    .Where(e => e.Text is not null)
                    .Select(e => e.Text!);
            var json = new BibtexJsonConverter().BibtexToJson(bibtex);

            Console.WriteLine("Inserting Google Scholar data into database...");
            var count = ExtractFromJsonSource(new BibtexDataExtractor(context), json);

            Console.WriteLine($"done extracting Google Scholar data ({count} entries).");

            return new DataLoaderResult(count);
        }

        private int ExtractFromJsonSource(IJsonDataExtractor<SubmissionDataExtractorResult> extractor, string json, int? startYear = null, int? endYear = null)
        {
            var (articles, books, inProceedings) = extractor.Extract(json);
            var count = 0;
            
            context.Books.AddRange(FilterByYear(books, startYear, endYear));
            count += books.Count;
            
            context.Articles.AddRange(FilterByYear(articles, startYear, endYear));
            count += articles.Count;
            
            context.InProceedings.AddRange(FilterByYear(inProceedings, startYear, endYear));
            count += inProceedings.Count;

            context.SaveChanges();
            return count;
        }

        private IEnumerable<T> FilterByYear<T>(ICollection<T> collection, int? startYear, int? endYear)
            where T : Submission
        {
            var query = (IEnumerable<T>)collection;
            if (startYear is not null)
            {
                query = query.Where(s => s.Year >= startYear);
            }
            if (endYear is not null)
            {
                query = query.Where(s => s.Year <= endYear);
            }

            return query;
        }
    }
}
