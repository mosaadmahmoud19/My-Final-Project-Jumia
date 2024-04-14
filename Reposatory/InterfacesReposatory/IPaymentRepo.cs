using My_Final_Project.Models;

namespace My_Final_Project.Reposatory.InterfacesReposatory
{
    public interface IPaymentRepo
    {
        public List<Payment> getAll();
        public void Create(Payment payment);
    }
}
