using My_Final_Project.Models;
using Microsoft.EntityFrameworkCore;
using My_Final_Project.Reposatory.Interfaces;

namespace My_Final_Project.Reposatory
{
    public class ProductRepo : IProducsRepo
    {
        private readonly ITIStore db;

        public ProductRepo(ITIStore db ){
            this.db = db;
        }
        public List<Products> GetAll()
        {
            return db.Products.Include(p=>p.category).ToList();
        }
        public Products GetByID(int id)
        {
            return db.Products.Include(p => p.category).SingleOrDefault(p=>p.ProductID==id);
        }
        public Products GetByname(string name)
        {
            return db.Products.Include(p => p.category).FirstOrDefault(p => p.Name == name);
        }
        public void Add(Products product)
        {
            db.Products.Add(product);
            db.SaveChanges();
        }
        public void Update(Products product)
        {
            db.Entry(product).State =EntityState.Modified;
            db.SaveChanges();

        }
        public void Delete(int id)
        {
            db.Products.Remove(GetByID(id));
            db.SaveChanges();
        }
        public void SaveChanges()
        {
            db.SaveChanges();
        }
    }
}
