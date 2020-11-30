using System.Threading.Tasks;
using LaFlorida.Services;
using LaFlorida.ServicesModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaFlorida.Pages.ApplicationUsers
{
    public class ResetPasswordModel : PageModel
    {
        private readonly IApplicationUserService _applicationUserService;

        public ResetPasswordModel(IApplicationUserService applicationUserService)
        {
            _applicationUserService = applicationUserService;
        }

        public ApplicationUserBase ApplicationUser { get; set; }
        [BindProperty]
        public ApplicationUserResetPassword ResetPassword { get; set; }
        

        public async Task<IActionResult> OnGetAsync(string id)
        {
            ApplicationUser = await _applicationUserService.GetRegisterApplicationUserByIdAsync(id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var edit = await _applicationUserService.ResetPasswordAsync(ResetPassword);
            if (!edit.Succeeded)
            {
                ModelState.AddModelError("error", edit.Errors.ToString());
                return Page();
            }

            return RedirectToPage("./Index", new { success = true, message = "Accionista editado con exito" });
        }
    }
}