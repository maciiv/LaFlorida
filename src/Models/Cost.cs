using System;
using System.ComponentModel.DataAnnotations;

namespace LaFlorida.Models
{
    public class Cost
    {
        public int CostId { get; set; }
        [Display(Name = "Cantidad")]
        public decimal? Quantity { get; set; }
        [Display(Name = "Precio Unitario")]
        public decimal? Price { get; set; }
        [Display(Name = "Total")]
        public decimal? Total { get; set; }
        [Display(Name = "Detalles")]
        public string Details { get; set; }
        [Display(Name = "Fecha de Creacion")]
        public DateTime CreateDate { get; set; }
        [Display(Name = "Accionista")]
        public string ApplicationUserId { get; set; }
        [Display(Name = "Ciclo")]
        public int CycleId { get; set; }
        [Display(Name = "Trabajo")]
        public int JobId { get; set; }

        [Display(Name = "Accionista")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        [Display(Name = "Ciclo")]
        public virtual Cycle Cycle { get; set; }
        [Display(Name = "Trabajo")]
        public virtual Job Job { get; set; }
    }
}
