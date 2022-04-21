using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Windows.Controls;
using System.Windows.Media;

namespace MiniProject
{
    public partial class LineChart : UserControl
    {
        private Dictionary<string, string> IntervalToQuery = new Dictionary<string, string>
        {
            { "1min", "FX_INTRADAY" },
            { "5min", "FX_INTRADAY" },
            { "15min", "FX_INTRADAY" },
            { "30min", "FX_INTRADAY" },
            { "60min", "FX_INTRADAY" },
            { "Daily", "FX_DAILY" },
            { "Weekly", "FX_WEEKLY" },
            { "Monthly", "FX_MONTHLY" }
        };
        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public LineChart()
        {

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = new ChartValues<double> { 4, 6, 5, 2 ,4 }
                },
                new LineSeries
                {
                    Title = "Series 2",
                    Values = new ChartValues<double> { 6, 7, 3, 4 ,6 },
                    PointGeometry = null
                },
                new LineSeries
                {
                    Title = "Series 3",
                    Values = new ChartValues<double> { 4,2,7,2,7 },
                    PointGeometry = null
                }
            };

            YFormatter = value => value.ToString("C");
        }

        public void Clear() 
        {
            SeriesCollection.Clear();
            Labels.Clear();
        }

        public void AddData(string from, string to, string interval, string attribute)
        {

        }

        private List<DataPoint> GetData(string firstCurrency, string secondCurrency, string interval, string attribute)
        {
            // You might not need this wrapper function but I kept it to keep track of changes.


            return DataFetcher.FetchData(firstCurrency, secondCurrency, interval);
        }
    }
}
