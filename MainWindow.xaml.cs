using Raptorious.SharpMt940Lib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
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

namespace MT940Parser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            System.Text.EncodingProvider provider = System.Text.CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(provider);
        }

        private async void ImagePanel_Drop(object sender, DragEventArgs e)
        {
            this.Info.Content = "";
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if(files.Count() > 1 )
                {
                    this.Info.Content = "You can drop only one file";
                    return;
                }

                try
                {
                    if(System.IO.Path.GetExtension(files[0]) != ".sta")
                    {
                        this.Info.Content = "Wrong file extension. It must be .sta!";
                        return;
                    }
                    await ProcessCsvFile(files[0]);
                    this.Info.Content = "";
                }
                catch(Exception ex)
                {
                    this.Info.Content = ex.Message;
                    return;
                }
            
            }
        }

        private async static Task ProcessCsvFile(string mt940FilePath)
        {
            var cultureInfo = new CultureInfo("pl-PL"); // ABN-AMRO uses decimal comma; https://en.wikipedia.org/wiki/Decimal_mark#Countries_using_Arabic_numerals_with_decimal_comma
            var parameters = new Parameters();

            parameters.AddCodeFor(TransactionDetail.Account, "~29", "~30", "~31", "~38");
            parameters.AddCodeFor(TransactionDetail.TransactionType, "~00");
            parameters.AddCodeFor(TransactionDetail.Description, "22", "~20", "~21");
            parameters.AddCodeFor(TransactionDetail.Name, "~32", "~33", "~62", "~63");
            parameters.AddCodeFor(TransactionDetail.Empty, "073");

            ICollection<CustomerStatementMessage> statements = Mt940Parser.Parse(new IngFormat(), mt940FilePath, cultureInfo, parameters);

            var reports = GenerateReports(statements);

            var output = System.IO.Path.ChangeExtension(mt940FilePath, "csv");

            using (var writer = new StreamWriter(output, false, Encoding.GetEncoding("ibm852")))
            {
                foreach(var report in reports)
                {
                    await writer.WriteLineAsync(report.Summary.GetCSVHeader());
                    await writer.WriteLineAsync(report.Summary.GetCSVRow());
                    await writer.WriteLineAsync(report.Transactions.First().GetCSVHeader());
                    foreach (var trans in reports.First().Transactions)
                    {
                        await writer.WriteLineAsync(trans.GetCSVRow());
                    }
                    await writer.WriteLineAsync(Environment.NewLine);
                }
            }
        }

        private static IEnumerable<Report> GenerateReports(ICollection<CustomerStatementMessage> statements)
        {
            ICollection<Report> reports = new List<Report>();
            foreach (var statement in statements)
            {
                var currentStatement = new Report();

                var summarrRow = statement?.Transactions.FirstOrDefault(x => x.TransactionType == TransactionTypes.SUMMARY);
                if (summarrRow != null)
                {
                    var summary = new Summary
                    {
                        OpeningBalance = statement.OpeningBalance.Balance.Value,
                        ClosingBalance = statement.ClosingBalance.Balance.Value,
                        ClosingAvailableBalance = statement.ClosingAvailableBalance.Balance.Value,
                        BlockedBalance = statement.ClosingBalance.Balance.Value - statement.ClosingAvailableBalance.Balance.Value,
                        Account = statement.Account.Replace("/", ""),
                        Description = $"\"{summarrRow.Details.Description}\"",
                        Date = summarrRow.ValueDate.ToIso8601DateOnly(),
                        Currency = summarrRow.Amount.Currency,
                    };

                    currentStatement.Summary = summary;
                }

                foreach (var transcation in statement.Transactions.Where(x => x.TransactionType != TransactionTypes.SUMMARY))
                {
                    var transactionItem = new Transaction
                    {
                        Date = transcation.ValueDate.ToIso8601DateOnly(),
                        Reference = transcation.Reference,
                        DebitOrCredit = transcation.DebitCredit == DebitCredit.Debit ? "Debit" : "Credit",
                        Account = transcation.Details.Account,
                        AccountName = $"\"{transcation.Details.Name}\"",
                        Description = $"\"{transcation.Details.Description}\"",
                        TransactionType = TransactionTypes.GetTransactionTypeString(transcation.TransactionType),
                        Value = transcation.DebitCredit == DebitCredit.Credit ? transcation.Amount.Value : transcation.Amount.Value * -1,
                        Currency = transcation.Amount.Currency
                    };

                    currentStatement.Transactions.Add(transactionItem);
                }

                reports.Add(currentStatement);
            }
            return reports;
        }
    }
}
