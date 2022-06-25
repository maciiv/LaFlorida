using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LaFlorida.Models;
using LaFlorida.Services;
using LaFlorida.PageModels;
using Microsoft.AspNetCore.Authorization;

namespace LaFlorida.Pages.Payments
{
    [Authorize(Roles = "Admin, Manager")]
    public class EditModel : PaymentsPageModel
    {
        public EditModel(IPaymentService paymentService, IApplicationUserService applicationUserService, ICycleService cycleService)
            :base(paymentService, applicationUserService, cycleService)
        {
        }

        [BindProperty]
        public Payment Payment { get; set; }
        [BindProperty]
        public int DashboardId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, int? previous = null, string applicationUserId = null)
        {
            if (id == null)
            {
                return RedirectToPage("./Index", new { error = true, message = "Pago no encontrado" });
            }

            Payment = await _paymentService.GetPaymentByIdAsync((int)id);

            if (Payment == null)
            {
                return RedirectToPage("./Index", new { error = true, message = "Pago no encontrado" });
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

            var edit = await _paymentService.EditPaymentAsync(Payment);
            if (!edit.Success)
            {
                ModelState.AddModelError("error", edit.Message);
                await SetSelectLists();
                await HasCycle(previous, applicationUserId);
                return Page();
            }

            if (DashboardId != 0)
                return RedirectToPage("../Dashboard", new { id = DashboardId, success = true, message = "Pago editado con exito" });

            return RedirectToPage("./Index", new { success = true, message = "Pago editado con exito" });
        }
    }
}
