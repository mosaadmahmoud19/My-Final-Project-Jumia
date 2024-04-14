using My_Final_Project.Models;

namespace My_Final_Project.Services
{
    public interface IEmailService
    {
        void SendEmail(Message message);
        void SendEmailfromUser(ContactUsMessage message);
    }
}