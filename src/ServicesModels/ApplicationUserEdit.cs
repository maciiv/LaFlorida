using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LaFlorida.ServicesModels
{
    public class ApplicationUserEdit : ApplicationUserBase
    {
        [Display(Name = "Rol")]
        public string RoleId { get; set; }
        [Display(Name = "Todos los Roles")]
        public List<string> Roles { get; set; }
    }
}
