using FamilyBudget.Controllers;
using FluentScheduler;

namespace FamilyBudget
{
    //Task registry 
    public class ScheduleRegistry : Registry
    {
        public ScheduleRegistry()
        {
            // Schedule a simple job to run at a specific time -> runs everday at 12:00
            Schedule(() => new Job()).ToRunEvery(0).Days().At(12, 00);
        }
    }
    //Task Job
    public class Job : IJob
    {
        //Calls method from HomeController -> which checks for repeat day of transactions and if its today adds new Transaction
        public void Execute()   
        {
            HomeController hc = new HomeController();
            hc.CheckDailyForRepetitions();
        }
    }
}

