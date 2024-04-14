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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepo categoryRepo;
        public CategoryController(ICategoryRepo categoryRepo)
        {
            this.categoryRepo = categoryRepo;
        }
        [HttpGet]
        public ActionResult<List<Category>> getCategories() 
        {
            List<CategoryDTO> Categories = new List<CategoryDTO>();
            foreach (var item in categoryRepo.GetCategories())
            {
                CategoryDTO Category = new CategoryDTO()
                {
                    Id=item.CategoryId,
                    CategoryName = item.CategoryName,
                };
                Categories.Add(Category);
            }
            return Ok(Categories);
        }
       
        [HttpPost]
        public ActionResult addcategory(CategoryDTO categoryDTO)
        {
            if (!ModelState.IsValid) return BadRequest();
            var category = new Category
            {
                CategoryName=categoryDTO.CategoryName,
            };
            categoryRepo.add(category);
            return Created("Category added successfully", categoryDTO);
        }
     
        [HttpDelete]
        public ActionResult deletecategory(int id)
        {
            var existingProduct =categoryRepo.GetByID(id);
            if (existingProduct == null)
            {
                return NotFound();
            }
            categoryRepo.Delete(id);
            return Ok();
        }
    }
}
