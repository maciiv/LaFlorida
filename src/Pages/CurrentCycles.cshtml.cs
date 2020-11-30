using System.Collections.Generic;
using System.Threading.Tasks;
using LaFlorida.Models;
using LaFlorida.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaFlorida.Pages
{
    [Authorize(Roles = "Admin, Manager")]
    public class CurrentCyclesModel : PageModel
    {
        private readonly ICycleService _cycleService;

        public CurrentCyclesModel(ICycleService cycleService)
        {
            _cycleService = cycleService;
        }

        public IList<Cycle> Cycles { get; set; }
        public bool Success { get; set; } = false;
        public bool Error { get; set; } = false;
        public string Message { get; set; }

        public async Task<IActionResult> OnGetAsync(bool success, bool error, string message)
        {
            Cycles = await _cycleService.GetActiveCyclesAsync();
            if (success) Success = true;
            if (error) Error = true;
            Message = message;

            return Page();
        }
    }
}
