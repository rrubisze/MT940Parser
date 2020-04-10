using System;
using System.Collections.Generic;
using System.Text;

namespace Raptorious.SharpMt940Lib
{
    /// <summary>
    /// Transcation Types
    /// </summary>
    public static class TransactionTypes
    {
        /// <summary>
        /// Summary transcation
        /// </summary>
        public const string SUMMARY = "S940";
        /// <summary>
        /// Transaction type money transfer
        /// </summary>
        public const string TRANSFER = "S073";
        /// <summary>
        /// Transaction type make by card
        /// </summary>
        public const string CARD = "S034";

        /// <summary>
        /// Get transactionTypeString (you can extend it)
        /// </summary>
        /// <returns></returns>
        public static string GetTransactionTypeString(string transactionType)
        {
            switch (transactionType)
            {
                case SUMMARY:
                    return "Informacje";
                case TRANSFER:
                    return "Przelew";
                case CARD:
                    return "Karta";
                default:
                    return "";
            }
        }
    }
}
