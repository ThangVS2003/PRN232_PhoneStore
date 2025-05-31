namespace PhoneStoreWeb.Models
{
    public class ProductViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? BrandID { get; set; }
        public string MainImage { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}