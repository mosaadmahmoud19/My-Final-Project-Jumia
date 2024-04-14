using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace My_Final_Project.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [ForeignKey("ApplicationUser")]
        public string UserID { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public int TotalAmount { get; set; }
        public List<OrderItems> OrderItems { get; set; }
        public Payment Payment { get; set; }
       
        public ApplicationUser ApplicationUser { get; set; }
    }
}
