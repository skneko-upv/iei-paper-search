using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using IEIPaperSearch.Models;
using IEIPaperSearch.Services.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IEIPaperSearch.Pages
{
    public class SearchModel : PageModel
    {
        readonly ISearchService searchService;

        public SearchModel(ISearchService searchService)
        {
            this.searchService = searchService;
        }

        [BindProperty]
        public string? Author { get; set; }

        [BindProperty]
        public string? Title { get; set; }

        [BindProperty]
        [Range(1000, 3000)]
        public uint? StartingYear { get; set; }

        [BindProperty]
        [Range(1000, 3000)]
        [GreaterThanOrEqualTo("StartingYear", ErrorMessage = "El año de final debe ser mayor o igual que el año de inicio.")]
        public uint? EndYear { get; set; }

        [BindProperty]
        public bool FindArticles { get; set; }

        [BindProperty]
        public bool FindBooks { get; set; }

        [BindProperty]
        public bool FindInProceedings { get; set; }

        [BindProperty]
        public bool FormLocked { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Author) && string.IsNullOrWhiteSpace(Title))
            {
                ModelState.AddModelError("", "Introduzca algún término de búsqueda.");
            }

            if (!FindBooks && !FindBooks && !FindInProceedings)
            {
                ModelState.AddModelError("", "Seleccione al menos un tipo de publicación a buscar.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            FormLocked = true;

            var results = searchService.Search(Title, Author, (int?)StartingYear, (int?)EndYear, FindArticles, FindBooks, FindInProceedings);
            foreach (var result in results)
            {
                Console.WriteLine(result);
            }

            return RedirectToPage("/SearchResults");
        }
    }
}
