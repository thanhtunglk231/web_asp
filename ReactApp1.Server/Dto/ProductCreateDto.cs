namespace ReactApp1.Server.Dto
{
    public class ProductCreateDto
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int CategoryID { get; set; }
        public bool IsVegetarian { get; set; } = false;
        public bool IsBestseller { get; set; } = false;
    }

}
