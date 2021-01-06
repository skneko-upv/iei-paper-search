using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    
        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public bool LoadFromDblp { get; set; }
        [BindProperty]
        public bool LoadFromIeeeXplore { get; set; }
        [BindProperty]
        public bool LoadFromGoogleScholar { get; set; }

        [BindProperty]
        [Range(1000,3000)]
        public uint StartingYear { get; set; }
        [BindProperty]
        [Range(1000,3000)]
        [GreaterThanOrEqualTo("StartingYear", ErrorMessage = "El año de final debe ser mayor o igual que el año de inicio.")]
        public uint EndYear { get; set; }

        [BindProperty]
        public bool FormLocked { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine("Form sent");

            if (!LoadFromDblp && !LoadFromIeeeXplore && !LoadFromGoogleScholar)
            {
                ModelState.AddModelError("", "Selecciona al menos una fuente.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            FormLocked = true;

            await LoadFromSelectedSources();

            TempData["test"] = 3;

            return RedirectToPage("/LoadResults");
        }

        async Task LoadFromSelectedSources()
        {
            if (LoadFromDblp)
            {
                loaderService.LoadFromDblp();
            }
            if (LoadFromIeeeXplore)
            {
                loaderService.LoadFromIeeeXplore();
            }
            if (LoadFromGoogleScholar)
            {
                loaderService.LoadFromGoogleScholar();
            }
        }
    }
}
