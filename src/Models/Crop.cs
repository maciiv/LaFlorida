using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LaFlorida.Models
{
    public class Crop
    {
        [Display(Name = "Cultivo")]
        public int CropId { get; set; }
        [Display(Name = "Nombre")]
        public string Name { get; set; }
        [Display(Name = "Duracion (meses)")]
        public int Lenght { get; set; }

        public virtual ICollection<Cycle> Cycles { get; set; }
    }
}
