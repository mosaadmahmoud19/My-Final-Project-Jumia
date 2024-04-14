using Microsoft.EntityFrameworkCore;
using My_Final_Project.Models;
using My_Final_Project.Reposatory.Interfaces;

namespace My_Final_Project.Reposatory
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly ITIStore iTIStore;

        public CategoryRepo(ITIStore iTIStore)
        {
            this.iTIStore = iTIStore;
        }
        public List<Category> GetCategories()
        {
            return iTIStore.Categories.ToList();
        }
        public Category GetByID(int id)
        {
            return iTIStore.Categories.SingleOrDefault(p => p.CategoryId == id);
        }
        public void add(Category category)
        {
            iTIStore.Add(category);
            iTIStore.SaveChanges();
        }
        public void Delete(int id)
        {
            iTIStore.Categories.Remove(GetByID(id));
            iTIStore.SaveChanges();
        }
    }
}
