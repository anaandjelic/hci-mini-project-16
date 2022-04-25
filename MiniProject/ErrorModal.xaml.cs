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
    /// Interaction logic for Modal.xaml
    /// </summary>
    public partial class ErrorModal : Window
    {
        private readonly double screenWidth = SystemParameters.PrimaryScreenWidth;
        private readonly double screenHeight = SystemParameters.PrimaryScreenHeight;

        public ErrorModal(string message)
        {
            InitializeComponent();

            Left = screenWidth / 2 - Width / 2;
            Top = screenHeight / 2 - Height / 2;

            Message.Text = message;
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
