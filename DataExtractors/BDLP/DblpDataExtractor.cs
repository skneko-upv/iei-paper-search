using IEIPaperSearch.Models;
using IEIPaperSearch.Persistence;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IEIPaperSearch.DataExtractors.BDLP
{
    internal class DblpDataExtractor : AbstractSubmissionDataExtractor<ArticleJsonDto>
    {
        public DblpDataExtractor(PaperSearchContext context) : base(context)
        { }

        protected override ICollection<ArticleJsonDto> DeserializeJson(JObject root)
        {
            IList<JToken> articlesJson = root["dblp"]!["article"]!.Children().ToList();
            IList<ArticleJsonDto> articleDtos = articlesJson.Select(a => a.ToObject<ArticleJsonDto>()!).ToList();

            return articleDtos;
        }

        protected override void ToCanonicalModel(ICollection<ArticleJsonDto> dtos)
        {
            foreach (var dto in dtos)
            {
                var pages = ToPagePair(dto.Pages);

                var article = new Article(
                    dto.Title,
                    dto.Year,
                    url: ToUrl(dto.Ee),
                    startPage: pages.Item1,
                    endPage: pages.Item2)
                {
                    Authors = MakeAuthors((ICollection<string>)ToPeopleList(dto.Authors)),
                    PublishedIn = MakeIssueAndJournal(dto.Volume, dto.Number, month: null, dto.Journal)
                };

                articles.Add(article);
            }
        }

        private static string? ToUrl(dynamic? ee)
        {
            var test = (ICollection<string>)ExtractContentList(ee, "$");
            return test.FirstOrDefault();
        }

        private static (string?,string?) ToPagePair(string? pages)
        {
            if (pages is null)
            {
                return (null, null);
            }

            var tokens = pages.Split("-")
                .Select(s => s.Trim())
                .ToList();

            return (tokens[0], tokens.Count > 1 ? tokens[1] : null);
        }

        private ICollection<string> ToPeopleList(dynamic? authors) => ExtractContentList(authors, "$");

        private static ICollection<string> ExtractContentList(dynamic property, string contentPropertyName = "content")
        {
            var list = new List<string>();

            if (property is null)
            {
                return list;
            }

            if (property is string)
            {
                list.Add(property);
                return list;
            }

            if (property is JObject && property[contentPropertyName]?.Value is string)
            {
                list.Add(property[contentPropertyName].Value);
                return list;
            }

            if (property is JArray)
            {
                foreach (var candidate in property)
                {
                    if (candidate!.Value is string)
                    {
                        list.Add(candidate!.Value);
                    }
                    else
                    {
                        var content = candidate[contentPropertyName].Value;
                        list.Add(content is string ? content : null);
                    }
                }

                return list;
            }

            throw new ArgumentException($"Property {property} has an unexpected shape.");
        }
    }
}
