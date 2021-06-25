using System;
using System.ComponentModel.DataAnnotations;

namespace LaFlorida.Models
{
    public class Sale
    {
        public int SaleId { get; set; }
        [Display(Name = "Cantidad")]
        public decimal? Quantity { get; set; }
        [Display(Name = "Precio")]
        public decimal? Price { get; set; }
        [Display(Name = "Total")]
        public decimal? Total { get; set; }
        [Display(Name = "Comprador")]
        public string Buyer { get; set; }
        [Display(Name = "Detalles")]
        public string Details { get; set; }
        [Display(Name = "Numero de Factura")]
        public string InvoiceId { get; set; }
        [Display(Name = "Fecha de Creacion")]
        public DateTime CreateDate { get; set; }
        [Display(Name = "Quintales")]
        public decimal? Quintals { get; set; }
        [Display(Name = "Ciclo")]
        public int CycleId { get; set; }

        [Display(Name = "Ciclo")]
        public virtual Cycle Cycle { get; set; }
    }
}
