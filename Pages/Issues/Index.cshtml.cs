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
    public class IndexModel : PageModel
    {
        private readonly IEIPaperSearch.Persistence.PaperSearchContext _context;

        public IndexModel(IEIPaperSearch.Persistence.PaperSearchContext context)
        {
            _context = context;
        }

        public IList<Issue> Issue { get;set; }

        public async Task OnGetAsync()
        {
            Issue = await _context.Issues.ToListAsync();
        }
    }
}
