using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly IReportService _reportService;

        public ManageModel(IApplicationUserService applicationUserService, ICycleService cycleService, IReportService reportService)
        {
            _applicationUserService = applicationUserService;
            _cycleService = cycleService;
            _reportService = reportService;
        }

        [BindProperty]
        public ApplicationUserBase ApplicationUser { get; set; }
        public IList<CycleCostByUser> ActiveCycles { get; set; } = new List<CycleCostByUser>();
        public IList<CycleCostByUser> ClosedCycles { get; set; } = new List<CycleCostByUser>();

        public async Task OnGetAsync()
        {
            ApplicationUser = await _applicationUserService.GetRegisterApplicationUserByNameAsync(User.Identity.Name);
            var cycles = await _cycleService.GetCyclesByUserAsync(ApplicationUser.Id);
            var activeCycles = cycles.Where(c => !c.IsComplete).OrderBy(c => c.CreateDate).ToList();
            foreach (var ac in activeCycles)
            {
                var cycleCostByUser = await _reportService.GetCycleCostByUsersAsync(ac.CycleId);
                ActiveCycles.Add(cycleCostByUser.FirstOrDefault(c => c.ApplicationUserId == ApplicationUser.Id));
            }
            var closedCycles = cycles.Where(c => c.IsComplete).OrderBy(c => c.CreateDate).TakeLast(9).ToList();
            foreach (var ac in closedCycles)
            {
                var cycleCostByUser = await _reportService.GetCycleCostByUsersAsync(ac.CycleId);
                ClosedCycles.Add(cycleCostByUser.FirstOrDefault(c => c.ApplicationUserId == ApplicationUser.Id));
            }
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