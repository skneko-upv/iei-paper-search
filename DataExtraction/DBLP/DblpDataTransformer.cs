using IEIPaperSearch.Models;
using System.Collections.Generic;
using System.Linq;

namespace IEIPaperSearch.DataExtraction.DBLP
{
    public class DblpDataTransformer
    {
        public ICollection<Submission> Transform(ICollection<dblpArticle> xmlArticles)
        {
            var submissions = new List<Submission>();

            foreach (var xmlArticle in xmlArticles)
            {
                int? startPage = null;
                int? endPage = null;
                if (xmlArticle.pages != null)
                {
                    var pages = xmlArticle.pages.Split("-").Select(s => s.Trim()).ToList();
                    startPage = int.Parse(pages.FirstOrDefault());
                    endPage = int.Parse(pages.LastOrDefault());
                }

                var article = new Article(
                    "Test" /* xmlArticle.title.First().i.First().sub */,
                    int.Parse(xmlArticle.year),
                    xmlArticle.ee.First().Value,
                    startPage,
                    endPage);

                // buscar autores en la base de datos

                // buscar ejemplar de revisa en la base datos

                submissions.Add(article);
            }

            return submissions;
        }
    }
}
