using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LaFlorida.Models;
using LaFlorida.Services;
using Microsoft.AspNetCore.Authorization;

namespace LaFlorida.Pages.Crops
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly ICropService _cropService;

        public EditModel(ICropService cropService)
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var edit = await _cropService.EditCropAsync(Crop);
            if (!edit.Success)
            {
                ModelState.AddModelError("error", edit.Message);
                return Page();
            }

            return RedirectToPage("./Index", new { success = true, message = "Cultivo editado con exito" });
        }
    }
}
