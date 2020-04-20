using MT940Parser.Context;
using MT940Parser.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MT940Parser.ViewModels
{
    public class ReportViewModel: BaseViewModel
    {
        private ParserContext _context;
        private ObservableCollection<Report> _reports;
        private Summary _summary;
        private Report _selectedReport;
        private ObservableCollection<Transaction> _currentTransactions;

        #region - PROPS -
        public ObservableCollection<Report> Reports 
        {
            get => _reports;
            set => Set(ref _reports, value);
        }
        public Summary Summary 
        { 
            get => _summary; 
            set => Set(ref _summary, value); 
        }
        public Report SelectedReport
        {
            get => _selectedReport;
            set => Set(ref _selectedReport, value);
        }
        public ObservableCollection<Transaction> CurrentTransactions 
        { 
            get => _currentTransactions; 
            set => Set(ref _currentTransactions, value); 
        }

        #endregion

        public ReportViewModel(ParserContext context)
        {
            _context = context;
            _context.ContextChanged += ContextChanged;
        }

        private void ContextChanged(object sender, EventArgs e)
        {
            InitializeProperties();
        }

        private void InitializeProperties()
        {
            var reports = _context["Reports"] as IEnumerable<Report>;
            if (reports == null && reports.Count() == 0)
            {
                return;
            }

            this.Reports = new ObservableCollection<Report>(reports);
            this.SelectedReport = Reports.First();
            this.CurrentTransactions = new ObservableCollection<Transaction>(SelectedReport.Transactions);
        }

    }
}
