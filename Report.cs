using Raptorious.SharpMt940Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT940Parser
{

    public interface ICSV
    {
        string GetCSVRow();
        string GetCSVHeader();
    }

    public class Report
    {
        public Report()
        {
            Transactions = new List<Transaction>();
        }
        public Summary Summary { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }

    public class Summary : ICSV
    {
        public decimal OpeningBalance { get; set; }
        public decimal ClosingBalance { get; set; }
        public decimal ClosingAvailableBalance { get; set; }
        public decimal BlockedBalance { get; set; }
        public string Account { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
        public Currency Currency { get; set; }

        public string GetCSVHeader()
        {
            var result = new StringBuilder();
            result.Append(nameof(OpeningBalance));
            result.Append(",");
            result.Append(nameof(ClosingBalance));
            result.Append(",");
            result.Append(nameof(ClosingAvailableBalance));
            result.Append(",");
            result.Append(nameof(BlockedBalance));
            result.Append(",");
            result.Append(nameof(Account));
            result.Append(",");
            result.Append(nameof(Description));
            result.Append(",");
            result.Append(nameof(Date));
            result.Append(",");
            result.Append(nameof(Currency));
            result.Append(",");

            return result.ToString();
        }

        public string GetCSVRow()
        {
            var result = new StringBuilder();
            result.Append(OpeningBalance);
            result.Append(",");
            result.Append(ClosingBalance);
            result.Append(",");
            result.Append(ClosingAvailableBalance);
            result.Append(",");
            result.Append(BlockedBalance);
            result.Append(",");
            result.Append(Account);
            result.Append(",");
            result.Append(Description);
            result.Append(",");
            result.Append(Date);
            result.Append(",");
            result.Append(Currency);
            result.Append(",");

            return result.ToString();
        }
    }

    public class Transaction : ICSV
    {
        public string Date { get; set; }
        public string Reference { get; set; }
        public string DebitOrCredit { get; set; }
        public string Account { get; set; }
        public string AccountName { get; set; }
        public string Description { get; set; }
        public string TransactionType { get; set; }
        public decimal Value { get; set; }
        public Currency Currency { get; set; }

        public string GetCSVHeader()
        {
            var result = new StringBuilder();
            result.Append(nameof(Date));
            result.Append(",");
            result.Append(nameof(Reference));
            result.Append(",");
            result.Append(nameof(DebitOrCredit));
            result.Append(",");
            result.Append(nameof(Account));
            result.Append(",");
            result.Append(nameof(AccountName));
            result.Append(",");
            result.Append(nameof(Description));
            result.Append(",");
            result.Append(nameof(TransactionType));
            result.Append(",");
            result.Append(nameof(Value));
            result.Append(",");
            result.Append(nameof(Currency));
            result.Append(",");

            return result.ToString();
        }

        public string GetCSVRow()
        {
            var result = new StringBuilder();
            result.Append(Date);
            result.Append(",");
            result.Append(Reference);
            result.Append(",");
            result.Append(DebitOrCredit);
            result.Append(",");
            result.Append(Account);
            result.Append(",");
            result.Append(AccountName);
            result.Append(",");
            result.Append(Description);
            result.Append(",");
            result.Append(TransactionType);
            result.Append(",");
            result.Append(Value);
            result.Append(",");
            result.Append(Currency);
            result.Append(",");

            return result.ToString();
        }
    }
}
