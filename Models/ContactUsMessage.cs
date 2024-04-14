using MimeKit;

namespace My_Final_Project.Models
{
    public class ContactUsMessage
    {
        public MailboxAddress From { get; set; }
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public ContactUsMessage(string from, IEnumerable<string> to, string subject, string content)
        {
            From = new MailboxAddress("User", from);
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress("Admin", x)));

            Subject = subject;
            Content = content;
        }
    }

}