using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LaFlorida.Models;
using LaFlorida.Services;
using Microsoft.AspNetCore.Authorization;

namespace LaFlorida.Pages.Crops
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly ICropService _cropService;

        public DeleteModel(ICropService cropService)
        {
            _cropService = cropService;
        }

        [BindProperty]
        public Crop Crop { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index", new { error = true, message = "Cultivo no encontrado" });
            }

            Crop = await _cropService.GetCropByIdAsync((int)id);

            if (Crop == null)
            {
                return RedirectToPage("./Index", new { error = true, message = "Cultivo no encontrado" });
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index", new { error = true, message = "Cultivo no encontrado" });
            }

            var delete = await _cropService.DeleteCropAsync((int)id);
            if (!delete.Success)
            {
                ModelState.AddModelError("error", delete.Message);
                if (User.IsInRole("Admin"))
                {
                    ModelState.AddModelError("error", delete.Exception);
                }
                Crop = await _cropService.GetCropByIdAsync((int)id);
                return Page();
            }

            return RedirectToPage("./Index", new { success = true, message = "Cultivo borrado con exito" });
        }
    }
}
