using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace My_Final_Project.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public int Age { get; set; }
        public string Phone { get; set; }
        public string Nationality { get; set; }
        public List<Order> orders { get; set; }= new List<Order>();
        public Carts Carts { get; set; }
        public WishList WishList { get; set; }
        public List<Review> reviews { get; set; }=new List<Review>();
    }
}