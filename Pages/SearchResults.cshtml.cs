using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IEIPaperSearch.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IEIPaperSearch.Pages
{
    public class SearchResultsModel : PageModel
    {
        

        [BindProperty(SupportsGet = true)]
        public ICollection<Submission> Results { get; set; }

        public void OnGet()
        {
            Results = (ICollection<Submission>)TempData.Get<List<dynamic>>("SearchResults")!;
            // Coger resultados -> Results
            //  Aquí:
            //TempData.Get<ICollection<Submission>>("SearchResults");
            //  En Search:
            //TempData.Put<ICollection<Submission>>("SearchResults", searchService.Search(...));
        }
    }
}
