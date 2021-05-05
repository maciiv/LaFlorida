using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaFlorida.Extensions
{
    public static class ImportParseExtensions
    {
        public static decimal? GetDecimal(this string data)
        {
            return string.IsNullOrEmpty(data) ? (decimal?)null : decimal.Parse(data);
        }

        public static int GetInt(this string data)
        {
            return string.IsNullOrEmpty(data) ? 0 : int.Parse(data);
        }
    }
}
