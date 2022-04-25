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
    /// Interaction logic for ChoiceModal.xaml
    /// </summary>
    public partial class ChoiceModal : Window
    {
        private readonly double ScreenWidth = SystemParameters.PrimaryScreenWidth;
        private readonly double ScreenHeight = SystemParameters.PrimaryScreenHeight;
        public string Choice { get; private set; }

        public ChoiceModal()
        {
            InitializeComponent();

            Left = ScreenWidth / 2 - Width / 2;
            Top = ScreenHeight / 2 - Height / 2;
        }
        private void DrawButtonClick(object sender, RoutedEventArgs e)
        {
            Choice = "Draw";
            Close();
        }
        private void ContButtonClick(object sender, RoutedEventArgs e)
        {
            Choice = "Continue";
            Close();
        }
    }

}
