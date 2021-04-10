using System.Collections.Generic;
using System.Linq;
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
        private readonly ILotService _lotService;

        public CurrentCyclesModel(ICycleService cycleService, ILotService lotService)
        {
            _cycleService = cycleService;
            _lotService = lotService;
        }

        public IList<Cycle> Cycles { get; set; }
        public bool Success { get; set; } = false;
        public bool Error { get; set; } = false;
        public string Message { get; set; }
        public IList<Lot> EmptyLots { get; set; }

        public async Task<IActionResult> OnGetAsync(bool success, bool error, string message)
        {
            Cycles = await _cycleService.GetActiveCyclesAsync();
            if (success) Success = true;
            if (error) Error = true;
            Message = message;

            var lots = await _lotService.GetLotsAsync();
            EmptyLots = lots.Where(c => !Cycles.Select(d => d.LotId).Contains(c.LotId)).ToList();

            return Page();
        }
    }
}
