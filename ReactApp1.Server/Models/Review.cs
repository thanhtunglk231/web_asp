namespace ReactApp1.Server.Models
{
    public class Review
    {
        public int ReviewID { get; set; }
        public string UserID { get; set; }
        public int ProductID { get; set; }
        public int Rating { get; set; }  // 1 - 5
        public string ReviewComment { get; set; }
        public DateTime ReviewDate { get; set; } = DateTime.Now;

        public ApplicationUser User { get; set; }
        public Product Product { get; set; }
    }

}
