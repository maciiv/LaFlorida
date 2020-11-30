using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaFlorida.Extensions
{
    public static class ChartExtensions
    {
        public static string GetLabels(this List<string> labels)
        {
            var result = JsonConvert.SerializeObject(labels);
            return result;
        }

        public static string GetData(this List<decimal?> data)
        {
            var result = JsonConvert.SerializeObject(data);
            return result;
        }
    }
}
