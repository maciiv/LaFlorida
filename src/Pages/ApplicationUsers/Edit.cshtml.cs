using System.Threading.Tasks;
using LaFlorida.Services;
using LaFlorida.ServicesModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace LaFlorida.Pages.ApplicationUsers
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly IApplicationUserService _applicationUserService;
        private readonly IApplicationRoleService _applicationRoleService;

        public EditModel(IApplicationUserService applicationUserService, IApplicationRoleService applicationRoleService)
        {
            _applicationUserService = applicationUserService;
            _applicationRoleService = applicationRoleService;
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

            var edit = await _applicationUserService.EditApplicationUserAsync(ApplicationUser);
            if (!edit.Succeeded)
            {
                foreach (var error in edit.Errors)
                {
                    ModelState.AddModelError("error", error.Description);
                }
                await SetSelectLists();
                return Page();
            }

            return RedirectToPage("./Index", new { success = true, message = "Accionista editado con exito" });
        }

        public async Task<PartialViewResult> OnGetRemoveRoleAsync(string id, string role)
        {
            var result = await _applicationUserService.RemoveRoleAsync(id, role);
            ApplicationUser = await _applicationUserService.GetRegisterApplicationUserByIdAsync(id);
            return new PartialViewResult
            {
                ViewName = "_EditPartial",
                ViewData = new ViewDataDictionary<ApplicationUserEdit>(ViewData, ApplicationUser)
            };
        }

        private async Task SetSelectLists()
        {
            ViewData["RoleId"] = new SelectList(await _applicationRoleService.GetApplicationRolesAsync(), "Name", "Name");
        }
    }
}