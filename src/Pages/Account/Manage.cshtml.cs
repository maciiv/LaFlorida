using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaFlorida.Models;
using LaFlorida.Services;
using LaFlorida.ServicesModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaFlorida.Pages.Account
{
    [Authorize]
    public class ManageModel : PageModel
    {
        private readonly IApplicationUserService _applicationUserService;
        private readonly ICycleService _cycleService;

        public ManageModel(IApplicationUserService applicationUserService, ICycleService cycleService)
        {
            _applicationUserService = applicationUserService;
            _cycleService = cycleService;
        }

        [BindProperty]
        public ApplicationUserBase ApplicationUser { get; set; }
        public IList<Cycle> ActiveCycles { get; set; }
        public IList<Cycle> CloseCycles { get; set; }

        public async Task OnGetAsync()
        {
            ApplicationUser = await _applicationUserService.GetRegisterApplicationUserByNameAsync(User.Identity.Name);
            var cycles = await _cycleService.GetCyclesByUserAsync(ApplicationUser.Id);
            ActiveCycles = cycles.Where(c => !c.IsComplete).OrderBy(c => c.CreateDate).ToList();
            CloseCycles = cycles.Where(c => c.IsComplete).OrderBy(c => c.CreateDate).TakeLast(9).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var edit = await _applicationUserService.EditApplicationUserBaseAsync(ApplicationUser);
            if (!edit.Succeeded)
            {
                ModelState.AddModelError("error", edit.Errors.ToString());
                return Page();
            }

            return RedirectToPage("/CurrentCycles", new { success = true, message = "Accionista editado con exito" });
        }
    }
}