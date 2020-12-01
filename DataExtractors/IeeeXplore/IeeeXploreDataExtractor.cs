using IEIPaperSearch.Models;
using IEIPaperSearch.Persistence;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace IEIPaperSearch.DataExtractors.IeeeXplore
{
    internal class IeeeXploreDataExtractor : IJsonDataExtractor<ICollection<Submission>>
    {
        private readonly PaperSearchContext context;

        private readonly List<Article> articles = new List<Article>();
        private readonly List<Journal> journals = new List<Journal>();
        private readonly List<Person> people = new List<Person>();
        private readonly List<Issue> issues = new List<Issue>();
        private readonly List<Book> books = new List<Book>();
        private readonly List<InProceedings> inProceedings = new List<InProceedings>();

        public IeeeXploreDataExtractor(PaperSearchContext context)
        {
            this.context = context;
        }

        public ICollection<Submission> Extract(string source)
        {
            var dtos = DeserializeJson(source);
            ToCanonicalModel(dtos);
            return articles.Select(a => (Submission)a).Union(books).Union(inProceedings).ToList();
        }

        ICollection<SubmissionJsonDto> DeserializeJson(string source)
        {
            var root = JObject.Parse(source);

            IList<JToken> submissionsJson = root["articles"]!.Children().ToList();

            IList<SubmissionJsonDto> submissionsDtos = submissionsJson.Select(s => s.ToObject<SubmissionJsonDto>()!).ToList();

            return submissionsDtos;
        }

        void ToCanonicalModel(ICollection<SubmissionJsonDto> dtos)
        {
            foreach (var dto in dtos)
            {
                switch (dto.ContentType)
                {
                    case "Early Access Articles":
                    case "Journals":
                        {
                            var article = new Article(
                                dto.Title,
                                dto.PublicationYear,
                                dto.PdfUrl,
                                startPage: dto.StartPage,
                                endPage: dto.EndPage);

                            article.Authors = ConsolidateAuthorsWithDatabase(ToPeopleList(dto.Authors.Authors));
                            article.PublishedIn = new Issue(
                                dto.Volume, 
                                dto.PublicationNumber, 
                                month: ToMonth(dto.PublicationDate),
                                new Journal(dto.PublicationTitle));

                            articles.Add(article);
                            break;
                        }
                    case "Conferences":
                        {
                            var inProceedingsElement = new InProceedings(
                                dto.Title,
                                dto.PublicationYear,
                                dto.PdfUrl,
                                conference: dto.PublicationTitle,
                                edition: dto.ConferenceDates,
                                dto.StartPage,
                                dto.EndPage);

                            inProceedingsElement.Authors = ConsolidateAuthorsWithDatabase(ToPeopleList(dto.Authors.Authors));

                            inProceedings.Add(inProceedingsElement);
                            break;
                        }
                }
            }
        }

        private Issue? ConsolidateIssueWithDatabase(SubmissionJsonDto dto)
        {
            if (dto.PublicationTitle is null) return null;

            var journal = journals.MatchingOrNew(new Journal(dto.PublicationTitle));
            journals.Add(journal);

            var issue = journal.Issues.MatchingOrNew(new Issue(dto.Volume, dto.PublicationNumber, month: null, journal));
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

        ICollection<string> ToPeopleList(ICollection<AuthorJsonDto> authors) =>
            authors
                .Select(a => a.FullName)
                .ToList();

        string? ToMonth(string? month)
        {
            if (month is null) return null;
            return string.Join(", ", month.Split(new char[] { ' ', '-', '.' })
                .Where(s => s.All(c => char.IsLetter(c))));
        }
    }
}
