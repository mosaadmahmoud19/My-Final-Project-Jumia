using Microsoft.EntityFrameworkCore;
using My_Final_Project.Models;

namespace FinalProject.Services
{
    public class ReviewRepository : IReviewRepository
    {
        ITIStore context;

        public ReviewRepository(ITIStore _context)
        {
            context = _context;
        }

        public void Create(Review review)
        {
            context.Reviews.Add(review);
            context.SaveChanges();
        }
        public List<Review> GetAll()
        {
            return context.Reviews.ToList();
        }

        public Review GetByID(int id)
        {
            return context.Reviews.FirstOrDefault(r => r.ReviewID == id);
        }

        public void Update(Review review)
        {
            context.Entry(review).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            context.Reviews.Remove(GetByID(id));
            context.SaveChanges();
        }
    }
}
