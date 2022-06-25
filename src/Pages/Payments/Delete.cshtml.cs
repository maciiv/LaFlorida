using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LaFlorida.Models;
using LaFlorida.Services;
using Microsoft.AspNetCore.Authorization;

namespace LaFlorida.Pages.Payments
{
    [Authorize(Roles = "Admin, Manager")]
    public class DeleteModel : PageModel
    {
        private readonly IPaymentService _paymentService;

        public DeleteModel(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [BindProperty]
        public Payment Payment { get; set; }
        [BindProperty]
        public int DashboardId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, int? previous = null)
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

            DashboardId = previous ?? 0;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index", new { error = true, message = "Retiro no encontrado" });
            }

            var delete = await _paymentService.DeletePaymentAsync((int)id);
            if (!delete.Success)
            {
                ModelState.AddModelError("error", delete.Message);
                if (User.IsInRole("Admin"))
                {
                    ModelState.AddModelError("error", delete.Exception);
                }
                Payment = await _paymentService.GetPaymentByIdAsync((int)id);
                return Page();
            }

            if (DashboardId != 0)
                return RedirectToPage("../Dashboard", new { id = DashboardId, success = true, message = "Pago borrado con exito" });

            return RedirectToPage("./Index", new { success = true, message = "Pago borrado con exito" });
        }
    }
}
