using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp1.Server.Models
{
    public class FAQ
    {
        public int FAQID { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Category { get; set; }
        [Column(TypeName = "NUMBER(1)")]
        public bool IsActive { get; set; } = true;
    }

}
