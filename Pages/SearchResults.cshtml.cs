using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IEIPaperSearch.Models;
using IEIPaperSearch.Services.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IEIPaperSearch.Pages
{
    public class SearchResultsModel : PageModel
    {
        readonly ISearchService searchService;

        public SearchResultsModel(ISearchService searchService)
        {
            this.searchService = searchService;
        }

        [BindProperty(SupportsGet = true)]
        public ICollection<Submission> Results { get; set; }

        public void OnGet(
            [FromQuery] string? title,
            [FromQuery] string? author,
            [FromQuery] int? startingYear,
            [FromQuery] int? endYear,
            [FromQuery] bool findArticles,
            [FromQuery] bool findBooks,
            [FromQuery] bool findInProceedings)
        {
            Results = searchService.Search(title, author, startingYear, endYear,
                findArticles, findBooks, findInProceedings).ToList();
        }
    }
}
