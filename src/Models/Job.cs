using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LaFlorida.Models
{
    public class Job
    {
        public int JobId { get; set; }
        [Display(Name = "Nombre")]
        public string Name { get; set; }
        [Display(Name = "Renta")]
        public bool IsRent { get; set; }
        [Display(Name = "Maquinaria")]
        public bool IsMachinist { get; set; }

        public virtual ICollection<Cost> Costs { get; set; }
    }
}
