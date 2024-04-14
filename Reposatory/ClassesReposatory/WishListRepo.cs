using Microsoft.EntityFrameworkCore;
using My_Final_Project.Models;
using My_Final_Project.Reposatory.Interfaces;

namespace My_Final_Project.Reposatory
{
    public class WishListRepo : IwishListRepo
    {
        private readonly ITIStore iTiStore;

        public WishListRepo(ITIStore iTiStore)
        {
            this.iTiStore = iTiStore;
        }
        public List<WishListItems> GetWishListItems(int wishid)
        {
            return iTiStore.WishListItems.Include(c => c.Products).Where(c => c.WishlistID == wishid).ToList();
        }
        public WishListItems GetWishListItemsById(int cartid, int productid)
        {
            return iTiStore.WishListItems.SingleOrDefault(c => c.WishlistID == cartid && c.ProductID == productid);
        }
        public WishList getwishid(string userid)
        {
            return iTiStore.WishLists.SingleOrDefault(c => c.UserID == userid);
        }
        public void AddToWishList(WishListItems wishListItems)
        {
            iTiStore.WishListItems.Add(wishListItems);
            iTiStore.SaveChanges();
        }
        public void CreateWishList(WishList wishList)
        {
            iTiStore.WishLists.Add(wishList);
            iTiStore.SaveChanges();
        }
        public void deleteFromWishlist(WishListItems wishListItems)
        {
            iTiStore.WishListItems.Remove(wishListItems);
            iTiStore.SaveChanges();
        }
    }
}
