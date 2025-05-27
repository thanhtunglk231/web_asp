namespace ReactApp1.Server.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public string UserID { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string DeliveryMethod { get; set; }
        public string Status { get; set; } = "Chờ xác nhận";
        public string ShippingAddress { get; set; }

        public ApplicationUser User { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public ICollection<Delivery> Deliveries { get; set; }
    }

}
