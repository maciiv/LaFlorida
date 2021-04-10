using System;

namespace LaFlorida.ServicesModels
{
    public class CycleCostByUser
    {
        public string ApplicationUserId { get; set; }
        public string UserName { get; set; }
        public string LotName { get; set; }
        public string CycleName { get; set; }
        public string CropName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? HarvestDate { get; set; }
        public decimal? Costs { get; set; }
        public decimal? Percentage { get; set; }
        public decimal? Sales { get; set; }
        public decimal? Withdraws { get; set; }
        public decimal? Balance { get; set; }
        public decimal? Profit { get; set; }
        public decimal? Return { get; set; }
    }
}
