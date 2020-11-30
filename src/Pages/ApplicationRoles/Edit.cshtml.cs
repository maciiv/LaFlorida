using System.Threading.Tasks;
using LaFlorida.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaFlorida.Pages.ApplicationRoles
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly IApplicationRoleService _applicationRoleService;

        public EditModel(IApplicationRoleService applicationRoleService)
        {
            _applicationRoleService = applicationRoleService;
        }

        [BindProperty]
        public IdentityRole ApplicationRole { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index", new { error = true, message = "Rol no encontrado" });
            }

            ApplicationRole = await _applicationRoleService.GetApplicationRoleByIdAsync(id);

            if (ApplicationRole == null)
            {
                return RedirectToPage("./Index", new { error = true, message = "Rol no encontrado" });
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var edit = await _applicationRoleService.EditApplicationRoleAsync(ApplicationRole.Id, ApplicationRole.Name);
            if (!edit.Succeeded)
            {
                ModelState.AddModelError("error", edit.Errors.ToString());
                return Page();
            }

            return RedirectToPage("./Index", new { success = true, message = "Rol editado con exito" });
        }
    }
}