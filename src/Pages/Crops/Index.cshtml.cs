using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LaFlorida.Models;
using LaFlorida.Services;
using Microsoft.AspNetCore.Authorization;

namespace LaFlorida.Pages.Crops
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ICropService _cropService;

        public IndexModel(ICropService cropService)
        {
            _cropService = cropService;
        }

        public IList<Crop> Crops { get; set; }
        public bool Success { get; set; } = false;
        public bool Error { get; set; } = false;
        public string Message { get; set; }

        public async Task OnGetAsync(bool success, bool error, string message)
        {
            Crops = await _cropService.GetCropsAsync();
            if (success) Success = true;
            if (error) Error = true;
            Message = message;
        }
    }
}
