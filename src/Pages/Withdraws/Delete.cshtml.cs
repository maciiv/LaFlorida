using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LaFlorida.Models;
using LaFlorida.Services;
using Microsoft.AspNetCore.Authorization;

namespace LaFlorida.Pages.Withdraws
{
    [Authorize(Roles = "Admin, Manager")]
    public class DeleteModel : PageModel
    {
        private readonly IWithdrawService _withdrawService;

        public DeleteModel(IWithdrawService withdrawService)
        {
            _withdrawService = withdrawService;
        }

        [BindProperty]
        public Withdraw Withdraw { get; set; }
        [BindProperty]
        public int DashboardId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, int? previous = null)
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

            DashboardId = previous ?? 0;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index", new { error = true, message = "Retiro no encontrado" });
            }

            var delete = await _withdrawService.DeleteWithdrawAsync((int)id);
            if (!delete.Success)
            {
                ModelState.AddModelError("error", delete.Message);
                if (User.IsInRole("Admin"))
                {
                    ModelState.AddModelError("error", delete.Exception);
                }
                Withdraw = await _withdrawService.GetWithdrawByIdAsync((int)id);
                return Page();
            }

            if (DashboardId != 0)
                return RedirectToPage("../Dashboard", new { id = DashboardId, success = true, message = "Retiro borrado con exito" });

            return RedirectToPage("./Index", new { success = true, message = "Retiro borrado con exito" });
        }
    }
}
