using My_Final_Project.Models;
using System.ComponentModel.DataAnnotations;

namespace My_Final_Project.DTO
{
    public class CartItemsDTO
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Img { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
 /*       public string Size { get; set; }
        public string Color { get; set; }*/

    }
}
