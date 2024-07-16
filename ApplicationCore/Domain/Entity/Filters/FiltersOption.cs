namespace ApplicationCore.Domain.Entity.Filters
{
    public enum Perpage
    {
        InPage5 = 5,
        InPage10 = 10,
        InPage25 = 25,
        InPage50 = 50
    }
    public class FiltersOption
    {

        public string? SearchBy { get; set; }
        public double? ToStartSearch { get; set; }
        public double? ToEndSearch { get; set; }
        public string? SortDirection { get; set; } = "ascending";
        public int CurrentPage { get; set; } = 1;
        public int perPage { get; set; } = (int)Perpage.InPage5;
    }
}