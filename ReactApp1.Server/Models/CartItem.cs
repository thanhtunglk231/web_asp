using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Build.Framework;

namespace ReactApp1.Server.Models
{
    public class CartItem
    {
        public int CartItemID { get; set; }

        [Required]
        public string UserID { get; set; }  // FK to AspNetUsers

        public int ProductID { get; set; }

        public int? OptionID { get; set; }

        public int Quantity { get; set; } = 1;

        [JsonIgnore]
        public ApplicationUser User { get; set; }

        [JsonIgnore]
        public Product? Product { get; set; }

        [JsonIgnore]
        public ProductOption? Option { get; set; }
    }

}
