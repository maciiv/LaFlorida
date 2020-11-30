using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LaFlorida.Models;
using LaFlorida.Services;
using Microsoft.AspNetCore.Authorization;

namespace LaFlorida.Pages.Sales
{
    [Authorize(Roles = "Admin, Manager")]
    public class DeleteModel : PageModel
    {
        private readonly ISaleService _saleService;

        public DeleteModel(ISaleService saleService)
        {
            _saleService = saleService;
        }

        [BindProperty]
        public Sale Sale { get; set; }
        [BindProperty]
        public int DashboardId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, int? previous = null)
        {
            if (id == null)
            {
                return RedirectToPage("./Index", new { error = true, message = "Venta no encontrada" });
            }

            Sale = await _saleService.GetSaleByIdAsync((int)id);

            if (Sale == null)
            {
                return RedirectToPage("./Index", new { error = true, message = "Venta no encontrada" });
            }

            DashboardId = previous ?? 0;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index", new { error = true, message = "Venta no encontrada" });
            }

            var delete = await _saleService.DeleteSaleAsync((int)id);
            if (!delete.Success)
            {
                ModelState.AddModelError("error", delete.Message);
                if (User.IsInRole("Admin"))
                {
                    ModelState.AddModelError("error", delete.Exception);
                }
                Sale = await _saleService.GetSaleByIdAsync((int)id);
                return Page();
            }

            if (DashboardId != 0)
                return RedirectToPage("../Dashboard", new { id = DashboardId, success = true, message = "Venta borrada con exito" });

            return RedirectToPage("./Index", new { success = true, message = "Venta borrada con exito" });
        }
    }
}
