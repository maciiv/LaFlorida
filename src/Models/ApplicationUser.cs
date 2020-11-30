using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LaFlorida.Models
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }
        [PersonalData]
        [Display(Name = "Apellido")]
        public string LastName { get; set; }

        public virtual ICollection<Cost> Costs { get; set; }
        public virtual ICollection<Withdraw> Withdraws { get; set; }
    }
}
