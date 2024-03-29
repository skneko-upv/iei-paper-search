﻿using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace IEIPaperSearch.DataSourceWrappers.GoogleScholar
{
    public sealed class GoogleScholarSeleniumScraper : IDisposable
    {
        const int DEFAULT_PAGE_LIMIT = 10;
        static readonly string GOOGLE_SCHOLAR_URL_BASE = "https://scholar.google.com/";
        static readonly string GOOGLE_SCHOLAR_BIBTEX_TEXT = "BibTeX";

        readonly IWebDriver driver;
        readonly int pageLimit;
        readonly IList<ScrapperResult> results = new List<ScrapperResult>();

        readonly Random rng = new Random();

        public GoogleScholarSeleniumScraper(IWebDriver driver, int pageLimit = DEFAULT_PAGE_LIMIT)
        {
            this.driver = driver;
            this.pageLimit = pageLimit;

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        public IList<ScrapperResult> Scrap(int? startYear, int? endYear, string? searchQuery)
        {
            results.Clear();

            SearchSettingUrl(startYear, endYear, searchQuery);

            var page = 1;
            while (page <= pageLimit)
            {
                var submissionDivs = driver.FindElements(By.ClassName("gs_or"));
                foreach (var submissionDiv in submissionDivs)
                {
                    var linkDiv = submissionDiv.FindElements(By.ClassName("gs_or_ggsm")).FirstOrDefault();
                    var url = linkDiv?.FindElements(By.TagName("a")).FirstOrDefault()?.GetProperty("href");
                    var citeButton = submissionDiv.FindElement(By.ClassName("gs_or_cit"));
                    citeButton.Click();
                    HandleCiteModal(url);
                }

                IWebElement nextPageButton;
                try {
                    nextPageButton = driver.FindElement(
                        By.CssSelector(".gs_btnPR .gs_in_ib .gs_btn_lrge .gs_btn_half .gs_btn_lsu"));
                }
                catch (NoSuchElementException)
                {
                    break;
                }

                if (nextPageButton is null)
                {
                    try
                    {
                        nextPageButton = driver.FindElement(By.Id("gs_n"))
                            .FindElement(By.TagName("center"))
                            .FindElement(By.TagName("table"))
                            .FindElement(By.TagName("tbody"))
                            .FindElement(By.TagName("tr"))
                            .FindElements(By.TagName("td"))
                            .Last()
                            .FindElement(By.TagName("a"));
                    }
                    catch (NoSuchElementException)
                    {
                        break;
                    }
                }

                nextPageButton.Click();
                page += 1;
            }

            foreach (var result in results)
            {
                driver.Navigate().GoToUrl(result.BibtexURL);
                TakeBibtexText(result);
            }

            return results.Where(r => r.Text is not null).ToList();
        }

        public void SearchSettingUrl(int? startYear, int? endYear, string? searchQuery)
        {
            var query = $"?q={HttpUtility.UrlEncode(searchQuery)}";

            if (startYear is not null)
            {
                query += $"&as_ylo={startYear}";
            }
            if (endYear is not null)
            {
                query += $"&as_yhi={endYear}";
            }

            var uri = new UriBuilder(GOOGLE_SCHOLAR_URL_BASE)
            {
                Path = "scholar",
                Query = query
            };

            driver.Navigate().GoToUrl(uri.ToString());
        }

        public void HandleCiteModal(string? url)
        {
            Thread.Sleep(rng.Next(2500, 4000));

            var bibtexButton = driver.FindElements(By.ClassName("gs_citi"))
                .First(e => e.Text == GOOGLE_SCHOLAR_BIBTEX_TEXT);
            results.Add(new ScrapperResult(url, bibtexButton.GetProperty("href")));

            driver.FindElement(By.Id("gs_cit-x")).Click();
        }

        public void TakeBibtexText(ScrapperResult result)
        {
            try
            {
                Thread.Sleep(rng.Next(1200, 2600));
                var text = driver.FindElement(By.TagName("pre")).Text;
                result.Text = text;
            } 
            catch (NoSuchElementException)
            { }
        }

        public void Dispose() => driver.Quit();

        public class ScrapperResult
        {
            public string? PdfURL { get; }
            public string? BibtexURL { get; }

            public string? Text { get; set; }

            public ScrapperResult(string? pdfURL, string? bibtexURL)
            {
                PdfURL = pdfURL;
                BibtexURL = bibtexURL;
            }
        }
    }
}
