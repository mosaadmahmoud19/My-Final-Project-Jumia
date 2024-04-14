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
    public class WishListController : ControllerBase
    {
        private readonly IwishListRepo wlistRepo;
        private readonly IProducsRepo productRepo;
        public WishListController(IwishListRepo wlistRepo, IProducsRepo productRepo)
        {
            this.wlistRepo = wlistRepo;
            this.productRepo = productRepo;
        }
        [HttpGet("{Userid}")]
        public ActionResult<WishListDTO> getWishList(string Userid)
        {
            WishList wish = wlistRepo.getwishid(Userid);
            if (wish == null)
            {
                return NotFound("WishList not found");
            }
            var wishlistitems = wlistRepo.GetWishListItems(wish.WishListId);
            List<WishListItemDTO> wishlistitemsDTOs = new List<WishListItemDTO>();
            foreach (var item in wishlistitems)
            {
                wishlistitemsDTOs.Add(new WishListItemDTO
                {
                    ProductID = item.Products.ProductID,
                    Name = item.Products.Name,
                    Img = item.Products.Images[0],
                    Quantity = item.Quantity,
                    UnitPrice = item.Products.Price
                });
            }
            WishListDTO WishListDTO = new WishListDTO()
            {
                Id = wish.WishListId,
                WishListItem = wishlistitemsDTOs,
                UserID = wish.UserID,
            };
            return Ok(WishListDTO);
        }
        [HttpPost]
        public ActionResult addToWishList([FromBody] AddToWishListDTO addToWishListDTO)
        {
            try
            {
                var wish = wlistRepo.getwishid(addToWishListDTO.UserID);
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var product = productRepo.GetByID(addToWishListDTO.ProductId);
                if (product == null)
                {
                    return NotFound();
                }
                if (wish.WishListitems != null && wish.WishListitems.Any(s => s.ProductID == addToWishListDTO.ProductId))
                {
                    return Conflict("The product is already in the wishlist.");
                }
                wlistRepo.AddToWishList(new WishListItems
                {
                    ProductID = product.ProductID,
                    Name = product.Name,
                    Img = product.Images[0],
                    Quantity = product.Stock,
                    UnitPrice = product.Price,
                    WishlistID = wish.WishListId
                });
                return Created("Product added to WishList successfully", addToWishListDTO.ProductId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        [HttpDelete]
        public ActionResult deleteFromCart(string Userid, int productid)
        {
            var wish = wlistRepo.getwishid(Userid);
            WishListItems wishList = wlistRepo.GetWishListItemsById(wish.WishListId, productid);
            if (wishList == null)
            {
                return NotFound();
            }
            wlistRepo.deleteFromWishlist(wishList);
            return Ok(new { done = "Deleted success" });
        }
    }
}
