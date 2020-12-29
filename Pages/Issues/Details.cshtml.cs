using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using IEIPaperSearch.Models;
using IEIPaperSearch.Persistence;

namespace IEIPaperSearch.Pages.Issues
{
    public class DetailsModel : PageModel
    {
        private readonly IEIPaperSearch.Persistence.PaperSearchContext _context;

        public DetailsModel(IEIPaperSearch.Persistence.PaperSearchContext context)
        {
            _context = context;
        }

        public Issue Issue { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Issue = await _context.Issues.FirstOrDefaultAsync(m => m.Id == id);

            if (Issue == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
