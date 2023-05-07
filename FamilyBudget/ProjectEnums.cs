namespace FamilyBudget
{
    public class ProjectEnums
    {
        //transaction categories
        public enum Category
        {
            General,
            Bill,
            Food,
            Fun,
            Shopping,
            Health,
            Home,
            Investment,
            Maintenance,
            Salary,
            Transport,
            Travel,
            Scholarship,
            MonthlyRepetition
        }
        //transaction type
        public enum TransactionType
        {
            Income,
            Expense
        }

        //transaction list filters
        public enum Tab
        {
            All = 0,
            NormalTransactions = 1,
            RepetitiveTransactions = 2,
            Repetitions = 3
        }

        public enum Sex
        {
            Man,
            Woman
        }

        public enum Currency
        {
            BGN,
            EUR,
            TRY
        }

        public static List<string> GetCategories()
        {
            return Enum.GetNames(typeof(Category)).ToList();
        }
        public static List<string> GetTypes()
        {
            
            return Enum.GetNames(typeof(TransactionType)).ToList();
        }
    }
}
