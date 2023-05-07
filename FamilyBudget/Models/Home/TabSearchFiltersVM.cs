namespace FamilyBudget.Models.Home
{
    public class TabSearchFiltersVM
    {
        public int Tab { get; set; }
        public string? Category { get; set; }
        public string? Type { get; set; }
        public DateTime MonthYear { get; set; }
        public string? Search { get; set; }
    }
}
