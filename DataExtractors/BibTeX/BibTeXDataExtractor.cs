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

            IList<SubmissionJsonDto> articlesDtos = articlesJson.Select(a => {
                var dto = a.ToObject<SubmissionJsonDto>()!;
                dto.Type = "article";
                return dto;
            }).ToList();
            IList<SubmissionJsonDto> booksDtos = booksJson.Select(a => {
                var dto = a.ToObject<SubmissionJsonDto>()!;
                dto.Type = "book";
                return dto;
            }).ToList();
            IList<SubmissionJsonDto> inProceedingsDtos = inProceedingsJson.Select(a => {
                var dto = a.ToObject<SubmissionJsonDto>()!;
                dto.Type = "inproceedings";
                return dto;
            }).ToList();

            IList<SubmissionJsonDto> dtos = articlesDtos.Union(booksDtos).Union(inProceedingsDtos).ToList();

            return ToCanonicalModel(dtos);
        }

        IList<JToken> GetArrayOrObject(dynamic jsonObject) => 
            jsonObject is JArray ? ((JToken)jsonObject).Children().ToList() : new List<JToken>() { jsonObject };

        ICollection<Submission> ToCanonicalModel(ICollection<SubmissionJsonDto> dtos)
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
                                startPage: pages.Item1 is null ? null : pages.Item1,
                                endPage: pages.Item2 is null ? null : pages.Item2);

                            inProceedings.Authors = ToPeopleList(dto.Authors);

                            submissions.Add(inProceedings);
                            break;
                        }
                }
            }

            return submissions;
        }

        private Issue ConsolidateIssueWithDatabase(Article article, SubmissionJsonDto dto)
        {
            var journal = context.Journals.FirstOrDefault(j => j.Name == dto.Journal) ?? new Journal(dto.Journal!);
            var issue = journal.Issues.MatchingOrNew(new Issue(dto.Volume, dto.Number, month: null, journal));
            journal.Issues.Add(issue);

            issue.Articles.Add(article);

            return issue;
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