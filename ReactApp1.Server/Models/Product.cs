using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp1.Server.Models
{
    [Table("Products")]
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int CategoryID { get; set; }
        [Column(TypeName = "NUMBER(1)")]
        public bool IsVegetarian { get; set; } = false;
        [Column(TypeName = "NUMBER(1)")]
        public bool IsBestseller { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Category Category { get; set; }
        public ICollection<ProductOption> ProductOptions { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }

}
