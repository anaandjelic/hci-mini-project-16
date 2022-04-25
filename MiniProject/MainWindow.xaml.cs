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

        private readonly double screenWidth = SystemParameters.PrimaryScreenWidth;
        private readonly double screenHeight = SystemParameters.PrimaryScreenHeight;

        private List<string> currencies = new List<string>();
        private readonly List<String> intervals = new List<string> { "1min", "5min" , "15min", "30min", "60min", "Daily", "Weekly", "Monthly" };
        private readonly List<String> attributes = new List<string> { "Open", "Close", "High", "Low" };

        public MainWindow()
        {

            InitializeComponent();
            InitializeComboboxes();

            Left = screenWidth / 2 - Width / 2;
            Top = screenHeight / 2 - Height / 2;

            lineChart = new LineChart();
            barChart = new BarChart();
            ClearEvent += lineChart.Clear;
            ClearEvent += barChart.Clear;
            ClearEvent += DataFetcher.Flush;

            DataContext = this;
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                string fromCurrency = FromCurrency.Text.Substring(0, 3);
                string toCurrency = ToCurrency.Text.Substring(0, 3);
                string intervalType = IntervalType.Text;
                string attribute = Attribute.Text;

                if (!currencies.Contains(FromCurrency.Text) ||
                    !currencies.Contains(ToCurrency.Text) ||
                    !intervals.Contains(intervalType) ||
                    !attributes.Contains(attribute))
                {
                    Modal modal = new Modal("You have to enter valid values for all fields. Please try again.");
                    modal.ShowDialog();
                    return;
                }
                else if (fromCurrency == toCurrency)
                {
                    Modal modal = new Modal("Currencies can't be the same.\nPlease try again.");
                    modal.ShowDialog();
                    return;
                }

                List<DataPoint> data = DataFetcher.FetchData(fromCurrency, toCurrency, intervalType);

                lineChart.AddData(fromCurrency, toCurrency, intervalType, attribute);
                barChart.AddData(fromCurrency, toCurrency, intervalType, attribute);
                LineXAxis.Labels = lineChart.XLabels;
                BarXAxis.Labels = barChart.XLabels;
                AddEvent?.Invoke();
            }
            catch (NoValuesFoundException)
            {
                Modal modal = new Modal("Looks like API hasn't returned any values. Please try again.");
                modal.ShowDialog();
            }
            catch (APITimedOutException)
            {
                Modal modal = new Modal("Looks like we reached our limit of 5 API calls per minute. Please try again later.");
                modal.ShowDialog();
            }
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
            IntervalType.ItemsSource = intervals;
            IntervalType.SelectedIndex = 0;

            Attribute.ItemsSource = attributes;
            Attribute.SelectedIndex = 0;

            LoadCSVData();
        }

        private void LoadCSVData()
        {
            using (var reader = new StreamReader("../../../physical_currency_list.csv"))
            {
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
