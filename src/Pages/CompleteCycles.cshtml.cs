using System.Collections.Generic;
using System.Threading.Tasks;
using LaFlorida.Models;
using LaFlorida.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaFlorida.Pages
{
    [Authorize]
    public class CompleteCyclesModel : PageModel
    {
        private readonly ICycleService _cycleService;

        public CompleteCyclesModel(ICycleService cycleService)
        {
            _cycleService = cycleService;
        }

        public IList<Cycle> Cycles { get; set; }

        public async Task OnGet()
        {
            Cycles = await _cycleService.GetCompleteCyclesAsync();
        }
    }
}