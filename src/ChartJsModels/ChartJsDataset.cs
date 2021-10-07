using System.Collections.Generic;

namespace LaFlorida.ChartJsModels
{
    public class ChartJsDataset
    {
        public string label;
        public List<decimal?> data = new List<decimal?>();
        public string backgroundColor = "rgba(153, 102, 255, 0.2)";
        public string borderColor = "rgba(153, 102, 255, 1)";
        public int borderWidth = 1;
    }   
}
