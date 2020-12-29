using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using IEIPaperSearch.Models;
using IEIPaperSearch.Persistence;

namespace IEIPaperSearch.Pages.InProceedings
{
    public class DeleteModel : PageModel
    {
        private readonly IEIPaperSearch.Persistence.PaperSearchContext _context;

        public DeleteModel(IEIPaperSearch.Persistence.PaperSearchContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.InProceedings InProceedings { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InProceedings = await _context.InProceedings.FirstOrDefaultAsync(m => m.Id == id);

            if (InProceedings == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InProceedings = await _context.InProceedings.FindAsync(id);

            if (InProceedings != null)
            {
                _context.InProceedings.Remove(InProceedings);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
