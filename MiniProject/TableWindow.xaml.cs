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
        // preuzimanje podataka sa api-a
        private DataFetcher data = new DataFetcher();
        private List<string> firstCurrencys;
        private List<string> secondCurrencys;
        private List<string> intervals;

        public TableWindow(List<string> currenciesData1, List<string> currenciesData2, List<string> intervalsData)
        {
            InitializeComponent();
            firstCurrencys = new List<string>(currenciesData1);
            secondCurrencys = new List<string>(currenciesData2);
            intervals = new List<string>(intervalsData);
            // popunjavanje comboboxa
            for(int i=0;i<firstCurrencys.Count;i++)
            {
                currenciePairesCB.Items.Add(firstCurrencys[i] + " >>>>>> " + secondCurrencys[i] + " >>>>>> " + intervals[i]);
            }
        }

        private void currenciePairesCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // kada se u cb vrsi izbor valuta, mi kupimo te podatke i ovde ih kreiramo oodnosno postavljamo
            // currenciePairesCB.SelectedIndex dobijamo index selektovanog s njim mozemo da pristupimo svim ostalim podacima na istom indexu zato sto ih dodajemo u isto vreme
            dataTable.Items.Clear();
            List<DataPoint> dataTableValues = fetchData(firstCurrencys[currenciePairesCB.SelectedIndex], secondCurrencys[currenciePairesCB.SelectedIndex], intervals[currenciePairesCB.SelectedIndex]);
            foreach (DataPoint dataPoint in dataTableValues)
            {
                dataTable.Items.Add(dataPoint);
            }

        }

        private List<DataPoint> fetchData(string firstCurrency, string secondCurrency, string interval)
        {
            //return data.FetchData(firstCurrency, secondCurrency, interval);
            //podaci iz apija koje pribavljam
            return DataFetcher.FetchData(firstCurrency.Substring(0,3), secondCurrency.Substring(0, 3), interval);
        }
    }
}
