using System.ComponentModel.DataAnnotations;

namespace LaFlorida.ServicesModels
{
    public class ApplicationUserResetPassword : ApplicationUserBase
    {
        [Display(Name = "Contrasena")]
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contrasena")]
        [Compare("Password", ErrorMessage = "La contrasena y confirmar contrasena no coinciden")]
        public string ConfirmPassword { get; set; }
    }
}
