﻿namespace LaFlorida.ServicesModels
{
    public class CycleCostByUser
    {
        public string ApplicationUserId { get; set; }
        public string UserName { get; set; }
        public string LotName { get; set; }
        public string CycleName { get; set; }
        public decimal? Costs { get; set; }
        public decimal? Percentage { get; set; }
        public decimal? Sales { get; set; }
        public decimal? Withdraws { get; set; }
        public decimal? Balance { get; set; }
    }
}
