using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LaFlorida.Models;
using LaFlorida.Services;
using LaFlorida.PageModels;
using Microsoft.AspNetCore.Authorization;

namespace LaFlorida.Pages.Sales
{
    [Authorize(Roles = "Admin, Manager")]
    public class EditModel : SalesPageModel
    {
        public EditModel(ISaleService saleService, ICycleService cycleService)
            :base(saleService, cycleService)
        {
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

            await SetSelectLists();

            DashboardId = previous ?? 0;

            if (previous == null)
            {
                return Page();
            }

            await HasCycle(previous);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, int? previous = null)
        {
            if (!ModelState.IsValid)
            {
                await SetSelectLists();
                await HasCycle(previous);
                return Page();
            }

            var edit = await _saleService.EditSaleAsync(Sale);
            if (!edit.Success)
            {
                ModelState.AddModelError("error", edit.Message);
                await SetSelectLists();
                await HasCycle(previous);
                return Page();
            }

            if (DashboardId != 0)
                return RedirectToPage("../Dashboard", new { id = DashboardId, success = true, message = "Venta editada con exito" });

            return RedirectToPage("./Index", new { success = true, message = "Venta editada con exito" });
        }
    }
}
