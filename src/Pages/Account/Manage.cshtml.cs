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
        private readonly IReportService _reportService;

        public ManageModel(IApplicationUserService applicationUserService, IReportService reportService)
        {
            _applicationUserService = applicationUserService;
            _reportService = reportService;
        }

        [BindProperty]
        public ApplicationUserBase ApplicationUser { get; set; }
        public IList<CycleCostByUser> ActiveCycles { get; set; } = new List<CycleCostByUser>();
        public IList<CycleCostByUser> ClosedCycles { get; set; } = new List<CycleCostByUser>();
        public IList<CycleCostByUser> ClosedCyclesPerformance { get; set; } = new List<CycleCostByUser>();
        public bool IsIdendity { get; set; }

        public async Task OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                ApplicationUser = await _applicationUserService.GetRegisterApplicationUserByNameAsync(User.Identity.Name);
                IsIdendity = true;
            }
            else
            {
                ApplicationUser = await _applicationUserService.GetRegisterApplicationUserByIdAsync(id);
            }

            var userCyclesCosts = await _reportService.GetUserCyclesCostsAsync(ApplicationUser.Id);
            ActiveCycles = userCyclesCosts.Where(c => !c.IsCycleComplete).OrderBy(c => c.CreateDate).ToList();
            ClosedCycles = userCyclesCosts.Where(c => c.IsCycleComplete).OrderBy(c => c.CreateDate).ToList();
            ClosedCyclesPerformance = ClosedCycles.Where(c => !c.IsCycleRent).ToList();

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