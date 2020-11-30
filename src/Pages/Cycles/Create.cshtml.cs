using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LaFlorida.Models;
using LaFlorida.Services;
using Microsoft.AspNetCore.Authorization;

namespace LaFlorida.Pages.Cycles
{
    [Authorize(Roles = "Admin, Manager")]
    public class CreateModel : PageModel
    {
        private readonly ICycleService _cycleService;
        private readonly ICropService _cropService;
        private readonly ILotService _lotService;

        public CreateModel(ICycleService cycleService, ICropService cropService, ILotService lotService)
        {
            _cycleService = cycleService;
            _cropService = cropService;
            _lotService = lotService;
        }

        [BindProperty]
        public Cycle Cycle { get; set; }

        public async Task<IActionResult> OnGet(bool? previous = null)
        {
            await SetSelectLists();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(bool? previous)
        {
            if (!ModelState.IsValid)
            {
                await SetSelectLists();
                return Page();
            }

            var create = await _cycleService.CreateCycleAsync(Cycle);
            if (!create.Success)
            {
                ModelState.AddModelError("error", create.Message);
                await SetSelectLists();
                return Page();
            }

            if (previous ?? false)
            {
                return RedirectToPage("/Dashboard", new { id = create.Model.CycleId, success = true, message = "Ciclo creado con exito" });
            }

            return RedirectToPage("./Index", new { success = true, message = "Ciclo creado con exito" });
        }

        private async Task SetSelectLists()
        {
            ViewData["CropId"] = await _cropService.GetCropsSelectListAsync();
            ViewData["LotId"] = await _lotService.GetLotsSelectListAsync();
        }
    }
}
