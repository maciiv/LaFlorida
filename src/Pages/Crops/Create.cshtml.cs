using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LaFlorida.Models;
using LaFlorida.Services;
using Microsoft.AspNetCore.Authorization;

namespace LaFlorida.Pages.Crops
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly ICropService _cropService;

        public CreateModel(ICropService cropService)
        {
            _cropService = cropService;
        }

        [BindProperty]
        public Crop Crop { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var create = await _cropService.CreateCropAsync(Crop);
            if (!create.Success)
            {
                ModelState.AddModelError("error", create.Message);
                return Page();
            }

            return RedirectToPage("./Index", new { success = true, message = "Cultivo creado con exito" });
        }
    }
}
