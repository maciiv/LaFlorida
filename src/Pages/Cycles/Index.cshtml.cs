using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LaFlorida.Models;
using LaFlorida.Services;
using Microsoft.AspNetCore.Authorization;

namespace LaFlorida.Pages.Cycles
{
    [Authorize(Roles = "Admin, Manager")]
    public class IndexModel : PageModel
    {
        private readonly ICycleService _cycleService;

        public IndexModel(ICycleService cycleService)
        {
            _cycleService = cycleService;
        }

        public IList<Cycle> Cycles { get; set; }
        public bool Success { get; set; } = false;
        public bool Error { get; set; } = false;
        public string Message { get; set; }

        public async Task OnGetAsync(bool success, bool error, string message)
        {
            Cycles = await _cycleService.GetCyclesAsync();
            if (success) Success = true;
            if (error) Error = true;
            Message = message;
        }
    }
}
