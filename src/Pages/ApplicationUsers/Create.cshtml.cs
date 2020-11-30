using System.Threading.Tasks;
using LaFlorida.Services;
using LaFlorida.ServicesModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LaFlorida.Pages.ApplicationUsers
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly IApplicationUserService _applicationUserService;
        private readonly IApplicationRoleService _applicationRoleService;

        public CreateModel(IApplicationUserService applicationUserService, IApplicationRoleService applicationRoleService)
        {
            _applicationUserService = applicationUserService;
            _applicationRoleService = applicationRoleService;
        }

        [BindProperty]
        public ApplicationUserCreate RegisterApplicationUser { get; set; }

        public async Task<IActionResult> OnGet()
        {
            await SetSelectLists();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await SetSelectLists();
                return Page();
            }

            var create = await _applicationUserService.CreateApplicationUserAsync(RegisterApplicationUser);
            if (!create.Succeeded)
            {
                ModelState.AddModelError("error", create.Errors.ToString());
                await SetSelectLists();
                return Page();
            }

            return RedirectToPage("./Index", new { success = true, message = "Accionista creado con exito" });
        }

        private async Task SetSelectLists()
        {
            ViewData["RoleId"] = new SelectList(await _applicationRoleService.GetApplicationRolesAsync(), "Name", "Name");
        }
    }
}