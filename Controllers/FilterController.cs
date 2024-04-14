using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using My_Final_Project.DTO;
using My_Final_Project.Models;
using My_Final_Project.Reposatory;
using My_Final_Project.Reposatory.InterfacesReposatory;

namespace My_Final_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilterController : ControllerBase
    {
        private readonly IFilterRepo filterRepo;

        public FilterController(IFilterRepo filterRepo)
        {
            this.filterRepo = filterRepo;
        }
        [HttpGet("/api/FBN/{name:alpha}")]
        public ActionResult<List<ProductsDTO3>> filterByName(string name)
        {
            List<Products> products = filterRepo.FilterBYName(name);
            if (products == null)
            {
                return NotFound();
            }
            List<ProductsDTO3> productsDTOs = new List<ProductsDTO3>();
            foreach (var item in products)
            {
                ProductsDTO3 productsDTO = new ProductsDTO3()
                {
                    Id = item.ProductID,
                    Name = item.Name,
                    Price = item.Price,
                    Img = item.Images[0],
                    Stock = item.Stock,
                    OnSale = item.OnSale,
                    Description = item.Description,
                    CategoryName = item.category.CategoryName,
                };
                productsDTOs.Add(productsDTO);
            }
            return Ok(productsDTOs);
        }
        [HttpGet("/api/FBC/{name:alpha}")]
        public ActionResult<List<ProductsDTO3>> filterByCategory(string name)
        {
            Category category = filterRepo.FilterBYCategory(name);
            List<Products> products = category.Products;
            if (category == null || products == null)
            {
                return NotFound();
            }
            List<ProductsDTO3> productsDTOs = new List<ProductsDTO3>();
            foreach (var item in products)
            {
                if (item.category.CategoryName == category.CategoryName)
                {
                    ProductsDTO3 productsDTO = new ProductsDTO3()
                    {
                        Id = item.ProductID,
                        Name = item.Name,
                        Price = item.Price,
                        Img = item.Images[0],
                        Stock = item.Stock,
                        OnSale = item.OnSale,
                        Description = item.Description,
                        CategoryName = item.category.CategoryName,
                    };
                    productsDTOs.Add(productsDTO);
                }
                else
                {
                    return NotFound();
                }
            }
            return Ok(productsDTOs);
        }
        [HttpGet("FBMaxPrice")]
        public ActionResult<List<ProductsDTO3>> filterByMaxPrice()
        {
            List<Products> products = filterRepo.FilterByPrice();
            if (products == null || !products.Any())
            {
                return NotFound("No products found within the specified price range.");
            }
            List<Products> maxPriceProduct = products.OrderByDescending(p => p.Price).ToList();
            List<ProductsDTO3> productsDTOs = new List<ProductsDTO3>();
            foreach (var item in maxPriceProduct)
            {
                ProductsDTO3 productsDTO = new ProductsDTO3()
                {
                    Id = item.ProductID,
                    Name = item.Name,
                    Price = item.Price,
                    Img = item.Images[0],
                    Stock = item.Stock,
                    OnSale = item.OnSale,
                    Description = item.Description,
                    CategoryName = item.category.CategoryName,
                };
                productsDTOs.Add(productsDTO);
            }
            return Ok(productsDTOs);
        }
        [HttpGet("FBMinPrice")]
        public ActionResult<List<ProductsDTO3>> filterByMinPrice()
        {
            List<Products> products = filterRepo.FilterByPrice();
            if (products == null || !products.Any())
            {
                return NotFound("No products found within the specified price range.");
            }
            List<Products> minPriceProduct = products.OrderBy(p => p.Price).ToList();
            List<ProductsDTO3> productsDTOs = new List<ProductsDTO3>();
            foreach (var item in minPriceProduct)
            {
                ProductsDTO3 productsDTO = new ProductsDTO3()
                {
                    Id = item.ProductID,
                    Name = item.Name,
                    Price = item.Price,
                    Img = item.Images[0],
                    Stock = item.Stock,
                    OnSale = item.OnSale,
                    Description = item.Description,
                    CategoryName = item.category.CategoryName,
                };
                productsDTOs.Add(productsDTO);
            }
            return Ok(productsDTOs);
        }

        [HttpGet("/api/FBOS/{OnSale:bool}")]
        public ActionResult<List<ProductsDTO3>> filterByOnSale(bool OnSale)
        {
            List<Products> products = filterRepo.FilterByOnSale(OnSale);
            if (products == null)
            {
                return NotFound();
            }
            List<ProductsDTO3> productsDTOs = new List<ProductsDTO3>();
            foreach (var item in products)
            {
                ProductsDTO3 productsDTO = new ProductsDTO3()
                {
                    Id = item.ProductID,
                    Name = item.Name,
                    Price = item.Price,
                    Img = item.Images[0],
                    Stock = item.Stock,
                    OnSale = item.OnSale,
                    Description = item.Description,
                    CategoryName = item.category.CategoryName,
                };
                productsDTOs.Add(productsDTO);
            }
            return Ok(productsDTOs);
        }
    }
}
