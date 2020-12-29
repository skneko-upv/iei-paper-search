using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using IEIPaperSearch.Models;
using IEIPaperSearch.Persistence;

namespace IEIPaperSearch.Pages.Journals
{
    public class IndexModel : PageModel
    {
        private readonly IEIPaperSearch.Persistence.PaperSearchContext _context;

        public IndexModel(IEIPaperSearch.Persistence.PaperSearchContext context)
        {
            _context = context;
        }

        public IList<Journal> Journal { get;set; }

        public async Task OnGetAsync()
        {
            Journal = await _context.Journals.ToListAsync();
        }
    }
}
