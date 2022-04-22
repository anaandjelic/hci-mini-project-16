using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Windows.Controls;
using System.Windows.Media;

namespace MiniProject
{
    public partial class LineChart : UserControl
    {
        public SeriesCollection SeriesCollection { get; set; }
        public List<string> XLabels { get; set; }
        public Func<double, string> YLabels { get; set; }
        public Func<DateTime, string> XFormatter { get; set; }

        public LineChart()
        {
            YLabels = value => value.ToString(".##");
            SeriesCollection = new SeriesCollection();
            XLabels = new List<string>();
        }

        public void ClearData()
        {
            SeriesCollection.Clear();
            XLabels.Clear();
        }

        public void AddData(string from, string to, string interval, string attribute)
        {
            var dataPoints = DataFetcher.FetchData(from, to, interval);
            List<double> values;

            if (attribute == "Open")
            {
                values = dataPoints.Select(c => c.Open).ToList();
            }
            else if (attribute == "Close")
            {
                values = dataPoints.Select(c => c.Close).ToList();
            }
            else if (attribute == "High")
            {
                values = dataPoints.Select(c => c.High).ToList();
            }
            else
            {
                values = dataPoints.Select(c => c.Low).ToList();
            }

            SetLabels(interval, dataPoints);

            SeriesCollection.Add(new LineSeries
            {
                Title = $"{from}-{to}",
                Values = new ChartValues<double>(values),
                PointGeometry = null,
                LineSmoothness = 0,
                Fill = new SolidColorBrush()
            });

        }

        private void SetLabels(string interval, List<DataPoint> dataPoints)
        {
            if (interval.Contains("min"))
                XLabels = dataPoints.Select(c => c.TimeStamp.ToString("HH:mm")).ToList();
            else if (interval == "Monthly")
                XLabels = dataPoints.Select(c => c.TimeStamp.ToString("MMM yyyy.")).ToList();
            else
                XLabels = dataPoints.Select(c => c.TimeStamp.ToString("dd.MMM")).ToList();
            Console.WriteLine(XLabels.ToString());
        }
    }
}
