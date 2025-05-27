namespace ReactApp1.Server.Models
{
    public class Delivery
    {
        public int DeliveryID { get; set; }
        public int OrderID { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime? EstimatedTime { get; set; }
        public string DeliveryStatus { get; set; } = "Đang chuẩn bị";

        public Order Order { get; set; }
    }

}
