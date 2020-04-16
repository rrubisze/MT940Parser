using MahApps.Metro.Controls;
using MT940Parser.Commands;
using MT940Parser.Extensions;
using MT940Parser.Models;
using MT940Parser.ViewModels;
using Raptorious.SharpMt940Lib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Transaction = MT940Parser.Models.Transaction;

namespace MT940Parser.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private MainWindowViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            System.Text.EncodingProvider provider = System.Text.CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(provider);
            using (var serviceScope = Resolver.GetScope())
            {
                _viewModel = (MainWindowViewModel)serviceScope.ServiceProvider.GetService(typeof(MainWindowViewModel));
            }
            this.DataContext = _viewModel;
        }

        private async void ImagePanel_Drop(object sender, DragEventArgs e)
        {
            //this.Info.Content = "";
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                await _viewModel.DropFileCommand.ExecuteAsync(files);            
            }
        }
    }
}
