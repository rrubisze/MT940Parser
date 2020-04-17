using MT940Parser.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace MT940Parser.ViewModels
{
    public class ReportViewModel: BaseViewModel
    {
        private readonly ParserContext _context;

        public ReportViewModel(ParserContext context)
        {
            _context = context;
        }
    }
}
