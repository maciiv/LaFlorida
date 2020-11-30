using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LaFlorida.Models;
using LaFlorida.Services;
using Microsoft.AspNetCore.Authorization;

namespace LaFlorida.Pages.Jobs
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly IJobService _jobService;

        public DeleteModel(IJobService jobService)
        {
            _jobService = jobService;
        }

        [BindProperty]
        public Job Job { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index", new { error = true, message = "Trabajo no encontrado" });
            }

            Job = await _jobService.GetJobByIdAsync((int)id);

            if (Job == null)
            {
                return RedirectToPage("./Index", new { error = true, message = "Trabajo no encontrado" });
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index", new { error = true, message = "Trabajo no encontrado" });
            }

            var delete = await _jobService.DeleteJobAsync((int)id);
            if (!delete.Success)
            {
                ModelState.AddModelError("error", delete.Message);
                if (User.IsInRole("Admin"))
                {
                    ModelState.AddModelError("error", delete.Exception);
                }
                Job = await _jobService.GetJobByIdAsync((int)id);
                return Page();
            }

            return RedirectToPage("./Index", new { success = true, message = "Trabajo borrado con exito" });
        }
    }
}
