using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IEIPaperSearch.DataSourceWrappers.GoogleScholar
{
    public sealed class GoogleScholarSeleniumScrapper : IDisposable
    {
        const int DEFAULT_PAGE_LIMIT = 10;
        static readonly string GOOGLE_SCHOLAR_URL_BASE = "https://scholar.google.com/";
        static readonly string GOOGLE_SCHOLAR_BIBTEX_TEXT = "BibTeX";

        readonly IWebDriver driver;
        readonly int pageLimit;
        readonly IList<ScrapperResult> results = new List<ScrapperResult>();

        public GoogleScholarSeleniumScrapper(IWebDriver driver, int pageLimit = DEFAULT_PAGE_LIMIT)
        {
            this.driver = driver;
            this.pageLimit = pageLimit;

            //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
        }

        public IList<ScrapperResult> Scrap(string searchQuery)
        {
            results.Clear();

            SearchSettingUrl(searchQuery);

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

                try {
                    var nextPageButton = driver.FindElement(
                        By.CssSelector(".gs_btnPR .gs_in_ib .gs_btn_lrge .gs_btn_half .gs_btn_lsu"));
                    nextPageButton.Click();
                    page += 1;
                }
                catch (NoSuchElementException)
                {
                    break;
                }
            }

            foreach (var result in results)
            {
                driver.Navigate().GoToUrl(result.BibtexURL);
                TakeBibtexText(result);
            }

            return results.Where(r => r.Text is not null).ToList();
        }

        public void SearchSettingUrl(string query)
        {
            var uri = new UriBuilder(GOOGLE_SCHOLAR_URL_BASE)
            {
                Path = "scholar",
                Query = $"?q={HttpUtility.UrlEncode(query)}"
            };

            driver.Navigate().GoToUrl(uri.ToString());
        }

        public void HandleCiteModal(string? url)
        {
            var bibtexButton = driver.FindElements(By.ClassName("gs_citi"))
                .First(e => e.Text == GOOGLE_SCHOLAR_BIBTEX_TEXT);
            results.Add(new ScrapperResult(url, bibtexButton.GetProperty("href")));

            driver.FindElement(By.Id("gs_cit-x")).Click();
        }

        public void TakeBibtexText(ScrapperResult result)
        {
            try
            {
                var text = driver.FindElement(By.TagName("pre")).Text;
                result.Text = text;
            } 
            catch (NoSuchElementException)
            { }
        }

        public void Dispose() => driver.Close();

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
