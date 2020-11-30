using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LaFlorida.Models;
using LaFlorida.Services;
using Microsoft.AspNetCore.Authorization;

namespace LaFlorida.Pages.Lots
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ILotService _lotService;

        public IndexModel(ILotService lotService)
        {
            _lotService = lotService;
        }

        public IList<Lot> Lots { get;set; }
        public bool Success { get; set; } = false;
        public bool Error { get; set; } = false;
        public string Message { get; set; }

        public async Task OnGetAsync(bool success, bool error, string message)
        {
            Lots = await _lotService.GetLotsAsync();
            if (success) Success = true;
            if (error) Error = true;
            Message = message;
        }
    }
}
