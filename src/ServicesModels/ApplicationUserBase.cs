using System.ComponentModel.DataAnnotations;

namespace LaFlorida.ServicesModels
{
    public class ApplicationUserBase
    {
        public string Id { get; set; }
        [Required]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Apellido")]
        public string LastName { get; set; }
    }
}
