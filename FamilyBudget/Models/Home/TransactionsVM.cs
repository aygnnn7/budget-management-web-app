using FamilyBudget.Entities;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using static FamilyBudget.ProjectEnums;

namespace FamilyBudget.Models.Home
{
    public class TransactionsVM
    {
        public List<TransactionModified>? Transactions { get; set; }
        public TabActivity? TabActivity { get; set; }
        public Filters? Filters { get; set; }
        public string? SearchString { get; set; }
    }

    public class TransactionModified : Transaction
    {
        //added x daysago
        public int DaysAgo
        {
            get
            {
                return (int)(DateTime.Now - this.CreatedTime).TotalDays;
            }
        }
        //added x hoursago
        public int HoursAgo
        {
            get
            {
                return (int)(DateTime.Now - this.CreatedTime).TotalHours;
            }
        }

        //next repetition date of the repetitive transaction
        public DateTime NextRepetitionDate
        {
            get
            {
                DateTime now = DateTime.Now;
                if (this.RepeatDay < DateTime.Now.Day)
                {
                    DateTime afterOneMonth = now.AddMonths(1);
                    return new DateTime(afterOneMonth.Year, afterOneMonth.Month, this.RepeatDay,12,00,00);
                }

                else return new DateTime(now.Year, now.Month, this.RepeatDay, 12, 00, 00);
            }
        }

        public bool IsRepetitive
        {
            get
            {
                if (this.RepeatDay > 0) return true;
                else return false;
            }
        }

        public string GetTimeText { get {
                string text;
                if(this.RepeatDay == 0)
                {
                    if(this.DaysAgo == 0)
                    {
                        if(this.HoursAgo == 0) text = "Just now";
                        else text = $"{this.HoursAgo} hours ago";

                    }else text = $"{this.DaysAgo} days ago";

                    text += $" ({this.CreatedTime.ToString("MMM")})";
                }else text = $"Next repetition {this.NextRepetitionDate}";

                return text;
        }}

        public string GetValueText
        {
            get
            {
                string text = "";
                if (Value > 0) text+= "+" ;
                text += Value.ToString() + Currency.BGN.ToString();
                return text;
            }
        }

        public string GetValueClass
        {
            get
            {
                if (Value >= 0) return "val-income";
                else return "val-expense";
            }
        } 

        public string GetCategoryClass
        {
            get
            {
                if (Value >= 0) return "cat-income";
                else return "cat-expense";
            }
        }

        
    }

    //Defining which tab is active and which counter is visible 
    public class TabActivity
    {
        //stylings
        private string TabActive = "active";
        private string TabDefault = string.Empty;
        private string TabCountVisible = "visible";
        private string DefaultVisibility = "invisible";
        public int ActiveTabIndex { get; set; }
        public string ActiveTabTypeText
        {
            get
            {
                string type = "transactions";
                if (ActiveTabIndex == (int)Tab.Repetitions) type = "repetitions";
                return type;
            }
        }
        //Tab class name
        public string? All { get; set; }
        public string? RepTransactions { get; set; }
        public string? Transactions { get; set; }
        public string? Repetitions { get; set; }

        //Counter class name
        public string? AllCount { get; set; }
        public string? TransactionsCount { get; set; }
        public string? RepTransactionsCount { get; set; }
        public string? RepetitionsCount { get; set; }
        
        public TabActivity(int filter)
        {
            ActiveTabIndex= filter;
            All = TabDefault;
            Transactions = TabDefault;
            RepTransactions = TabDefault;
            Repetitions = TabDefault;

            AllCount = DefaultVisibility;
            TransactionsCount = DefaultVisibility;
            RepTransactionsCount = DefaultVisibility;
            RepetitionsCount = DefaultVisibility;

            switch (filter)
            {
                case (int)Tab.NormalTransactions:
                    Transactions = TabActive;
                    TransactionsCount = TabCountVisible;
                    break;

                case (int)Tab.RepetitiveTransactions:
                    RepTransactions = TabActive;
                    RepTransactionsCount = TabCountVisible;
                    break;

                case (int)Tab.Repetitions:

                    Repetitions = TabActive;
                    RepetitionsCount = TabCountVisible;
                    break;

                default:
                    All = TabActive;
                    AllCount = TabCountVisible;
                    break;
            }
        }
    }
}
