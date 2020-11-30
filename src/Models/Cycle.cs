using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LaFlorida.Models
{
    public class Cycle
    {
        public int CycleId { get; set; }
        [Display(Name = "Nombre")]
        public string Name { get; set; }
        [Display(Name = "Fecha de Creacion")]
        public DateTime CreateDate { get; set; }
        [Display(Name = "Fecha de Cosecha")]
        public DateTime? HarvestDate { get; set; }
        [Display(Name = "Cultivo")]
        public int CropId { get; set; }
        [Display(Name = "Lote")]
        public int LotId { get; set; }
        [Display(Name = "Liquidado")]
        public bool IsComplete { get; set; }
        [Display(Name = "Renta")]
        public bool IsRent { get; set; }

        [Display(Name = "Cultivo")]
        public virtual Crop Crop { get; set; }
        [Display(Name = "Lote")]
        public virtual Lot Lot { get; set; }

        public virtual ICollection<Sale> Sales { get; set; }
        public virtual ICollection<Cost> Costs { get; set; }
        public virtual ICollection<Withdraw> Withdraws { get; set; }
    }
}
