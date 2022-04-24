using LiveCharts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MiniProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public delegate void ClearData();
    public delegate void AddData();

    public partial class MainWindow : Window
    {
        public LineChart lineChart { get; set; }
        public BarChart barChart { get; set; }
        public event ClearData ClearEvent;
        public event AddData AddEvent;

        public MainWindow()
        {
            InitializeComponent();
            InitializeComboboxes();

            lineChart = new LineChart();
            barChart = new BarChart();
            ClearEvent += lineChart.Clear;
            ClearEvent += barChart.Clear;
            ClearEvent += DataFetcher.Flush;

            DataContext = this;
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            String fromCurrency = FromCurrency.Text.Substring(0, 3);
            String toCurrency = ToCurrency.Text.Substring(0,3);
            String intervalType = IntervalType.Text;
            String attribute = Attribute.Text;

            lineChart.AddData(fromCurrency.Substring(0,3), toCurrency.Substring(0,3), intervalType, attribute);
            barChart.AddData(fromCurrency.Substring(0,3), toCurrency.Substring(0,3), intervalType, attribute);
            LineXAxis.Labels = lineChart.XLabels;
            BarXAxis.Labels = barChart.XLabels;
            AddEvent?.Invoke();
        }

        private void ClearButtonClick(object sender, RoutedEventArgs e)
        {
            ClearEvent?.Invoke();
        }

        private void TableButtonClick(object sender, RoutedEventArgs e)
        {
            TableWindow tableWindow = new TableWindow();
            tableWindow.Show();
            ClearEvent += tableWindow.Clear;
            AddEvent += tableWindow.AddOption;
        }

        private void InitializeComboboxes()
        {
            List<String> intervals = new List<String>();
            List<String> attributes = new List<string>();

            intervals.Add("1min");
            intervals.Add("5min");
            intervals.Add("15min");
            intervals.Add("30min");
            intervals.Add("60min");
            intervals.Add("Daily");
            intervals.Add("Weekly");
            intervals.Add("Monthly");
            IntervalType.ItemsSource = intervals;
            IntervalType.SelectedIndex = 0;

            attributes.Add("Open");
            attributes.Add("Close");
            attributes.Add("High");
            attributes.Add("Low");
            Attribute.ItemsSource = attributes;
            Attribute.SelectedIndex = 0;

            LoadCSVData();
        }

        private void LoadCSVData()
        {
            using (var reader = new StreamReader("../../../physical_currency_list.csv"))
            {
                List<String> currencies = new List<String>();
                var line = reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    var values = line.Split(',');

                    currencies.Add($"{values[0]} - {values[1]}");
                }

                FromCurrency.ItemsSource = currencies;
                FromCurrency.SelectedIndex = 0;
                ToCurrency.ItemsSource = currencies;
                ToCurrency.SelectedIndex = 0;
            }
        }
    }
}
