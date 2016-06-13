using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace SearchApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        VM vm;
        public MainWindow()
        {
            InitializeComponent();
            
            vm = new VM();
            this.DataContext = vm;
            vm.SearchEnded += SearchEnded;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            stopButton.Visibility = Visibility.Visible;
            searchButton.IsEnabled = false;
            progress.Visibility = Visibility.Visible;
            RunSearch();
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            vm.CancelAsync();
            SearchEnded();
        }

        private void RunSearch()
        {
            vm.SearchPath = pathToSearch.Text;
            vm.FileName = fileName.Text;
            vm.RunSearch();
        }

        private void SearchEnded()
        {
            stopButton.Visibility = Visibility.Collapsed;
            searchButton.IsEnabled = true;
            progress.Visibility = Visibility.Collapsed;
        }
    }
}
