using Microsoft.EntityFrameworkCore;
using My_Final_Project.Models;
using My_Final_Project.Reposatory.InterfacesReposatory;

namespace FinalProject.Services
{
    public class PaymentRepository : IPaymentRepo
    {
        private readonly ITIStore iTIStore;

        public PaymentRepository(ITIStore iTIStore)
        {
            this.iTIStore = iTIStore;
        }
        public List<Payment> getAll()
        {
            return iTIStore.Payments.ToList();
        }
        public void Create(Payment payment)
        {
            iTIStore.Payments.Add(payment);
            iTIStore.SaveChanges();
        }
    }
}
