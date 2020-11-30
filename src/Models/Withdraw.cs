using System;
using System.ComponentModel.DataAnnotations;

namespace LaFlorida.Models
{
    public class Withdraw
    {
        public int WithdrawId { get; set; }
        [Display(Name = "Cantidad")]
        public decimal? Quantity { get; set; }
        [Display(Name = "Fecha de Creacion")]
        public DateTime CreateDate { get; set; }
        [Display(Name = "Ciclo")]
        public int CycleId { get; set; }
        [Display(Name = "Accionista")]
        public string ApplicationUserId { get; set; }

        [Display(Name = "Ciclo")]
        public virtual Cycle Cycle { get; set; }
        [Display(Name = "Accionista")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
