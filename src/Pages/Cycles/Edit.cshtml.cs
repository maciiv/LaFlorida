using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LaFlorida.Models;
using LaFlorida.Services;
using Microsoft.AspNetCore.Authorization;

namespace LaFlorida.Pages.Cycles
{
    [Authorize(Roles = "Admin, Manager")]
    public class EditModel : PageModel
    {
        private readonly ICycleService _cycleService;
        private readonly ICropService _cropService;
        private readonly ILotService _lotService;

        public EditModel(ICycleService cycleService, ICropService cropService, ILotService lotService)
        {
            _cycleService = cycleService;
            _cropService = cropService;
            _lotService = lotService;
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

            await SetSelectLists();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await SetSelectLists();
                return Page();
            }

            var edit = await _cycleService.EditCycleAsync(Cycle);
            if (!edit.Success)
            {
                ModelState.AddModelError("error", edit.Message);
                await SetSelectLists();
                return Page();
            }

            return RedirectToPage("./Index", new { success = true, message = "Ciclo editado con exito" });
        }

        private async Task SetSelectLists()
        {
            ViewData["CropId"] = await _cropService.GetCropsSelectListAsync();
            ViewData["LotId"] = await _lotService.GetLotsSelectListAsync();
        }
    }
}
