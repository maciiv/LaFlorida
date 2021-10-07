using LaFlorida.ServicesModels;
using LaFlorida.ChartJsModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using LaFlorida.Helpers;

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

        public static string GetChartDataset(this List<ChartData> chartData, string labelTitle, string background = null, string border = null)
        {         
            if (string.IsNullOrEmpty(background))
            {
                var resultPolar = new ChartJsData<ChartJsDatasetPolar>
                {
                    labels = chartData.Select(c => c.Label).ToList()
                };
                var datasetPolar = new ChartJsDatasetPolar
                {
                    label = labelTitle,
                    data = chartData.Select(c => c.Data).ToList(),
                };

                var chartJsHelper = new ChartJsHelper();
                var backgroundColor = new List<string>();
                var borderColor = new List<string>();
                foreach (var l in resultPolar.labels)
                {
                    var (b, c) = chartJsHelper.GetBackgroundRandomColor();
                    backgroundColor.Add(b);
                    borderColor.Add(c);
                }
                datasetPolar.backgroundColor = backgroundColor;
                datasetPolar.borderColor = borderColor;
                resultPolar.datasets.Add(datasetPolar);
                return JsonConvert.SerializeObject(resultPolar);
            }

            var result = new ChartJsData<ChartJsDataset>
            {
                labels = chartData.Select(c => c.Label).ToList()
            };

            var dataset = new ChartJsDataset
            {
                label = labelTitle,
                data = chartData.Select(c => c.Data).ToList(),
                backgroundColor = background,
                borderColor = border
            };
            result.datasets.Add(dataset);

            return JsonConvert.SerializeObject(result);
        }

        public static string GetChartDatasets(this List<ChartData> chartData)
        {
            var chartJsHelper = new ChartJsHelper();
            var result = new ChartJsData<ChartJsDataset>();
            var allDataLabels = chartData.Select(c => c.DataLabel).Distinct();

            var group = chartData.GroupBy(c => c.Label);
             
            foreach (var grp in group)
            {
                result.labels.Add(grp.Key);
                foreach (var dl in allDataLabels)
                {
                    if (!result.datasets.Where(c => c.label == dl).Any())
                    {
                        var (background, border) = chartJsHelper.GetBackgroundRandomColor();
                        var dataset = new ChartJsDataset
                        {
                            label = dl,
                            backgroundColor = background,
                            borderColor = border
                        };
                        dataset.data.Add(grp.FirstOrDefault(d => d.DataLabel == dl)?.Data ?? 0);
                        result.datasets.Add(dataset);
                    }
                    else
                    {
                        result.datasets.FirstOrDefault(c => c.label == dl).data.Add(grp.FirstOrDefault(d => d.DataLabel == dl)?.Data ?? 0);
                    }
                }
            };          

            return JsonConvert.SerializeObject(result);
        }
    }
}
