using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp1.Server.Models
{
    public class Promotion
    {
        public int PromotionID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int DiscountPercent { get; set; }  // 0-100
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [Column(TypeName = "NUMBER(1)")]
        public bool IsActive { get; set; } = true;
    }

}
