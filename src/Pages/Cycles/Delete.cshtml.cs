using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LaFlorida.Models;
using LaFlorida.Services;
using Microsoft.AspNetCore.Authorization;

namespace LaFlorida.Pages.Cycles
{
    [Authorize(Roles = "Admin, Manager")]
    public class DeleteModel : PageModel
    {
        private readonly ICycleService _cycleService;

        public DeleteModel(ICycleService cycleService)
        {
            _cycleService = cycleService;
        }

        [BindProperty]
        public Cycle Cycle { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index", new { error = true, message = "Ciclo no encontrado" });
            }

            Cycle = await _cycleService.GetCycleByIdAsync((int)id);

            if (Cycle == null)
            {
                return RedirectToPage("./Index", new { error = true, message = "Ciclo no encontrado" });
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index", new { error = true, message = "Ciclo no encontrado" });
            }

            var delete = await _cycleService.DeleteCycleAsync((int)id);
            if (!delete.Success)
            {
                ModelState.AddModelError("error", delete.Message);
                if (User.IsInRole("Admin"))
                {
                    ModelState.AddModelError("error", delete.Exception);
                }
                Cycle = await _cycleService.GetCycleByIdAsync((int)id);
                return Page();
            }

            return RedirectToPage("./Index", new { success = true, message = "Ciclo borrado con exito" });
        }
    }
}
