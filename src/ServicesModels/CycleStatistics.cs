using System;

namespace LaFlorida.ServicesModels
{
    public class CycleStatistics
    {
        public string CycleName { get; set; }
        public string LotName { get; set; }
        public int LotSize { get; set; }
        public string CropName { get; set; }
        public int CropLenght { get; set; }
        public DateTime CreateDate { get; set; }
        public decimal? TotalCosts { get; set; }
        public decimal? TotalSales { get; set; }
        public decimal? Performace { get; set; }
        public decimal? Return { get; set; }
        public decimal? Profit { get; set; }
        public decimal? ProfitByLenght { get; set; }
        public decimal? ProfitBySize { get; set; }
    }
}
