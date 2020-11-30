using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LaFlorida.Models
{
    public class Job
    {
        public int JobId { get; set; }
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        public virtual ICollection<Cost> Costs { get; set; }
    }
}
