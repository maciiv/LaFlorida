using System;

namespace LaFlorida.ServicesModels
{
    public class CycleStatistics : SummaryStatistics
    {
        public int LotId { get; set; }
        public string LotName { get; set; }
        public int LotSize { get; set; }
        public int CropId { get; set; }
        public string CropName { get; set; }
        public int CropLenght { get; set; }
        public DateTime CreateDate { get; set; }
        public decimal? Performace { get; set; }
        public decimal? ProfitByLenght { get; set; }
        public decimal? ProfitBySize { get; set; }
    }
}
