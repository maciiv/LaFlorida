using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LaFlorida.Models;
using LaFlorida.Services;
using LaFlorida.PageModels;
using Microsoft.AspNetCore.Authorization;

namespace LaFlorida.Pages.Payments
{
    [Authorize(Roles = "Admin, Manager")]
    public class CreateModel : PaymentsPageModel
    {
        public CreateModel(IPaymentService paymentService, IApplicationUserService applicationUserService, ICycleService cycleService)
            :base(paymentService, applicationUserService, cycleService)
        {
        }

        [BindProperty]
        public Payment Payment { get; set; }
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

            Payment = await HasCycle(previous, applicationUserId);
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

            var create = await _paymentService.CreatePaymentAsync(Payment);
            if (!create.Success)
            {
                ModelState.AddModelError("error", create.Message);
                await SetSelectLists();
                await HasCycle(previous, applicationUserId);
                return Page();
            }

            if (DashboardId != 0)
                return RedirectToPage("../Dashboard", new { id = DashboardId, success = true, message = "Pago creado con exito" });

            return RedirectToPage("./Index", new { success = true, message = "Pago creado con exito" });
        }
    }
}
