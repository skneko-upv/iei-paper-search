using IEIPaperSearch.Models;
using IEIPaperSearch.Persistence;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace IEIPaperSearch.DataExtractors
{
    internal abstract class AbstractSubmissionDataExtractor<Dto> : IJsonDataExtractor<SubmissionDataExtractorResult>
    {
        private readonly PaperSearchContext context;

        protected readonly ISet<Article> articles = new HashSet<Article>();
        protected readonly ISet<Book> books = new HashSet<Book>();
        protected readonly ISet<InProceedings> inProceedings = new HashSet<InProceedings>();
        protected readonly ISet<Person> people = new HashSet<Person>();
        protected readonly ISet<Issue> issues = new HashSet<Issue>();
        protected readonly ISet<Journal> journals = new HashSet<Journal>();

        public AbstractSubmissionDataExtractor(PaperSearchContext context)
        {
            this.context = context;
            people.UnionWith(context.People);
        }

        public SubmissionDataExtractorResult Extract(string source)
        {
            var root = JObject.Parse(source);
            if (root is null)
            {
                throw new ArgumentException("Source is not valid JSON.", nameof(source));
            }

            var dtos = DeserializeJson(root);

            ToCanonicalModel(dtos);

            return new SubmissionDataExtractorResult(articles, books, inProceedings);
        }

        protected abstract ICollection<Dto> DeserializeJson(JObject root);

        protected abstract void ToCanonicalModel(ICollection<Dto> dtos);

        protected Issue? MakeIssueAndJournal(string? volume, string? number, string? month, string? journalName)
        {
            if (string.IsNullOrWhiteSpace(journalName))
            {
                return null;
            }

            var journal = context.Journals.MatchingOrNew(journals, new Journal(journalName));
            journals.Add(journal);

            var issue = context.Issues.MatchingOrNew(issues, new Issue(volume, number, month, journal));
            issues.Add(issue);

            return issue;
        }

        protected ICollection<Person> MakeAuthors(ICollection<string> authorNames)
        {
            var authors = new List<Person>();

            foreach (var authorName in authorNames)
            {
                var person = context.People.MatchingOrNew(people, new Person(authorName));
                people.Add(person);

                authors.Add(person);
            }

            return authors;
        }
    }

    internal class SubmissionDataExtractorResult
    {
        public ICollection<Article> Articles { get; }

        public ICollection<Book> Books { get; }

        public ICollection<InProceedings> InProceedings { get; }

        public SubmissionDataExtractorResult(ICollection<Article> articles, ICollection<Book> books, ICollection<InProceedings> inProceedings)
        {
            Articles = articles;
            Books = books;
            InProceedings = inProceedings;
        }
    }
}
