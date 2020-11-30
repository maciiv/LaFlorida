using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LaFlorida.Models;
using LaFlorida.Services;
using Microsoft.AspNetCore.Authorization;

namespace LaFlorida.Pages.Costs
{
    [Authorize(Roles = "Admin, Manager")]
    public class DeleteModel : PageModel
    {
        private readonly ICostService _costService;

        public DeleteModel(ICostService costService)
        {
            _costService = costService;
        }

        [BindProperty]
        public Cost Cost { get; set; }
        [BindProperty]
        public int DashboardId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, int? previous = null)
        {
            if (id == null)
            {
                return RedirectToPage("./Index", new { error = true, message = "Costo no encontrado" });
            }

            Cost = await _costService.GetCostByIdAsync((int)id);

            if (Cost == null)
            {
                return RedirectToPage("./Index", new { error = true, message = "Costo no encontrado" });
            }

            DashboardId = previous ?? 0;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index", new { error = true, message = "Costo no encontrado" });
            }

            var delete = await _costService.DeleteCostAsync((int)id);
            if (!delete.Success)
            {
                ModelState.AddModelError("error", delete.Message);
                if (User.IsInRole("Admin"))
                {
                    ModelState.AddModelError("error", delete.Exception);
                }
                Cost = await _costService.GetCostByIdAsync((int)id);
                return Page();
            }

            if (DashboardId != 0)
                return RedirectToPage("../Dashboard", new { id = DashboardId, success = true, message = "Costo borrado con exito" });

            return RedirectToPage("./Index", new { success = true, message = "Costo borrado con exito" });
        }
    }
}
