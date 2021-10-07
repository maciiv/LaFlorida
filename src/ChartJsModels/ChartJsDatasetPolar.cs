using System.Collections.Generic;

namespace LaFlorida.ChartJsModels
{
    public class ChartJsDatasetPolar
    {
        public string label;
        public List<decimal?> data = new List<decimal?>();
        public List<string> backgroundColor = new List<string>();
        public List<string> borderColor = new List<string>();
        public int borderWidth = 1;
    }
}
