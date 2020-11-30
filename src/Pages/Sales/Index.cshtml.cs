using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LaFlorida.Models;
using LaFlorida.Services;
using Microsoft.AspNetCore.Authorization;

namespace LaFlorida.Pages.Sales
{
    [Authorize(Roles = "Admin, Manager")]
    public class IndexModel : PageModel
    {
        private readonly ISaleService _saleService;

        public IndexModel(ISaleService saleService)
        {
            _saleService = saleService;
        }

        public IList<Sale> Sales { get; set; }
        public bool Success { get; set; } = false;
        public bool Error { get; set; } = false;
        public string Message { get; set; }

        public async Task OnGetAsync(bool success, bool error, string message)
        {
            Sales = await _saleService.GetSalesAsync();
            if (success) Success = true;
            if (error) Error = true;
            Message = message;
        }
    }
}
