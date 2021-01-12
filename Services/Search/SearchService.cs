using IEIPaperSearch.Models;
using IEIPaperSearch.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace IEIPaperSearch.Services.Search
{
    public class SearchService : ISearchService
    {
        readonly PaperSearchContext context;

        public SearchService(PaperSearchContext context)
        {
            this.context = context;
        }

        public IEnumerable<Submission> Search(string? title, string? author, int? startingYear, int? endYear, 
            bool findArticles, bool findBooks, bool findInProceedings)
        {
            ISet<Submission> articles = new HashSet<Submission>();
            if (findArticles)
            {
                articles.UnionWith(SearchByType<DbSet<Article>, Article>(context.Articles, title, author, startingYear, endYear)
                    .Include("Authors")
                    .Include("PublishedIn")
                    .Include(e => ((Article)e).PublishedIn!.Journal)
                    .ToHashSet());
            }

            ISet<Submission> books = new HashSet<Submission>();
            if (findBooks)
            {
                books.UnionWith(SearchByType<DbSet<Book>, Book>(context.Books, title, author, startingYear, endYear)
                    .Include("Authors")
                    .ToHashSet());
            }

            ISet<Submission> inProceedings = new HashSet<Submission>();
            if (findInProceedings)
            {
                inProceedings.UnionWith(SearchByType<DbSet<InProceedings>, InProceedings>(context.InProceedings, title, author, startingYear, endYear)
                    .Include("Authors")
                    .ToHashSet());
            }

            return articles.Union(books).Union(inProceedings);
        }

        IQueryable<Submission> SearchByType<S, T>(S set, string? title, string? author, int? startingYear, int? endYear)
            where S : DbSet<T>
            where T : Submission
        {
            var query = (IQueryable<Submission>)set;
            if (title is not null)
            {
                query = query.Where(e => e.Title.Contains(title));
            }
            if (author is not null)
            {
                query = query.Where(e => e.Authors.Any(a => a.Name.Contains(author) || a.Surnames.Contains(author)));
            }
            if (startingYear is not null)
            {
                query = query.Where(e => e.Year >= startingYear);
            }
            if (endYear is not null)
            {
                query = query.Where(e => e.Year <= endYear);
            }

            return query;
        }
    }
}
