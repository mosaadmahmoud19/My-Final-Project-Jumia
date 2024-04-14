namespace My_Final_Project.DTO
{
    public class ProductsDTO2
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
        public bool OnSale { get; set; }
        public string CategoryName { get; set; }
        public List<string> images { get; set; } = new List<string>();
        //public List<string> Colors { get; set; } = new List<string>();
        //public List<string> Sizes { get; set; } = new List<string>();
    }
}
