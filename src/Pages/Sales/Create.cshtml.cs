using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LaFlorida.Models;
using LaFlorida.Services;
using LaFlorida.PageModels;
using Microsoft.AspNetCore.Authorization;

namespace LaFlorida.Pages.Sales
{
    [Authorize(Roles = "Admin, Manager")]
    public class CreateModel : SalesPageModel
    {
        public CreateModel(ISaleService saleService, ICycleService cycleService)
            :base(saleService, cycleService)
        {
        }

        [BindProperty]
        public Sale Sale { get; set; }
        [BindProperty]
        public int DashboardId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? previous = null)
        {
            await SetSelectLists();
            DashboardId = previous ?? 0;

            if (previous == null)
            {
                return Page();
            }

            Sale = await HasCycle(previous);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? previous = null)
        {
            if (!ModelState.IsValid)
            {
                await SetSelectLists();
                await HasCycle(previous);
                return Page();
            }

            var create = await _saleService.CreateSaleAsync(Sale);
            if (!create.Success)
            {
                ModelState.AddModelError("error", create.Message);
                await SetSelectLists();
                await HasCycle(previous);
                return Page();
            }

            if (DashboardId != 0)
                return RedirectToPage("../Dashboard", new { id = DashboardId, success = true, message = "Venta creada con exito" });

            return RedirectToPage("./Index", new { success = true, message = "Venta creada con exito" });
        }
    }
}
