﻿using MT940Parser.Extensions;
using MT940Parser.Models;
using Raptorious.SharpMt940Lib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transaction = MT940Parser.Models.Transaction;

namespace MT940Parser.Services
{
    public class Mt940Service
    {

        public ICollection<CustomerStatementMessage> GenerateStatements(string mt940FilePath)
        {
            var cultureInfo = new CultureInfo("pl-PL");
            var parameters = new Parameters();

            parameters.AddCodeFor(TransactionDetail.Account, "~29", "~30", "~31", "~38");
            parameters.AddCodeFor(TransactionDetail.TransactionType, "~00");
            parameters.AddCodeFor(TransactionDetail.Description, "22", "~20", "~21");
            parameters.AddCodeFor(TransactionDetail.Name, "~32", "~33", "~62", "~63");
            parameters.AddCodeFor(TransactionDetail.Empty, "073");

            return Mt940Parser.Parse(new IngFormat(), mt940FilePath, cultureInfo, parameters);         
        }

        public IEnumerable<Report> GenerateReports(ICollection<CustomerStatementMessage> statements)
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

        public async Task GenerateReportCSV(IEnumerable<Report> reports, string outputPath)
        {
 
            using (var writer = new StreamWriter(outputPath, false, Encoding.GetEncoding("ibm852")))
            {
                foreach (var report in reports)
                {
                    await writer.WriteLineAsync(report.Summary.GetCSVHeader());
                    await writer.WriteLineAsync(report.Summary.GetCSVRow());

                    await writer.WriteLineAsync(report.Transactions.First().GetCSVHeader());

                    foreach (var trans in report.Transactions)
                    {
                        await writer.WriteLineAsync(trans.GetCSVRow());
                    }
                    await writer.WriteLineAsync(Environment.NewLine);
                }
            }
        }
    }
}
