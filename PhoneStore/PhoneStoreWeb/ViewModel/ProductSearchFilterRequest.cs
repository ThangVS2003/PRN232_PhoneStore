namespace PhoneStoreWeb.ViewModel
{
    public class ProductSearchFilterRequest
    {
        public string? keyword { get; set; }
        public bool? sortByPrice { get; set; } // true for ascending, false for descending
        public decimal? minPrice { get; set; }
        public decimal? maxPrice { get; set; }
    }
}
