using FamilyBudget.Entities;
using FamilyBudget.Models.Home;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using static FamilyBudget.ProjectEnums;

namespace FamilyBudget.Repositories
{
    public class Db : DbContext
    {
        public DbSet<Transaction> transactions { get; set; }
        public DbSet<User> users { get; set; }


        //Connection String
        private string sqlConnectionString = "Server=(LocalDb)\\LocalDB;Database=FamilyBudget;Trusted_Connection=True;";

        public Db()
        {
            this.transactions = this.Set<Transaction>();
            this.users = this.Set<User>();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // sql connection string
            optionsBuilder
                .UseSqlServer(sqlConnectionString)
                .UseLazyLoadingProxies();

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            int id = 1;
            Random rn = new();
            List<Transaction> transactions = new List<Transaction>();
            int userId = 1;
            List<User> users = new List<User>()
            {
                 new User()
                 {
                     Email = "test@test.com",
                     Id = userId,
                     Name = "test",
                     Password = "testpass",
                     RegisteredTime = DateTime.Now,
                     Sex = true
                 }
            };
            string dummyText = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since";
            //Adding random transactions in database
            for (int i = id; i <= 6; i++)
            {
                
                //incomes
                transactions.Add(new Transaction
                {
                    Id = id,
                    Category = Category.Salary.ToString(),
                    CreatedTime = DateTime.Now.AddMonths(-i),
                    ModifiedTime = DateTime.Now,
                    RepeatDay = 0,
                    Description = dummyText,
                    Value = rn.Next(400, 1000),
                    OwnerId = userId

                });
                id++;

                //expenses

                transactions.Add(new Transaction
                {
                    Id = id,
                    Category = Category.General.ToString(),
                    CreatedTime = DateTime.Now.AddMonths(-i),
                    ModifiedTime = DateTime.Now,
                    RepeatDay = 0,
                    Description = dummyText,
                    Value = rn.Next(100, 1000) * -1,
                    OwnerId = userId


                });
                id++;

                //MonthlyRepetitions
                transactions.Add(new Transaction
                {
                    Id = id,
                    Category = Category.MonthlyRepetition.ToString(),
                    CreatedTime = DateTime.Now.AddMonths(-i),
                    ModifiedTime = DateTime.Now,
                    RepeatDay = 0,
                    Description = dummyText,
                    Value = -55,
                    OwnerId = userId

                });
                id++;
            }
            
            //repetition itself
            transactions.Add(new Transaction
            {
                Id = id,
                Category = Category.Health.ToString(),
                CreatedTime = DateTime.Now.AddMonths(4),
                ModifiedTime = DateTime.Now,
                RepeatDay = 14,
                Description = dummyText,
                Value = -55,
                OwnerId = userId

            });

            modelBuilder.Entity<User>().HasData(users);
            modelBuilder.Entity<Transaction>().HasData(transactions);
        }
    }
}
