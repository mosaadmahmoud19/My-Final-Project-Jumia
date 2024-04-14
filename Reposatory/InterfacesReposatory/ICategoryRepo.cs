using My_Final_Project.Models;

namespace My_Final_Project.Reposatory
{
    public interface ICategoryRepo
    {
        public List<Category> GetCategories();
        public Category GetByID(int id);
        public void add(Category category);
        public void Delete(int id);
    }

}
