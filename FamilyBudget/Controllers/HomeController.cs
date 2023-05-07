using FamilyBudget.ActionFilters;
using FamilyBudget.Entities;
using FamilyBudget.ExtensionMethods;
using FamilyBudget.Models.Home;
using FamilyBudget.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Win32.TaskScheduler;
using System.ComponentModel;
using System.Globalization;
using static FamilyBudget.ProjectEnums;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FamilyBudget.Controllers
{

    public class HomeController : Controller
    {

        [OnlyNotLogged]
        public IActionResult Information()
        {
            return View();
        }

        [HttpGet, AuthFilter]
        public IActionResult Index()
        {
            User? loggedUser = this.HttpContext.Session.GetObject<User>("loggedUser");
            Db context = new Db();
            List<Transaction> allTransactions = context.transactions.ToList();
            
            decimal totalIncome = allTransactions.Where(t => t.Value > 0 && t.RepeatDay == 0 && t.OwnerId == loggedUser.Id).ToList().Sum(t => t.Value);
            decimal totalExpense = allTransactions.Where(t => t.Value < 0 && t.RepeatDay == 0 && t.OwnerId == loggedUser.Id).ToList().Sum(t => t.Value);
            
            int lastMonth = DateTime.Now.Month;
            int year = DateTime.Now.Year;

            //gets months -> January with index 1, February with index 2 etc.
            string[] months = new string[12];
            DateTime dt = new DateTime();
            for (int i = 0; i < months.Length; i++)
            {
                months[i] = dt.ToString("MMM");
                dt = dt.AddMonths(1);
            }
            // List of 6 Months for diagram with month name, month income and month expense
            List<MonthTransactions> monthTransactions = new();
            //last x months
            int monthCount = 6;
            for (int i = 0; i < monthCount; i++)
            {
                if (lastMonth == 1)
                {
                    lastMonth = 12;
                    year--;
                }
                else lastMonth--;
                int monthIndex = lastMonth;

                if (monthIndex == 0) monthIndex = 11;
                else monthIndex--;

                List<Transaction> transactionsByMonth = allTransactions.Where(tr => tr.CreatedTime.Month == lastMonth && tr.CreatedTime.Year == year).ToList();

                monthTransactions.Add(
                    //month name
                    new MonthTransactions(months[monthIndex].ToString(),
                    //month income
                    transactionsByMonth.Where(t => t.Value < 0).Sum(t => t.Value),
                    //month income
                    transactionsByMonth.Where(t => t.Value > 0).Sum(t => t.Value)));
            }

            monthTransactions.Reverse();
            HomeVM model = new HomeVM()
            {
                TotalBalance = totalIncome + totalExpense,
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
                MonthTransactions = monthTransactions
            };
            return View(model);
        }

        [AuthFilter]
        public IActionResult Search(string search)
        {
            return RedirectToAction("TransactionList", 
                new {search});
        }

        public IActionResult RemoveSearch(int tab, string filter)
        {
            return RedirectToAction("TransactionList",
               new {tab, otherFilters = filter });
        }

        [HttpPost, AuthFilter]
        public IActionResult Filter(TabSearchFiltersVM model)
        {
            //category 1, type 2, monthYear 3   
            model.Category = model.Category ?? Filters.NoFilter;
            model.Category = model.Category ?? Filters.NoFilter;
          
            string monthYearString = Filters.NoFilter;
            if (model.MonthYear != DateTime.MinValue ) monthYearString = model.MonthYear.ToString("MM.yyyy");

            string FilterString = model.Category + "-" + model.Type + "-" + monthYearString;
            return RedirectToAction("TransactionList", new { model.Tab, otherFilters = FilterString, search = model.Search});
        }

        public IActionResult RemoveFilter(int tab, string deleteFilter, string oldFilterString, string search)
        {
            Filters f = new Filters(oldFilterString);
            string newFilterString = f.RemoveFilter(deleteFilter);
            return RedirectToAction("TransactionList", new { tab, otherFilters = newFilterString, search });
        }
        [HttpGet, AuthFilter]
        public IActionResult TransactionList(int tab, string otherFilters, string search)
        {
            User? loggedUser = this.HttpContext.Session.GetObject<User>("loggedUser");
            Db dbContext = new Db();
            List<Transaction> transactions = dbContext.transactions.Where(t => t.OwnerId == loggedUser.Id).ToList();
            TransactionsVM modelList = new TransactionsVM();

            if (!string.IsNullOrEmpty(search))
            {
                transactions = transactions.Where(t =>t.Description.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                modelList.SearchString = search;
            }
            
            // load current filters 
            Filters filters = new Filters(otherFilters);
            modelList.Filters = filters;
            modelList.Transactions = new List<TransactionModified>();
            //filtering the transactions by tab
            switch (tab)
            {
                case (int)Tab.NormalTransactions:
                    modelList.TabActivity = new TabActivity((int)Tab.NormalTransactions);
                    transactions = transactions.Where(tr => tr.RepeatDay == 0 && tr.Category != Category.MonthlyRepetition.ToString()).ToList();
                    break;

                case (int)Tab.RepetitiveTransactions:
                    modelList.TabActivity = new TabActivity((int)Tab.RepetitiveTransactions);
                    transactions = transactions.Where(tr => tr.RepeatDay == 0 && tr.Category == Category.MonthlyRepetition.ToString()).ToList();
                    break;

                case (int)Tab.Repetitions:
                    modelList.TabActivity = new TabActivity((int)Tab.Repetitions);
                    transactions = transactions.Where(tr => tr.RepeatDay > 0).ToList();
                    break;

                default:
                    modelList.TabActivity = new TabActivity((int)Tab.All);
                    transactions = transactions.Where(tr => tr.RepeatDay == 0).ToList();
                    break;
            }

            //filtering by Category
            if (filters.HasCategory)
            {
                transactions = transactions.Where(t => t.Category == filters.Category.ToString()).ToList();
            }
            //filtering by Type
            if (filters.HasType)
            {
                if (filters.Type == TransactionType.Income.ToString()) transactions = transactions.Where(t => t.Value >= 0).ToList();
                else transactions = transactions.Where(t => t.Value < 0).ToList();
            }
            //filtering by DateTime
            if (filters.HasMonthYear)
            {
                transactions = transactions.Where(t => t.CreatedTime.Month == filters.MonthYear.Month 
                                                         && t.CreatedTime.Year == filters.MonthYear.Year).ToList();
            }

            foreach (Transaction transaction in transactions)
            {
                modelList.Transactions.Add(new TransactionModified
                {
                    CreatedTime = transaction.CreatedTime,
                    ModifiedTime = transaction.ModifiedTime,
                    Description = transaction.Description,
                    Category = transaction.Category,
                    Value = transaction.Value,
                    Id = transaction.Id,
                    RepeatDay = transaction.RepeatDay
                });
            }

            //Sorting by created time (descending)
            modelList.Transactions = modelList.Transactions.OrderByDescending(t => t.CreatedTime).ToList();
            return View(modelList);
        }

        [AuthFilter, OnlyOwnerUser]
        public IActionResult Delete(int id)
        {
            

            Db context = new Db();
            //Finding and deleting the specific transaction
            Transaction? transaction = context.transactions.Where(t => t.Id == id).FirstOrDefault();
           
            if (transaction != null)
            {
                context.transactions.Remove(transaction);
                context.SaveChanges();
            }
            return RedirectToAction("TransactionList", "Home");
        }
        
        [HttpGet, AuthFilter, OnlyOwnerUser]
        public IActionResult Manage(int id, bool isCreateRepetitive)
        {

            //Id == 0 means creating
            if (id == 0)
            {
                return View(new ManageVM
                {
                    IsCreate = true,
                    IsRepetitive = isCreateRepetitive
                });
            }
            //Else its editing
            
            Db context = new();
            Transaction ?transaction = context.transactions.Where(t => t.Id == id).FirstOrDefault();
            
            if (transaction != null)
            {
                string type = "";
                if (transaction.Value >= 0) type = TransactionType.Income.ToString();
                else
                {
                    type = TransactionType.Expense.ToString();
                    transaction.Value *= -1;
                }

                ManageVM model = new ManageVM()
                {
                    Id = id,
                    Category = transaction.Category,
                    Value = transaction.Value,
                    Description = transaction.Description,
                    Type = type,
                    CreatedTime = transaction.CreatedTime,
                    RepeatDay = transaction.RepeatDay
                };
                return View(model);
            }

            return View();
        }
        
        //sorun var yeni eklemede

        [HttpPost, AuthFilter,OnlyOwnerUser]
        public IActionResult Manage(ManageVM model)
        {
            //I couldn't solve the validation problem here RepeatDay property always has error - "This field is required", I didn't add required in the model
            if (!ModelState.IsValid && !(ModelState.ErrorCount == 1 && model.RepeatDay == 0) && model.IsCreate)
            {
                return View(model);
            }
          
            if (!ModelState.IsValid && !model.IsCreate)
                return View(model);
            //make the number negative if the type is expense
            if (model.Type == TransactionType.Expense.ToString())
            {
                model.Value *= -1;
            }
            
            Db dbContext = new Db();
            Transaction? oldTranscaction = dbContext.transactions.Where(t => t.Id == model.Id).FirstOrDefault();
            User? loggedUser = HttpContext.Session.GetObject<User>("loggedUser");
            if(oldTranscaction != null)  dbContext.Entry(oldTranscaction).State = EntityState.Detached;
            Transaction? tr = new Transaction()
            {
                Id = model.Id,
                Category = model.Category,
                Value = model.Value,
                Description = model.Description,
                ModifiedTime = DateTime.Now,
                RepeatDay = model.RepeatDay
                
            };
            
            //add
            if (model.IsCreate)
            {
                tr.CreatedTime = DateTime.Now;
                tr.OwnerId = loggedUser.Id;
                dbContext.transactions.Add(tr);
             
            }
            //edit
            else
            {
                tr.CreatedTime = oldTranscaction.CreatedTime;
                tr.OwnerId = oldTranscaction.OwnerId;
                dbContext.transactions.Update(tr);
            }

            dbContext.SaveChanges();
            return RedirectToAction("TransactionList", "Home");
        }

        //Called from task
        public void CheckDailyForRepetitions()
        {
            User? loggedUser = this.HttpContext.Session.GetObject<User>("loggedUser");
            //This method is called every day -> Task.cs
            Db context = new();
            List<Transaction>? repetitions  = new();
            //check if transactions repeatday is today and add new transaction if it is
            repetitions = context.transactions?.Where(t => t.RepeatDay == DateTime.Now.Day && t.OwnerId == loggedUser.Id).ToList();
            
            if(repetitions.Count > 0)
            {
                List<Transaction> newTransactions = new();
                foreach (var transaction in repetitions)
                {
                    Transaction newTransaction = new Transaction()
                    {
                        Category = Category.MonthlyRepetition.ToString(),
                        Value = transaction.Value,
                        CreatedTime = DateTime.Now,
                        ModifiedTime = DateTime.Now,
                        Description = "Montly Transaction (" + transaction.Category + "): " + transaction.Description,
                        RepeatDay = 0
                    };
                    newTransactions.Add(newTransaction);
                }
                context.UpdateRange(newTransactions);
                context.SaveChanges();
            }
        }

    }
} 