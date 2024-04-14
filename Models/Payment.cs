using System.ComponentModel.DataAnnotations;

namespace My_Final_Project.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]

        public DateTime TransactionDate { get; set; }

        [Required]
        public string PaymentMethode { get; set; }


        [Required]
        public string Email { get; set; }
        [Required]
        public string ClientName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public long Cardnumber { get; set; }
      
        [Required]
        public int CVV { get; set; }

        [Required]
        public int ZipCode { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ShipDate { get; set; }

        [Required]
        public string Address { get; set; }
        public int OrderId { get; set; }
        public Order order { get; set; } 
    }
}
