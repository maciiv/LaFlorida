using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LaFlorida.Models;
using LaFlorida.Services;
using Microsoft.AspNetCore.Authorization;

namespace LaFlorida.Pages.Lots
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly ILotService _lotService;

        public DeleteModel(ILotService lotService)
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index", new { error = true, message = "Lote no encontrado" });
            }

            var delete = await _lotService.DeleteLotAsync((int)id);
            if (!delete.Success)
            {
                ModelState.AddModelError("error", delete.Message);
                if (User.IsInRole("Admin"))
                {
                    ModelState.AddModelError("error", delete.Exception);
                }
                Lot = await _lotService.GetLotByIdAsync((int)id);
                return Page();
            }

            return RedirectToPage("./Index", new { success = true, message = "Lote borrado con exito" });
        }
    }
}
