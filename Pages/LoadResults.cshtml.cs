using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static IEIPaperSearch.Services.DataLoaders.IDataLoaderService;

namespace IEIPaperSearch.Pages
{
    public class LoadResultsModel : PageModel
    {
        public DataLoaderResult? dblpResult;
        public DataLoaderResult? ieeeXploreResult;
        public DataLoaderResult? googleScholarResult;

        public DataLoaderResult combinedResult;

        
        [BindProperty(SupportsGet = true)]
        public int DblpCount { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public int IeeeXploreCount { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public int GoogleScholarCount { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public int CombinedCount { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public int ErrorCount { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public ICollection<string> ErrorList { get; set; }

        public void OnGet()
        {
            dblpResult = TempData.Get<DataLoaderResult>("LoadResults.Dblp");
            ieeeXploreResult = TempData.Get<DataLoaderResult>("LoadResults.IeeeXplore");
            googleScholarResult = TempData.Get<DataLoaderResult>("LoadResults.GoogleScholar");

            combinedResult = new DataLoaderResult(0);
            if (dblpResult is not null)
            {
                combinedResult += dblpResult;
            }
            if (ieeeXploreResult is not null)
            {
                combinedResult += ieeeXploreResult;
            }
            if (googleScholarResult is not null)
            {
                combinedResult += googleScholarResult;
            }

            DblpCount = dblpResult?.InsertedSubmissionCount ?? 0;
            IeeeXploreCount = ieeeXploreResult?.InsertedSubmissionCount ?? 0;
            GoogleScholarCount = googleScholarResult?.InsertedSubmissionCount ?? 0;
            CombinedCount = combinedResult.InsertedSubmissionCount;

            ErrorCount = combinedResult.Errors.Count;

            ErrorList = combinedResult.Errors;
        }
    }
}
