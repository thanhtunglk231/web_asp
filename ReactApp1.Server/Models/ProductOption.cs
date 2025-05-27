namespace ReactApp1.Server.Models
{
    public class ProductOption
    {
        public int OptionID { get; set; }
        public int ProductID { get; set; }
        public string OptionName { get; set; }
        public decimal AdditionalPrice { get; set; } = 0;

        public Product Product { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }

}
