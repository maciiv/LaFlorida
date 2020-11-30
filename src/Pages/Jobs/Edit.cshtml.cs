using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LaFlorida.Models;
using LaFlorida.Services;
using Microsoft.AspNetCore.Authorization;

namespace LaFlorida.Pages.Jobs
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly IJobService _jobService;

        public EditModel(IJobService jobService)
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var edit = await _jobService.EditJobAsync(Job);
            if (!edit.Success)
            {
                ModelState.AddModelError("error", edit.Message);
                return Page();
            }

            return RedirectToPage("./Index", new { success = true, message = "Trabajo editado con exito" });
        }
    }
}
