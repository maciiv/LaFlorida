using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LaFlorida.Models;
using LaFlorida.Services;
using Microsoft.AspNetCore.Authorization;

namespace LaFlorida.Pages.Costs
{
    [Authorize(Roles = "Admin, Manager")]
    public class IndexModel : PageModel
    {
        private readonly ICostService _costService;

        public IndexModel(ICostService costService)
        {
            _costService = costService;
        }

        public IList<Cost> Costs { get; set; }
        public bool Success { get; set; } = false;
        public bool Error { get; set; } = false;
        public string Message { get; set; }

        public async Task OnGetAsync(bool success, bool error, string message)
        {
            Costs = await _costService.GetCostsAsync();
            if (success) Success = true;
            if (error) Error = true;
            Message = message;
        }
    }
}
