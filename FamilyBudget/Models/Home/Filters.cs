using Castle.Components.DictionaryAdapter;
using System.Drawing.Text;

namespace FamilyBudget.Models.Home
{
    public class Filters
    {
        public static string NoFilter = "All";
        private string[]? filters;
        public Filters(string filterstring)
        {
            if (filterstring == null) SetFilterString(new string[] { NoFilter, NoFilter, NoFilter});
            else FilterString = filterstring;
            filters = FilterString?.Split('-');
            //1
            Category = filters[0];
            //2
            Type = filters[1];
            //3
            MonthYearString = filters[2];
            if (filters[2] != NoFilter)
            {
                string[] monthsYears = filters[2].Split(".");
                int month = int.Parse(monthsYears[0]);
                int year = int.Parse(monthsYears[1]);
                MonthYear = new DateTime(year, month, DateTime.MinValue.Day);
            }
        }

        public string FilterString { get; set; }
        public string Category { get; }
        public string Type { get; }
        public string MonthYearString { get; }
        public DateTime MonthYear { get; }
      
        public bool HasCategory => Category.ToLower() != NoFilter.ToLower();
        public bool HasType => Type.ToLower() != NoFilter.ToLower();
        public bool HasMonthYear => MonthYearString.ToLower() != NoFilter.ToLower();
        //active filters
        public List<string> ActiveFilters { get
        {
            return filters.Where(f => !f.Equals(NoFilter)).ToList();
        } }
        //returns FilterString after changing the filter
        public string RemoveFilter(string Filter)
        {
            ActiveFilters.Remove(Filter);
            int indexOfFilter = filters.ToList().IndexOf(Filter);
            filters[indexOfFilter] = NoFilter;
            SetFilterString(filters); 
            return FilterString;
        }

        //sets filterstring 
        private void SetFilterString(string[] fs) {
            string fstring = "";
            foreach(string f in fs)
            {
                fstring += "-" + f;
            }

            this.FilterString = fstring.Remove(0, 1);
        }
    }
}

