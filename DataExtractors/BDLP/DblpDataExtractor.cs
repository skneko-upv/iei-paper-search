using IEIPaperSearch.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IEIPaperSearch.DataExtractors.BDLP
{
    internal class DblpDataExtractor : IJsonDataExtractor<ICollection<Article>>
    {
        public ICollection<Article> Extract(string source)
        {
            var dtos = DeserializeJson(source);
            return ToCanonicalModel(dtos);
        }

        ICollection<ArticleJsonDto> DeserializeJson(string source)
        {
            var root = JObject.Parse(source);

            IList<JToken> articlesJson = root["dblp"]!["article"]!.Children().ToList();
            IList<ArticleJsonDto> articleDtos = articlesJson.Select(a => a.ToObject<ArticleJsonDto>()!).ToList();

            return articleDtos;
        }

        ICollection<Article> ToCanonicalModel(ICollection<ArticleJsonDto> dtos)
        {
            var articles = new List<Article>();

            foreach (var dto in dtos)
            {
                var pages = ToPagePair(dto.Pages);

                var article = new Article(
                    dto.Title,
                    dto.Year,
                    url: ToUrl(dto.Ee),
                    startPage: pages.Item1,
                    endPage: pages.Item2);

                /* TODO article.Authors */
                /* TODO article.PublishedIn */

                articles.Add(article);
            }

            return articles;
        }

        string? ToUrl(dynamic? ee)
        {
            if (ee is null)
            {
                return null;
            }

            if (ee is string)
            {
                return ee;
            }

            if (ee is JObject && ee["content"]?.Value is string)
            {
                return ee["content"].Value;
            }

            if (ee is JArray)
            {
                var first = ee.First;
                if (first.Value is string)
                {
                    return first;
                }
                else
                {
                    var content = first["content"].Value;
                    return content is string ? content : null;
                }
            }

            throw new ArgumentException("EE has a invalid shape.");
        }

        (string?,string?) ToPagePair(string? pages)
        {
            if (pages is null)
            {
                return (null, null);
            }

            var tokens = pages.Split("-")
                .Select(s => s.Trim())
                .ToList();

            return (tokens[0], tokens.Count() > 1 ? tokens[1] : null);
        }
    }
}
