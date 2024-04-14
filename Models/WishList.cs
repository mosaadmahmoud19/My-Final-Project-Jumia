using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace My_Final_Project.Models
{
    public class WishList
    {
        [Key]
        public int WishListId { get; set; }
        [ForeignKey("ApplicationUser")]
        public string UserID { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public virtual List<WishListItems> WishListitems { get; set; }
    }
}
