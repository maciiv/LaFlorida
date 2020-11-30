using System.ComponentModel.DataAnnotations;

namespace LaFlorida.ServicesModels
{
    public class ApplicationUserCreate : ApplicationUserBase
    {
        [Display(Name = "Contrasena")]
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contrasena")]
        [Compare("Password", ErrorMessage = "La contrasena y confirmar contrasena no coinciden")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email/Usuario es invalido")]
        [Required]
        public string Email { get; set; }
        [Display(Name = "Rol")]
        public string RoleId { get; set; }
    }
}
