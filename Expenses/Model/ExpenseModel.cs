using System;

namespace Expenses.Model
{
    /// <summary>
    /// Reprsenet an expense with details such as amount, category, etc...
    /// Based on representation of Bank Discount
    /// </summary>
    public class ExpenseModel
    {
        /// <summary>
        /// the currency of the transaction, USD or NIS 
        /// default is NIS but can be USD
        /// </summary>
        public string Currency { get; set; }


        /// <summary>
        /// gets or sets final amount after credit card discounts
        /// </summary>
        public decimal BillingAmount { get; set; }    //column0


        /// <summary>
        /// gets or sets semi amount which is the original price before discounts or division into installments
        /// </summary>
        public decimal TransactionAmount { get; set; }     //column1


        /// <summary>
        /// category that the bank chose for the transaction such as food, etc..
        /// </summary>
        public string Category { get; set; }        //column2


        /// <summary>
        /// additional description from the bank
        /// </summary>
        public string Description { get; set; }     //column3


        /// <summary>
        /// the date when the transaction was made
        /// </summary>
        public DateTime Date { get; set; }          //column4


        /// <summary>
        /// User can associate a transaction to a specific customizedcategory and later on implement spcific calculations for each group of customizedcategories
        /// </summary>
        public string CustomizedCategory {  get; set; }
    }
}
