using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LaFlorida.Models
{
    public class Lot
    {
        [Display(Name = "Lote")]
        public int LotId { get; set; }
        [Display(Name = "Nombre")]
        public string Name { get; set; }
        [Display(Name = "Hectareas")]
        public int Size { get; set; }

        public virtual ICollection<Cycle> Cycles { get; set; }
    }
}
