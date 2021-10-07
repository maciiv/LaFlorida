using System.Collections.Generic;

namespace LaFlorida.ChartJsModels
{
    public class ChartJsData<T>
    {
        public List<string> labels = new List<string>();
        public List<T> datasets = new List<T>();

    }
}
