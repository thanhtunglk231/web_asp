namespace ReactApp1.Server.Models
{
    public class Payment
    {
        public int PaymentID { get; set; }
        public int OrderID { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Now;
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }

        public Order Order { get; set; }
    }

}
