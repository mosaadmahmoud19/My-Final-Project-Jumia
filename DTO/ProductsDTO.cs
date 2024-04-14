using My_Final_Project.Models;

namespace My_Final_Project.DTO
{
    public class ProductsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
        public bool OnSale { get; set; }
        public int CategoryID { get; set; }
        //public List<string> Colors { get; set; } = new List<string>();
        //public List<string> Sizes { get; set; } = new List<string>();

    }
}
