using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using LaFlorida.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaFlorida.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [BindProperty]
        [EmailAddress(ErrorMessage = "Email/Usuario es invalido")]
        [Required]
        public string Email { get; set; }
        [BindProperty]
        [DataType(DataType.Password)]
        [Required]
        [Display(Name = "Contrasena")]
        public string Password { get; set; }

        public void OnGet(string returnUrl)
        {

        }

        public async Task<IActionResult> OnPostAsync(string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Email);

            if (user == null)
            {
                ModelState.AddModelError("message", "El usuario o contrasena son invalidos");
                return Page();
            }

            if (user != null && !user.EmailConfirmed)
            {
                ModelState.AddModelError("message", "El usuario no ha sido concedido accesso");
                return Page();
            }

            var signIn = await _signInManager.PasswordSignInAsync(user.UserName, Password, false, false);
            if (!signIn.Succeeded)
            {
                ModelState.AddModelError("message", "El usuario o contrasena son invalidos");
                return Page();
            }

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

            if (await _userManager.IsInRoleAsync(user, "Client"))
            {
                return RedirectToPage("/Account/Manage");
            }

            return RedirectToPage("/CurrentCycles");
        }
    }
}