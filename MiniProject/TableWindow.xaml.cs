using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MiniProject
{
    /// <summary>
    /// Interaction logic for TableWindow.xaml
    /// </summary>
    public partial class TableWindow : Window
    {

        public TableWindow()
        {
            InitializeComponent();

            currenciePairesCB.ItemsSource = DataFetcher.GetCacheKeys();
        }

        private void currenciePairesCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dataTable.Items.Clear();
            if (currenciePairesCB.ItemsSource != null)
            {
                string selectedValue = currenciePairesCB.SelectedValue.ToString();
                string[] arguments = selectedValue.Split("-");

                List<DataPoint> dataTableValues = DataFetcher.FetchData(arguments[0].Trim(), arguments[1].Trim(), arguments[2].Trim());
                foreach (DataPoint dataPoint in dataTableValues)
                {
                    dataTable.Items.Add(dataPoint);
                }
            }
        }


        public void Clear()
        {
            currenciePairesCB.ItemsSource = null;
        }

        public void AddOption()
        {
            currenciePairesCB.ItemsSource = DataFetcher.GetCacheKeys();
        }
    }
}
