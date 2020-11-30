using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LaFlorida.Models;
using LaFlorida.Services;
using Microsoft.AspNetCore.Authorization;

namespace LaFlorida.Pages.Lots
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly ILotService _lotService;

        public EditModel(ILotService lotService)
        {
            _lotService = lotService;
        }

        [BindProperty]
        public Lot Lot { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index", new { error = true, message = "Lote no encontrado" });
            }

            Lot = await _lotService.GetLotByIdAsync((int)id);

            if (Lot == null)
            {
                return RedirectToPage("./Index", new { error = true, message = "Lote no encontrado" });
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var edit = await _lotService.EditLotAsync(Lot);
            if (!edit.Success)
            {
                ModelState.AddModelError("error", edit.Message);
                return Page();
            }

            return RedirectToPage("./Index", new { success = true, message = "Lote editado con exito" });
        }
    }
}
