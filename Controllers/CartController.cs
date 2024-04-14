using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using My_Final_Project.DTO;
    using My_Final_Project.Models;
    using My_Final_Project.Reposatory;
    using My_Final_Project.Reposatory.Interfaces;

    namespace My_Final_Project.Controllers
    {
       
        [Route("api/[controller]")]
        [ApiController]
        public class CartController : ControllerBase
        {
            private readonly ICartRepo cartRepo;
            private readonly IProducsRepo productRepo;
            public CartController(ICartRepo cartRepo, IProducsRepo productRepo)
            {
                this.cartRepo = cartRepo;
                this.productRepo = productRepo;
            }
            [HttpGet("{Userid}")]
            public ActionResult<CartDTO> getCart(string Userid)
            {
                Carts carts = cartRepo.GetCartByUserId(Userid);
                if (carts == null)
                {
                    return NotFound("Cart not found");
                }
            var cartItems = cartRepo.GetCartItems(carts.CartID);
                List<CartItemsDTO> cartItemsDTOs = new List<CartItemsDTO>();
                int totalCartPrice = 0;
                int totalQuantity = 0;
                foreach (var item in cartItems)
                {
                    cartItemsDTOs.Add(new CartItemsDTO
                    {
                        ProductId = item.Product.ProductID,
                        Name = item.Product.Name,
                        Img = item.Product.Images[0],
                        Quantity = item.Quantity,
                        UnitPrice = item.Product.Price,
                        //Size = item.Size,
                        //Color = item.Color,
                    });
                    totalCartPrice += item.Product.Price * item.Quantity;
                    totalQuantity  += item.Quantity;
                }
                CartDTO cartDTO = new CartDTO()
                {
                    Id = carts.CartID,
                    TotalPrice = totalCartPrice,
                    TotalQuantity = totalQuantity,
                    CartItems = cartItemsDTOs,
                    CartItemCount = cartItems.Count,
                    UserID = carts.UserID,
                };
                return Ok(cartDTO);
            }
        [HttpPost]
        public ActionResult addToCart([FromBody] AddToCartDTO addToCartDTO)
        {
            var cart = cartRepo.GetCartByUserId(addToCartDTO.UserID);
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var product = productRepo.GetByID(addToCartDTO.ProductId);
            if (product == null)
            {
                return NotFound();
            }
            if (product.Stock == 0)
            {
                return BadRequest("Stock Empty");
            }
            if (cartRepo.AddORUpdateCart(addToCartDTO.ProductId, cart.CartID) == true) ;
            else
            {
                cartRepo.AddToCart(new CartItems
                {
                    ProductID = product.ProductID,
                    Name = product.Name,
                    Img = product.Images[0],
                    Quantity = +1,
                    UnitPrice = product.Price,
                    CartID = cart.CartID,
                    //Size = addToCartDTO.Size,
                    //Color = addToCartDTO.Color,
                });
            }
            product.Stock--;
            cartRepo.SaveChanges();
            return Created("Product added to cart successfully", addToCartDTO.ProductId);
        }
        [HttpPut("{userId}")]
        public ActionResult UpdateCartItemQuantity(string userId, [FromBody] UpdateCartItemDTO updateCartItemDTO)
        {
            var cart = cartRepo.GetCartByUserId(userId);
            if (cart == null)
            {
                return NotFound("Cart not found");
            }

            var cartItem = cartRepo.GetCartItemsById(cart.CartID, updateCartItemDTO.ProductId);
            if (cartItem == null)
            {
                return NotFound("Cart item not found");
            }
            var product = productRepo.GetByID(updateCartItemDTO.ProductId);
            if (product == null)
            {
                return NotFound("Product not found");
            }
            if (updateCartItemDTO.Quantity > product.Stock + cartItem.Quantity)
            {
                return BadRequest("Insufficient stock");
            }
            var quantityDifference = cartItem.Quantity - updateCartItemDTO.Quantity;
            product.Stock += quantityDifference;
            productRepo.SaveChanges();
            cartItem.Quantity = updateCartItemDTO.Quantity;
            cartRepo.SaveChanges();
            return Content("Quantity updated successfully");
        }
        [HttpDelete("DeleteOneCartItem")]
            public ActionResult deleteFromCart(string Userid, int productid )
            {
                var cart = cartRepo.GetCartByUserId(Userid);
                CartItems cartItem = cartRepo.GetCartItemsById(cart.CartID,productid);   
                if (cartItem == null)
                {
                    return NotFound();
                }
                if (cartItem.Quantity > 0)
                {
                    cartRepo.ReduceQuantity(productid, cart.CartID);
                }
                else 
                {
                    cartRepo.DeleteFromCart(cartItem);
                }
                cartRepo.DeleteIfQuantityZero(cart.CartID);

                cartRepo.deletecart(cartItem.UnitPrice, cartItem.Quantity, cart.CartID);
                return Ok(new {done ="Deleted Success"});
            }
             [HttpDelete("DeleteAllCartItems")]
             public ActionResult deleteFromCart(string Userid)
             {
                cartRepo.DeleteAllCartitems(Userid);
                return Ok(new { done = "Deleted Success" });
             }
        }
    }
