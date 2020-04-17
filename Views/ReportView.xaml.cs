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
    /// Interaction logic for ReportView.xaml
    /// </summary>
    public partial class ReportView : Page
    {
        private ReportViewModel _viewModel;

        public ReportView()
        {
            InitializeComponent();
            using (var serviceScope = Resolver.GetScope())
            {
                _viewModel = (ReportViewModel)serviceScope.ServiceProvider.GetService(typeof(ReportViewModel));
            }
            this.DataContext = _viewModel;
        }
    }
}
