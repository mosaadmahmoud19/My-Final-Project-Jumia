using My_Final_Project.Models;

namespace My_Final_Project.Reposatory
{
    public interface IwishListRepo
    {
        public List<WishListItems> GetWishListItems(int wishid);
        public WishListItems GetWishListItemsById(int cartid, int productid);
        public void AddToWishList(WishListItems wishListItems);
        public WishList getwishid(string userid);
        public void deleteFromWishlist(WishListItems wishListItems);
        public void CreateWishList(WishList wishList);
    }
}
