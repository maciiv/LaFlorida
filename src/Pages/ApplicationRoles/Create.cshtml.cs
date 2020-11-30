using System.Threading.Tasks;
using LaFlorida.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaFlorida.Pages.ApplicationRoles
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly IApplicationRoleService _applicationRoleService;

        public CreateModel(IApplicationRoleService applicationRoleService)
        {
            _applicationRoleService = applicationRoleService;
        }

        [BindProperty]
        public string RoleName { get; set; }

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

            var create = await _applicationRoleService.CreateApplicationRoleAsync(RoleName);
            if (!create.Succeeded)
            {
                ModelState.AddModelError("error", create.Errors.ToString());
                return Page();
            }

            return RedirectToPage("./Index", new { success = true, message = "Role creado con exito" });
        }
    }
}