using Expenses.Model;
using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using DocumentFormat.OpenXml.VariantTypes;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Globalization;

namespace Expenses.ViewModel
{
    public class ExcelReader
    {
        private const string USDString = "$";
        private const string NISString = "₪";
        private const string DefaultCurrency = NISString;

        private static readonly List<String> Currencies = new List<String>() { USDString, NISString };

        /// <summary>
        /// In some transactions the bank added currency to the row and in some did not.
        /// when it did add the currency, a formatexception is thrown.
        /// function looks for known currencies and returns the parsed amountText without as a decimal with only digits.
        /// default currency is NIS.
        /// </summary>
        private static (decimal,string) handleAmount(string finalAmountText)
        {
            decimal? finalAmountDecimal = 0;

            try
            {
                finalAmountDecimal = decimal.Parse(finalAmountText);
            }
            catch (FormatException ex)
            {
                foreach (var _currency in Currencies)
                {
                    finalAmountDecimal = handleCurrency(finalAmountText, _currency);
                    if (finalAmountDecimal.HasValue)
                        return (finalAmountDecimal.Value, _currency);
                }
                Console.WriteLine(ex.ToString());
            }
            Console.WriteLine($"at ReadExpenses encountered error with finalAmountText={finalAmountText} where no currency was found and yet a FormatException is thrown.");
            return (finalAmountDecimal.Value, DefaultCurrency);
        }

        /// <summary>
        /// In some transactions the bank added currency to the row and in some did not.
        /// when it did add the currency, a formatexception is thrown.
        /// function looks for known currencies and returns the parsed amountText without as a decimal with only digits.
        /// </summary>
        private static decimal? handleCurrency(string amountText, string currency)
        {
            if (amountText.Contains(currency))
                return decimal.Parse(amountText.Replace(currency, ""));
            return null;
        }

        /// <summary>
        /// Reads provided excel file and sets ObservableCollection of ExpenseModel with data from the excel.
        /// Does some processing work on the data like processing currencies and categories.
        /// </summary>
        public static ObservableCollection<ExpenseModel> ReadExpenses(string filePath)
        {
            //initialize empty ExpoenseModel ObservableCollection
            var expenses = new ObservableCollection<ExpenseModel>(); 

            // Ensure that the file exists
            if (!File.Exists(filePath))
                throw new FileNotFoundException("The specified file does not exist.", filePath);

            FileInfo fileInfo = new FileInfo(filePath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Assuming the data is in the first worksheet
                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;

                for (int row = 2; row <= rowCount; row++) // Assuming the first row contains headers
                {
                    (decimal finalAmountDecimal, string currency) = handleAmount(worksheet.Cells[row, 1].Text);
                    var category = worksheet.Cells[row, 3].Text;
                    var description = worksheet.Cells[row, 4].Text;
                    var date = DateTime.ParseExact(worksheet.Cells[row, 5].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    var expense = new ExpenseModel
                    {
                        FinalAmount = finalAmountDecimal,
                        Category = category,
                        Description = description,
                        Date = date,
                        Currency = currency,
                    };

                    expenses.Add(expense);
                }
            }

            return expenses;
        }
    }
}
