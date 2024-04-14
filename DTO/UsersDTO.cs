namespace My_Final_Project.DTO
{
    public class UsersDTO
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Name { get; set; }
        public string? Role { get; set; }
        public int Age { get; set; }
        public string? Nationality { get; set; }
        public string? Email { get; set; }
        public virtual bool EmailConfirmed { get; set; }
        public virtual string? PhoneNumber { get; set; }

    }
}