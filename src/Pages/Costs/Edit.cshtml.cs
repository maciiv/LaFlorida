using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LaFlorida.Models;
using LaFlorida.Services;
using LaFlorida.PageModels;
using Microsoft.AspNetCore.Authorization;

namespace LaFlorida.Pages.Costs
{
    [Authorize(Roles = "Admin, Manager")]
    public class EditModel : CostsPageModel
    {
        public EditModel(ICostService costService, ICycleService cycleService, IJobService jobService, IApplicationUserService applicationUserService)
            : base(costService, cycleService, jobService, applicationUserService)
        {
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

            var edit = await _costService.EditCostAsync(Cost);
            if (!edit.Success)
            {
                ModelState.AddModelError("error", edit.Message);
                await SetSelectLists();
                await HasCycle(previous);
                return Page();
            }

            if (DashboardId != 0)
                return RedirectToPage("../Dashboard", new { id = DashboardId, success = true, message = "Costo editado con exito" });

            return RedirectToPage("./Index", new { success = true, message = "Costo editado con exito" });
        }
    }
}
