using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LaFlorida.Models;
using LaFlorida.Services;
using LaFlorida.PageModels;
using Microsoft.AspNetCore.Authorization;

namespace LaFlorida.Pages.Withdraws
{
    [Authorize(Roles = "Admin, Manager")]
    public class CreateModel : WithdrawsPageModel
    {
        public CreateModel(IWithdrawService withdrawService, IApplicationUserService applicationUserService, ICycleService cycleService)
            :base(withdrawService, applicationUserService, cycleService)
        {
        }

        [BindProperty]
        public Withdraw Withdraw { get; set; }
        [BindProperty]
        public int DashboardId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? previous = null, string applicationUserId = null)
        {           
            DashboardId = previous ?? 0;
            await SetSelectLists();

            if (previous == null && string.IsNullOrEmpty(applicationUserId))
            {               
                return Page();
            }

            Withdraw = await HasCycle(previous, applicationUserId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? previous = null, string applicationUserId = null)
        {
            if (!ModelState.IsValid)
            {
                await SetSelectLists();
                await HasCycle(previous, applicationUserId);
                return Page();
            }

            var create = await _withdrawService.CreateWithdrawAsync(Withdraw);
            if (!create.Success)
            {
                ModelState.AddModelError("error", create.Message);
                await SetSelectLists();
                await HasCycle(previous, applicationUserId);
                return Page();
            }

            if (DashboardId != 0)
                return RedirectToPage("../Dashboard", new { id = DashboardId, success = true, message = "Retiro creado con exito" });

            return RedirectToPage("./Index", new { success = true, message = "Retiro creado con exito" });
        }
    }
}
