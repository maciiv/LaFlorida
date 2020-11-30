using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LaFlorida.Models;
using LaFlorida.Services;
using Microsoft.AspNetCore.Authorization;

namespace LaFlorida.Pages.Jobs
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly IJobService _jobService;

        public CreateModel(IJobService jobService)
        {
            _jobService = jobService;
        }

        [BindProperty]
        public Job Job { get; set; }

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

            var create = await _jobService.CreateJobAsync(Job);
            if (!create.Success)
            {
                ModelState.AddModelError("error", create.Message);
                return Page();
            }

            return RedirectToPage("./Index", new { success = true, message = "Trabajo creado con exito" });
        }
    }
}
