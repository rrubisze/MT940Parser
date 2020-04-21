using MT940Parser.Context;
using MT940Parser.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Raptorious.SharpMt940Lib;
using Transaction = MT940Parser.Models.Transaction;

namespace MT940Parser.ViewModels
{
    public class ReportViewModel : BaseViewModel
    {
        private ParserContext _context;
        private ObservableCollection<Report> _reports;
        private Summary _summary;
        private Report _selectedReport;
        private ObservableCollection<Transaction> _currentTransactions;
        private Totals _totals;
        private bool _anyReport;

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
            set
            {
                Set(ref _selectedReport, value);
                this.CurrentTransactions = new ObservableCollection<Transaction>(_selectedReport.Transactions);
            }
        }
        public ObservableCollection<Transaction> CurrentTransactions
        {
            get => _currentTransactions;
            set => Set(ref _currentTransactions, value);
        }
        public Totals Totals
        {
            get => _totals;
            set
            {
                Set(ref _totals, value);
            }
        }

        public bool AnyReport { get => _anyReport; set => Set(ref _anyReport, value); }
        #endregion

        public ReportViewModel(ParserContext context)
        {
            _context = context;
            _context.ContextChanged += ContextChanged;
        }

        private void ContextChanged(object sender, EventArgs e)
        {
            if (_context["Reports"] == null)
            {
                this.AnyReport = false;
                return;
            }
            InitializeProperties();
        }

        private void InitializeProperties()
        {
            var reports = _context["Reports"] as IEnumerable<Report>;
            if (reports == null && reports.Count() == 0)
            {
                this.AnyReport = false;
                return;
            }

            this.AnyReport = true;

            this.Reports = new ObservableCollection<Report>(reports);
            this.SelectedReport = Reports.First();
            this.CurrentTransactions = new ObservableCollection<Transaction>(SelectedReport.Transactions);
            Totals = new Totals()
            {
                TotalIncome = reports.SelectMany(x => x.Transactions).Where(x => x.DebitOrCredit == DebitCredit.Credit).Sum(x => x.Value),
                TotalOutcome = reports.SelectMany(x => x.Transactions).Where(x => x.DebitOrCredit == DebitCredit.Debit).Sum(x => x.Value)
            };

        }

    }
}
