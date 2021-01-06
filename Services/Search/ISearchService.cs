using IEIPaperSearch.Models;
using System;
using System.Collections.Generic;

namespace IEIPaperSearch.Services.Search
{
    public interface ISearchService
    {
        public IEnumerable<Submission> Search(string? title, string? author, int? startingYear, int? endYear, 
            bool findArticles, bool findBooks, bool findInProceedings);
    }
}
