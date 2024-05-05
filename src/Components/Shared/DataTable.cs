using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace LaFlorida.Shared
{
    public class DataTableModel<T> : ComponentBase
    {
        public IQueryable<T> Filter(IQueryable<T> source, string searchTerm)
        {
            var result = new List<T>();
            var props = typeof(T).GetProperties();

            foreach (var prop in props)
            {
                var search = source.Where(c => prop.GetValue(c) != null &&
                    prop.GetValue(c).ToString().ToLower().Trim().Contains(searchTerm));

                result.AddRange(search);
            }

            return result.Distinct().AsQueryable();
        }
    }
}