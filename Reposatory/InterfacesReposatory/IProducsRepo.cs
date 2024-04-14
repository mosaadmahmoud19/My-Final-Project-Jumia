using My_Final_Project.Models;

namespace My_Final_Project.Reposatory.Interfaces
{
    public interface IProducsRepo
    {
        List<Products> GetAll();
        Products GetByID(int id);
        Products GetByname(string name);
        void Add(Products product);
        void Update(Products product);
        void Delete(int id);
        public void SaveChanges();
    }
}
