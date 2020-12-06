using System.Threading.Tasks;
using LaFlorida.Services;
using LaFlorida.ServicesModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaFlorida.Pages.ApplicationUsers
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly IApplicationUserService _applicationUserService;

        public DeleteModel(IApplicationUserService applicationUserService)
        {
            _applicationUserService = applicationUserService;
        }

        [BindProperty]
        public ApplicationUserEdit ApplicationUser { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index", new { error = true, message = "Accionista no encontrado" });
            }

            ApplicationUser = await _applicationUserService.GetRegisterApplicationUserByIdAsync(id);

            if (ApplicationUser == null)
            {
                return RedirectToPage("./Index", new { error = true, message = "Accionista no encontrado" });
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToPage("./Index", new { error = true, message = "Accionista no encontrado" });
            }

            var delete = await _applicationUserService.DeleteApplicationUserAsync(id);
            if (!delete.Succeeded)
            {
                foreach (var error in delete.Errors)
                {
                    ModelState.AddModelError("error", error.Description);
                }
                return Page();
            }

            return RedirectToPage("./Index", new { success = true, message = "Accionista borrado con exito" });
        }
    }
}