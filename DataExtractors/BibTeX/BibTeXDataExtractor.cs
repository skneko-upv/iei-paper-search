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

        private readonly List<Article> articles = new List<Article>();
        private readonly List<Journal> journals = new List<Journal>();
        private readonly List<Person> people = new List<Person>();
        private readonly List<Issue> issues = new List<Issue>();
        private readonly List<Book> books = new List<Book>();
        private readonly List<InProceedings> inProceedings = new List<InProceedings>();

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

            ToCanonicalModel(dtos);
            return articles.Select(a => (Submission)a).Union(books).Union(inProceedings).ToList();
        }

        IList<JToken> GetArrayOrObject(dynamic jsonObject) => 
            jsonObject is JArray ? ((JToken)jsonObject).Children().ToList() : new List<JToken>() { jsonObject };

        void ToCanonicalModel(ICollection<SubmissionJsonDto> dtos)
        {
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

                            article.Authors = ConsolidateAuthorsWithDatabase(ToPeopleList(dto.Authors));
                            article.PublishedIn = ConsolidateIssueWithDatabase(dto);

                            articles.Add(article);
                            break;
                        }
                    case "book":
                        {
                            var book = new Book(
                                dto.Title,
                                dto.Year,
                                url: null,
                                dto.Publisher!);

                            book.Authors = ConsolidateAuthorsWithDatabase(ToPeopleList(dto.Authors));

                            books.Add(book);
                            break;
                        }
                    case "inproceedings":
                        {
                            var inProceedingsElement = new InProceedings(
                                dto.Title,
                                dto.Year,
                                url: null,
                                dto.BookTitle!,
                                edition: null,
                                startPage: pages.Item1 is null ? null : pages.Item1,
                                endPage: pages.Item2 is null ? null : pages.Item2);

                            inProceedingsElement.Authors = ConsolidateAuthorsWithDatabase(ToPeopleList(dto.Authors));

                            inProceedings.Add(inProceedingsElement);
                            break;
                        }
                }
            }
        }
        private Issue? ConsolidateIssueWithDatabase(SubmissionJsonDto dto)
        {
            if (dto.Journal is null) return null;

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

        ICollection<string> ToPeopleList(string authors) => 
            authors.Split(',')
                .Select(p => p.Trim())
                .ToList();
    }
}