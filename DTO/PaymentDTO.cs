using System.ComponentModel.DataAnnotations;

namespace My_Final_Project.DTO
{
    public class PaymentDTO
    {

        public DateTime TransactionDate { get; set; }


        public string PaymentMethode { get; set; }

        public int OrderId { get; set; }
        [Required]
        public long Cardnumber { get; set; }
      
        [Required]
        public int CVV { get; set; }
        public int ZipCode { get; set; }

        public DateTime ShipDate { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string ClientName { get; set; }

        public string PhoneNumber { get; set; }

    }
}
