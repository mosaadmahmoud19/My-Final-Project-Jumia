using My_Final_Project.Models;

namespace My_Final_Project.DTO
{
    public class WishListDTO
    {
        public int Id { get; set; }
        public string UserID { get; set; }
        public virtual List<WishListItemDTO> WishListItem { get; set; } = new List<WishListItemDTO>();
    }
}
