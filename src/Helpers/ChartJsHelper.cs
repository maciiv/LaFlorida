using System;
using System.Drawing;

namespace LaFlorida.Helpers
{
    public class ChartJsHelper
    {
        public (string background, string border) GetBackgroundRandomColor()
        {
            var random = new Random();
            var color = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
            var background = $"rgba({color.R}, {color.G}, {color.B}, 0.2)";
            var border = $"rgba({color.R}, {color.G}, {color.B}, 1)";
            return (background, border);
        }
    }
}
