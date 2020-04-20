using MT940Parser.Context;
using MT940Parser.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MT940Parser.ViewModels
{
    public class ReportViewModel: BaseViewModel, IDisposable
    {
        private ParserContext _context;
        private ObservableCollection<Report> _reports;
        private Summary _summary;

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
        #endregion

        public ReportViewModel(ParserContext context)
        {
            _context = context;
            _context.ContextChanged += ContextChanged;
        }

        public void Dispose()
        {
            _context = null;
        }

        private void ContextChanged(object sender, EventArgs e)
        {
            var reports = _context["Reports"] as IEnumerable<Report>;
            if(reports != null && reports.Count() > 0)
            {
                
            }

            this.Reports = new ObservableCollection<Report>(reports);
        }

    }
}
