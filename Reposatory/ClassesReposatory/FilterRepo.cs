using Microsoft.EntityFrameworkCore;
using My_Final_Project.Models;
using My_Final_Project.Reposatory.InterfacesReposatory;
using System.Linq;

namespace My_Final_Project.Reposatory.ClassesReposatory
{
    public class FilterRepo:IFilterRepo
    {
        private readonly ITIStore iTIStore;

        public FilterRepo(ITIStore iTIStore)
        {
            this.iTIStore = iTIStore;
        }
        public List<Products> FilterBYName(string name)
        {
            return iTIStore.Products.Include(s => s.category).Where(p => p.Name.Contains(name)).ToList();
        }
        public Category FilterBYCategory(string name)
        {
            return iTIStore.Categories.Include(s => s.Products).FirstOrDefault(p => p.CategoryName.Contains(name));
        }
        public List<Products> FilterByPrice()
        {
            return iTIStore.Products.Include(s => s.category).ToList();
        }
        public List<Products> FilterByOnSale(bool onsale)
        {
            return iTIStore.Products.Include(s => s.category).Where(p => p.OnSale == onsale).ToList();
        }
    }
}
