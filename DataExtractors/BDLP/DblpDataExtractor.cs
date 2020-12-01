using IEIPaperSearch.Models;
using IEIPaperSearch.Persistence;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IEIPaperSearch.DataExtractors.BDLP
{
    internal class DblpDataExtractor : IJsonDataExtractor<ICollection<Article>>
    {
        private readonly PaperSearchContext context;

        private readonly List<Article> articles = new List<Article>();
        private readonly List<Journal> journals = new List<Journal>();
        private readonly List<Person> people = new List<Person>();
        private readonly List<Issue> issues = new List<Issue>();

        public DblpDataExtractor(PaperSearchContext context)
        {
            this.context = context;
        }

        public ICollection<Article> Extract(string source)
        {
            var dtos = DeserializeJson(source);
            ToCanonicalModel(dtos);
            return articles;
        }

        static ICollection<ArticleJsonDto> DeserializeJson(string source)
        {
            var root = JObject.Parse(source);

            IList<JToken> articlesJson = root["dblp"]!["article"]!.Children().ToList();
            IList<ArticleJsonDto> articleDtos = articlesJson.Select(a => a.ToObject<ArticleJsonDto>()!).ToList();

            return articleDtos;
        }

        void ToCanonicalModel(ICollection<ArticleJsonDto> dtos)
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
                    Authors = ConsolidateAuthorsWithDatabase((ICollection<string>)ToPeopleList(dto.Authors)),
                    PublishedIn = ConsolidateIssueWithDatabase(dto)
                };

                articles.Add(article);
            }
        }

        private Issue ConsolidateIssueWithDatabase(ArticleJsonDto dto)
        {
            var journal = journals.MatchingOrNew(new Journal(dto.Journal));
            journals.Add(journal);

            var issue = journal.Issues.MatchingOrNew(new Issue(dto.Volume, dto.Number, month: null, journal));
            issues.Add(issue);

            return issue;
        }

        private ICollection<Person> ConsolidateAuthorsWithDatabase(ICollection<string> authorNames)
        {
            var authors = new List<Person>();

            foreach (var authorName in authorNames)
            {
                var person = people.MatchingOrNew(new Person(authorName));
                people.Add(person);

                authors.Add(person);
            }

            return authors;
        }

        string? ToUrl(dynamic? ee)
        {
            var test = (ICollection<string>)ExtractContentList(ee, "$");
            return test.FirstOrDefault();
        }

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

        static (string?,string?) ToPagePair(string? pages)
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

        ICollection<string> ToPeopleList(dynamic? authors) => ExtractContentList(authors, "$");
    }
}
