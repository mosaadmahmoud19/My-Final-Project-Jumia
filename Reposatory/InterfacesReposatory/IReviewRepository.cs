
using My_Final_Project.Models;

namespace FinalProject.Services
{
    public interface IReviewRepository
    {
        void Create(Review review);
        void Delete(int id);
        List<Review> GetAll();
        Review GetByID(int id);
        void Update(Review review);
    }
}