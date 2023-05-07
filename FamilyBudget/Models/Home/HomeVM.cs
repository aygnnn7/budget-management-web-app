using Microsoft.Win32.TaskScheduler;
using static FamilyBudget.ProjectEnums;

namespace FamilyBudget.Models.Home
{
    public class HomeVM
    {
        public decimal TotalBalance { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public List<MonthTransactions>? MonthTransactions { get; set; }
        public string CurrentCurrency { get { return Currency.BGN.ToString(); } }


    }

    //Month details for the diagrams
    public class MonthTransactions
    {
        public MonthTransactions(string month, decimal expense, decimal income)
        {
            Month = month;
            Expense = expense;
            Income = income;
        }

        public string Month { get; set; }
        public decimal Expense { get; set; }

        public decimal Income { get; set; }

        public decimal Balance { get
            {
                return this.Income + this.Expense; 
            }
        }
    }
}
