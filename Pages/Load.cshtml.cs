using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using IEIPaperSearch.Services.DataLoaders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static IEIPaperSearch.Services.DataLoaders.IDataLoaderService;

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

            return RedirectToPage("/LoadResults");
        }

        Task LoadFromSelectedSources()
        {
            return Task.Run(() =>
            {
                if (LoadFromDblp)
                {
                    TempData.Put<DataLoaderResult>("LoadResults.Dblp", 
                        ExtractFromOneSource(loaderService.LoadFromDblp, "DBLP"));
                }
                if (LoadFromIeeeXplore)
                {
                    TempData.Put<DataLoaderResult>("LoadResults.IeeeXplore", 
                        ExtractFromOneSource(loaderService.LoadFromIeeeXplore, "IEEE Xplore"));
                }
                if (LoadFromGoogleScholar)
                {
                    TempData.Put<DataLoaderResult>("LoadResults.GoogleScholar", 
                        ExtractFromOneSource(loaderService.LoadFromGoogleScholar, "Google Scholar"));
                }
            });
        }

        delegate DataLoaderResult ExtractionFunction();

        DataLoaderResult ExtractFromOneSource(ExtractionFunction extract, string errorDiscriminator)
        {
            DataLoaderResult result = new DataLoaderResult(0);
            var errors = new List<string>();
            try
            {
                result = extract();
            }
            catch (Exception e)
            {
                errors.Add($"El extractor {errorDiscriminator} ha fallado: {e.Message}");
            }

            result.AddAllErrors(errors);
            return result;
        }
    }
}
