using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using My_Final_Project.DTO;
using My_Final_Project.Models;
using My_Final_Project.Reposatory.Interfaces;
using My_Final_Project.Reposatory.InterfacesReposatory;

namespace My_Final_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProducsRepo productsRepo;
        private readonly ImagesController imagesServices;

        public ProductsController(IProducsRepo productsRepo, ImagesController imagesServices)
        {
            this.productsRepo = productsRepo;
            this.imagesServices = imagesServices;
        }
        [HttpGet]
            
        public ActionResult<List<ProductsDTO2>> getAll()
        {
            List<ProductsDTO3> products = new List<ProductsDTO3>();
            foreach(var item in productsRepo.GetAll())
            {
                ProductsDTO3 product = new ProductsDTO3()
                {
                   Id=item.ProductID,
                   Name=item.Name,
                   Price=item.Price,
                   Img = item.Images[0],
                   Stock =item.Stock,
                   OnSale=item.OnSale,
                   Description=item.Description,   
                   CategoryName=item.category.CategoryName,
                };
                products.Add(product);  
            }    
            return Ok(products);
        }

        [HttpGet("{id:int}")]
       
        public ActionResult getById(int id)
        {   
            Products products = productsRepo.GetByID(id);
            if (products == null)
            {
                return NotFound();
            }
            ProductsDTO2 productsDTO = new ProductsDTO2()
            {
                Id = products.ProductID,
                Name = products.Name,
                Price = products.Price,
                images = products.Images,
                Stock =products.Stock,
                OnSale=products.OnSale,
                Description=products.Description,
                CategoryName = products.category.CategoryName,
                //Sizes = products.Sizes,
                //Colors = products.Colors,
            };
            return Ok(productsDTO);
        }
        [HttpGet("{name:alpha}")]
        public ActionResult getByName(string name)
        {
            Products products = productsRepo.GetByname(name);
            if (products == null)
            {
                return NotFound();
            }
            ProductsDTO2 productsDTO = new ProductsDTO2()
            {
                Id = products.ProductID,
                Name = products.Name,
                Price = products.Price,
                images = products.Images,
                Stock = products.Stock,
                OnSale = products.OnSale,
                Description = products.Description,
                CategoryName = products.category.CategoryName,
                //Sizes = products.Sizes,
                //Colors = products.Colors,
            };
            return Ok(productsDTO);
        }
       
        [HttpPost]
        public  ActionResult add([FromForm] ProductsDTO productDTO, List<IFormFile> images)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            List<string> imageUrls = new List<string>();
            foreach (var img in images)
            {
                string imgUrl = imagesServices.UploadPhoto(img);
                imageUrls.Add(imgUrl);
            }
            int price = productDTO.Price;
            if (productDTO.OnSale)
            {
                price = (int)(productDTO.Price * 0.7);
            }
            
            var product = new Products
            {
                Name = productDTO.Name,
                Description = productDTO.Description,
                Price = price,
                Images = imageUrls,
                OnSale = productDTO.OnSale,
                Stock = productDTO.Stock,
                CategoryId=productDTO.CategoryID,
                //Sizes = productDTO.Sizes,
                //Colors = productDTO.Colors,
            };
            productsRepo.Add(product);
            return CreatedAtAction("GetById", new { id = product.ProductID }, productDTO);
        }
       
        [HttpPut("{id}")]
        public ActionResult edit([FromForm] ProductsDTO productDTO, int id, List<IFormFile> images)
        {
            if (productDTO == null) return BadRequest(new { error = "productDTO == null" });
            if (productDTO.Id != id) return BadRequest(new {error= "productDTO.Id != id" });
            if (!ModelState.IsValid) return BadRequest(ModelState);
            List<string> imageUrls = new List<string>();
            foreach (var img in images)
            {
                string imgUrl = imagesServices.UploadPhoto(img);
                imageUrls.Add(imgUrl);
            }
            int price = productDTO.Price;
            if (productDTO.OnSale)
            {
                price = (int)(productDTO.Price * 0.7);
            }
            var product = new Products
            {
                ProductID = productDTO.Id,
                Name = productDTO.Name,
                Description = productDTO.Description,
                Price = price,
                Images = imageUrls,
                OnSale = productDTO.OnSale,
                Stock = productDTO.Stock,
                CategoryId = productDTO.CategoryID,
                //Sizes = productDTO.Sizes,
                //Colors = productDTO.Colors,
            };
            productsRepo.Update(product);
            return NoContent(); 
        }
       
        [HttpDelete]
        public ActionResult delete(int id)
        {
            var existingProduct = productsRepo.GetByID(id);
            if (existingProduct == null)
            {
                return NotFound();
            }
            productsRepo.Delete(id);
            return Ok();
        }
    }
}
