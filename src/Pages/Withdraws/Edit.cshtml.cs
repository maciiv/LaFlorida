using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LaFlorida.Models;
using LaFlorida.Services;
using LaFlorida.PageModels;
using Microsoft.AspNetCore.Authorization;

namespace LaFlorida.Pages.Withdraws
{
    [Authorize(Roles = "Admin, Manager")]
    public class EditModel : WithdrawsPageModel
    {
        public EditModel(IWithdrawService withdrawService, IApplicationUserService applicationUserService, ICycleService cycleService)
            :base(withdrawService, applicationUserService, cycleService)
        {
        }

        [BindProperty]
        public Withdraw Withdraw { get; set; }
        [BindProperty]
        public int DashboardId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, int? previous = null, string applicationUserId = null)
        {
            if (id == null)
            {
                return RedirectToPage("./Index", new { error = true, message = "Retiro no encontrado" });
            }

            Withdraw = await _withdrawService.GetWithdrawByIdAsync((int)id);

            if (Withdraw == null)
            {
                return RedirectToPage("./Index", new { error = true, message = "Retiro no encontrado" });
            }

            await SetSelectLists();

            DashboardId = previous ?? 0;

            if (previous == null && string.IsNullOrEmpty(applicationUserId))
            {
                return Page();
            }

            await HasCycle(previous, applicationUserId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, int? previous = null, string applicationUserId = null)
        {
            if (!ModelState.IsValid)
            {
                await SetSelectLists();
                await HasCycle(previous, applicationUserId);
                return Page();
            }

            var edit = await _withdrawService.EditWithdrawAsync(Withdraw);
            if (!edit.Success)
            {
                ModelState.AddModelError("error", edit.Message);
                await SetSelectLists();
                await HasCycle(previous, applicationUserId);
                return Page();
            }

            if (DashboardId != 0)
                return RedirectToPage("../Dashboard", new { id = DashboardId, success = true, message = "Retiro editado con exito" });

            return RedirectToPage("./Index", new { success = true, message = "Retiro editado con exito" });
        }
    }
}
