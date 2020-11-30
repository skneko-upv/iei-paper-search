using IEIPaperSearch.Models;
using IEIPaperSearch.Persistence;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IEIPaperSearch.DataExtractors.Bibtex
{
    internal class BibtexDataExtractor : IJsonDataExtractor<ICollection<Submission>>
    {
        private readonly PaperSearchContext context;

        public BibtexDataExtractor(PaperSearchContext context)
        {
            this.context = context;
        }

        public ICollection<Submission> Extract(string source) => DeserializeJson(source);

        ICollection<Submission> DeserializeJson(string source)
        {
            var root = JObject.Parse(source);

            IList<JToken> articlesJson = GetArrayOrObject(root["articles"]!);
            IList<JToken> booksJson = GetArrayOrObject(root["books"]!);
            IList<JToken> inProceedingsJson = GetArrayOrObject(root["inproceedings"]!);

            IList<BibtexJsonDto> articlesDtos = articlesJson.Select(a => {
                var dto = a.ToObject<BibtexJsonDto>()!;
                dto.Type = "article";
                return dto;
            }).ToList();
            IList<BibtexJsonDto> booksDtos = booksJson.Select(a => {
                var dto = a.ToObject<BibtexJsonDto>()!;
                dto.Type = "book";
                return dto;
            }).ToList();
            IList<BibtexJsonDto> inProceedingsDtos = inProceedingsJson.Select(a => {
                var dto = a.ToObject<BibtexJsonDto>()!;
                dto.Type = "inproceedings";
                return dto;
            }).ToList();

            IList<BibtexJsonDto> dtos = articlesDtos.Union(booksDtos).Union(inProceedingsDtos).ToList();

            return ToCanonicalModel(dtos);
        }

        IList<JToken> GetArrayOrObject(dynamic jsonObject) => 
            jsonObject is JArray ? ((JToken)jsonObject).Children().ToList() : new List<JToken>() { jsonObject };

        ICollection<Submission> ToCanonicalModel(ICollection<BibtexJsonDto> dtos)
        {
            var submissions = new List<Submission>();

            foreach (var dto in dtos)
            {
                var pages = ToPagePair(dto.Pages);

                switch (dto.Type) {
                    case "article":
                        {
                            var article = new Article(
                                dto.Title,
                                dto.Year,
                                url: null,
                                startPage: pages.Item1,
                                endPage: pages.Item2);

                            article.Authors = ToPeopleList(dto.Authors);
                            article.PublishedIn = ConsolidateIssueWithDatabase(article, dto);

                            submissions.Add(article);
                            break;
                        }
                    case "book":
                        {
                            var book = new Book(
                                dto.Title,
                                dto.Year,
                                url: null,
                                dto.Publisher!);

                            book.Authors = ToPeopleList(dto.Authors);

                            submissions.Add(book);
                            break;
                        }
                    case "inproceedings":
                        {
                            var inProceedings = new InProceedings(
                                dto.Title,
                                dto.Year,
                                url: null,
                                dto.BookTitle!,
                                edition: null,
                                startPage: pages.Item1 is null ? null : (int?)int.Parse(pages.Item1),
                                endPage: pages.Item2 is null ? null : (int?)int.Parse(pages.Item2));

                            inProceedings.Authors = ToPeopleList(dto.Authors);

                            submissions.Add(inProceedings);
                            break;
                        }
                }
            }

            return submissions;
        }

        private Issue ConsolidateIssueWithDatabase(Article article, BibtexJsonDto dto)
        {
            var journal = context.Journals.FirstOrDefault(j => j.Name == dto.Journal) ?? new Journal(dto.Journal!);
            var issue = journal.Issues.MatchingOrNew(new Issue(dto.Volume, dto.Number, month: null, journal));
            journal.Issues.Add(issue);

            issue.Articles.Add(article);

            return issue;
        }

        private ICollection<Person> ConsolidateAuthorsWithDatabase(Article article, ICollection<string> authorNames)
        {
            var authors = new List<Person>();

            foreach (var authorName in authorNames)
            {
                var person = context.People.MatchingOrNew(new Person(authorName));

                person.AuthorOf.Add(article);
                authors.Add(person);
            }

            return authors;
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

        (string?, string?) ToPagePair(string? pages)
        {
            if (pages is null)
            {
                return (null, null);
            }

            var tokens = pages.Split("--")
                .Select(s => s.Trim())
                .ToList();

            return (tokens[0], tokens.Count() > 1 ? tokens[1] : null);
        }

        ICollection<Person> ToPeopleList(string authors) => 
            authors.Split(',')
                .Select(p => p.Trim())
                .Select(name => new Person(name))
                .ToList();
    }
}