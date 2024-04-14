using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace My_Final_Project.Models
{
    public class ITIStore:IdentityDbContext<ApplicationUser>
    {
        public ITIStore() { }
        public ITIStore (DbContextOptions options):base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>().ToTable("Users");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UsersClaims");
        
            modelBuilder.Entity<CartItems>().HasKey(x => new { x.ProductID, x.CartID });
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<WishListItems>().HasKey(x => new { x.ProductID, x.WishlistID });
            base.OnModelCreating(modelBuilder);
        }
        public virtual DbSet<Products>Products { get; set; }
        public virtual DbSet<Carts> Carts { get; set; } 

        public virtual DbSet<CartItems> CartItems { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<WishList> WishLists { get; set; }
        public virtual DbSet<Order>  Orders { get; set; }
        public virtual DbSet<OrderItems>  OrderItems { get; set; }
        public virtual DbSet<Payment>  Payments { get; set; }
        public virtual DbSet<WishListItems>  WishListItems { get; set; }
        public virtual DbSet<Review>  Reviews { get; set; }
    }
}
