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
        [BindProperty]
        public string? Author { get; set; }

        [BindProperty]
        public string? Title { get; set; }

        [BindProperty]
        [Range(1000, 3000)]
        public uint? StartingYear { get; set; }

        [BindProperty]
        [Range(1000, 3000)]
        [GreaterThanOrEqualTo("StartingYear", ErrorMessage = "El a�o de final debe ser mayor o igual que el a�o de inicio.")]
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
            if (string.IsNullOrWhiteSpace(Author) && string.IsNullOrWhiteSpace(Title) && StartingYear is null && EndYear is null)
            {
                ModelState.AddModelError("", "Introduzca alg�n t�rmino de b�squeda.");
            }

            if (!FindArticles && !FindBooks && !FindInProceedings)
            {
                ModelState.AddModelError("", "Seleccione al menos un tipo de publicaci�n a buscar.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            FormLocked = true;

            return RedirectToPage("/SearchResults", new { 
                title = Title, 
                author = Author, 
                startingYear = StartingYear, 
                endYear = EndYear, 
                findArticles = FindArticles, 
                findBooks = FindBooks, 
                findInProceedings = FindInProceedings });
        }
    }
}
