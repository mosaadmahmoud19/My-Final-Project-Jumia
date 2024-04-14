using My_Final_Project.Models;

namespace My_Final_Project.Reposatory
{
    public interface ICartRepo 
    {
        public List<CartItems> GetCartItems(int cartid);
        public CartItems GetCartItemsById(int cartid, int productid);
        public void AddToCart(CartItems cartItems);
        public void DeleteFromCart(CartItems cartItem);
        public Carts GetCart(int id);
        public void deletecart(int newprice, int newquantity, int cartid);
        public bool AddORUpdateCart(int productid, int cartid);
        public void CreateCart (Carts carts);
        public Carts GetCartByUserId(string userid);
        public void ReduceQuantity(int productid, int cartid);
        public void DeleteIfQuantityZero(int cartId);
        public void DeleteAllCartitems(string userid);
        public void SaveChanges();
    }
}
