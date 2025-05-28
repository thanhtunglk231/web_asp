using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ReactApp1.Server.Models
{
    public class CartItem
    {
        public int CartItemID { get; set; }

        public string? UserID { get; set; }  // Cho phép null nếu bạn gán trong controller

        public int ProductID { get; set; }

        public int? OptionID { get; set; }

        public int Quantity { get; set; } = 1;

        [JsonIgnore]
        public ApplicationUser? User { get; set; }

        [JsonIgnore]
        public Product? Product { get; set; }

        [JsonIgnore]
        public ProductOption? Option { get; set; }
    }
}
