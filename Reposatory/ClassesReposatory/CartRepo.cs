using Microsoft.EntityFrameworkCore;
using My_Final_Project.Models;
using My_Final_Project.Reposatory.Interfaces;

namespace My_Final_Project.Reposatory
{
    public class CartRepo : ICartRepo
    {
        private readonly ITIStore iTiStore;

        public CartRepo(ITIStore iTiStore)
        {
            this.iTiStore = iTiStore;
        }
        public List<CartItems> GetCartItems(int cartid)
        {
            return iTiStore.CartItems.Include(c=>c.Product).Where(c=>c.CartID==cartid).ToList();
        }
        public CartItems GetCartItemsById(int cartid , int productid)
        {
            return iTiStore.CartItems.SingleOrDefault(c => c.CartID == cartid && c.ProductID == productid);
        }
        public void AddToCart(CartItems cartItems)
        {
            iTiStore.CartItems.Add(cartItems);
            iTiStore.SaveChanges();
        }
        public void CreateCart(Carts carts)
        {
            iTiStore.Carts.Add(carts);
            iTiStore.SaveChanges();
        }
        public Carts GetCart(int id)
        {
            return iTiStore.Carts.SingleOrDefault(s => s.CartID == id);
        }
        public Carts GetCartByUserId(string userid)
        {
            return iTiStore.Carts.SingleOrDefault(c => c.UserID == userid);
        }
        public void SaveChanges()
        {
            iTiStore.SaveChanges();
        }
        public bool AddORUpdateCart(int productid, int cartid)
        {
            var existingCartItem = iTiStore.CartItems.SingleOrDefault(s => s.CartID == cartid && s.ProductID == productid);
            if (existingCartItem != null)
            {
                existingCartItem.Quantity += 1;
                iTiStore.SaveChanges();
                return true;
            }
            else return false;
        }
        public void DeleteFromCart(CartItems cartItem)
        {
            iTiStore.CartItems.Remove(cartItem);
            iTiStore.SaveChanges();
        }
        public void ReduceQuantity(int productid, int cartid)
        {
            var existingCartItem = iTiStore.CartItems.Include(p=>p.Product).SingleOrDefault(s => s.CartID == cartid && s.ProductID == productid);
            if (existingCartItem != null )
            {
                existingCartItem.Quantity--;
                existingCartItem.Product.Stock++;
                iTiStore.SaveChanges();
            }
        }
        public void DeleteIfQuantityZero(int cartId)
        {
            var cart = iTiStore.Carts.Include(c => c.CartItems).FirstOrDefault(c => c.CartID == cartId);

            if (cart != null)
            {
                var itemsToRemove = cart.CartItems.Where(ci => ci.Quantity <= 0).ToList();

                if (itemsToRemove.Any())
                {
                    iTiStore.CartItems.RemoveRange(itemsToRemove);
                    iTiStore.SaveChanges();
                }
            }
        }
        public void deletecart(int newprice, int newquantity, int cartid)
        {
            Carts cart = iTiStore.Carts.SingleOrDefault(t => t.CartID == cartid);
            if (cart != null)
            {
                cart.TotalPrice = cart.TotalPrice - (newprice * newquantity);
                cart.TotalQuantity = cart.TotalQuantity - newquantity;
                iTiStore.SaveChanges(); 
            }
        }
        public void DeleteAllCartitems(string userid)
        {
            var cart = iTiStore.Carts.Include(c => c.CartItems).SingleOrDefault(s => s.UserID == userid);
            foreach (var item in cart.CartItems)
            {
                var product = iTiStore.Products.SingleOrDefault(p => p.ProductID == item.ProductID);
                if (product != null)
                {
                    product.Stock += item.Quantity;
                }
                iTiStore.CartItems.Remove(item);
            }
            iTiStore.SaveChanges();
        }
    }
}
