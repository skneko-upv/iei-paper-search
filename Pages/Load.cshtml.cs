using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IEIPaperSearch.Services.DataLoaders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IEIPaperSearch.Pages
{
    public class LoadModel : PageModel
    {
        readonly IDataLoaderService loaderService;
        public LoadModel(IDataLoaderService loaderService)
        {
            this.loaderService = loaderService;
        }
    
        public void OnGet()
        {
        }
    }
}
