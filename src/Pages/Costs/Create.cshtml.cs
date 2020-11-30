using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LaFlorida.Models;
using LaFlorida.Services;
using LaFlorida.PageModels;
using Microsoft.AspNetCore.Authorization;

namespace LaFlorida.Pages.Costs
{
    [Authorize(Roles = "Admin, Manager")]
    public class CreateModel : CostsPageModel
    {
        public CreateModel(ICostService costService, ICycleService cycleService, IJobService jobService, IApplicationUserService applicationUserService)
            : base(costService, cycleService, jobService, applicationUserService)
        {
        }

        [BindProperty]
        public Cost Cost { get; set; }
        [BindProperty]
        public int DashboardId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? previous = null)
        {
            DashboardId = previous ?? 0;
            await SetSelectLists();

            if (previous == null)
            {                
                return Page();
            }

            Cost = await HasCycle(previous);

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

            var create = await _costService.CreateCostAsync(Cost);
            if (!create.Success)
            {
                ModelState.AddModelError("error", create.Message);
                await SetSelectLists();
                await HasCycle(previous);
                return Page();
            }

            if (DashboardId != 0)
                return RedirectToPage("../Dashboard", new { id = DashboardId, success = true, message = "Costo creado con exito" });

            return RedirectToPage("./Index", new { success = true, message = "Costo creado con exito" });
        }
    }
}
