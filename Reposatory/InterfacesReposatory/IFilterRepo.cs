using My_Final_Project.Models;

namespace My_Final_Project.Reposatory.InterfacesReposatory
{
    public interface IFilterRepo
    {
        public List<Products> FilterBYName(string name);
        public Category FilterBYCategory(string name);
        public List<Products> FilterByPrice();
        public List<Products> FilterByOnSale(bool onsale);
    }
}
