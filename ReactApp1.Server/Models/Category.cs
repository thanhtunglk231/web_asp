using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp1.Server.Models
{
    public class Category
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "NUMBER(1)")]
        public bool IsActive { get; set; } = true;

        public ICollection<Product> Products { get; set; }
    }

}
