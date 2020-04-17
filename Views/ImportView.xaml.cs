using MT940Parser.Extensions;
using MT940Parser.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MT940Parser.Views
{
    /// <summary>
    /// Interaction logic for ImportView.xaml
    /// </summary>
    public partial class ImportView : Page
    {
        private ImportViewModel _viewModel;

        public ImportView()
        {
            InitializeComponent();
            using (var serviceScope = Resolver.GetScope())
            {
                _viewModel = (ImportViewModel)serviceScope.ServiceProvider.GetService(typeof(ImportViewModel));
            }
            this.DataContext = _viewModel;
        }

        private async void DropContainer_Drop(object sender, DragEventArgs e)
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
