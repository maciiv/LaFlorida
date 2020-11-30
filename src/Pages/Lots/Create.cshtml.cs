using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LaFlorida.Models;
using LaFlorida.Services;
using Microsoft.AspNetCore.Authorization;

namespace LaFlorida.Pages.Lots
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly ILotService _lotService;

        public CreateModel(ILotService lotService)
        {
            _lotService = lotService;
        }

        [BindProperty]
        public Lot Lot { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }
     
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var create = await _lotService.CreateLotAsync(Lot);
            if (!create.Success)
            {
                ModelState.AddModelError("error", create.Message);
                return Page();
            }

            return RedirectToPage("./Index", new { success = true, message = "Lote creado con exito" });
        }
    }
}
