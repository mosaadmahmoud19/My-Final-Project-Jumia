using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using My_Final_Project.DTO;
using My_Final_Project.Models;
using My_Final_Project.Reposatory;
using My_Final_Project.Reposatory.InterfacesReposatory;
using My_Final_Project.Services;

namespace My_Final_Project.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepo paymentRepository;
        private readonly IEmailService emailService;
        private readonly IOrderRepo orderRepo;

        public PaymentController(IPaymentRepo _paymentRepository, IEmailService emailService,IOrderRepo orderRepo)
        {
            paymentRepository = _paymentRepository;
            this.emailService = emailService;
            this.orderRepo = orderRepo;
         
        }
        [HttpGet]
        public IActionResult getPayment()
        {
            List<PaymentDTO> paymentDTOs = new List<PaymentDTO>();
            List<Payment> payments = paymentRepository.getAll();

            foreach (Payment item in payments)
            {
                PaymentDTO payment = new PaymentDTO()
                {
                   ShipDate =item.ShipDate,
                   TransactionDate = item.TransactionDate,
                   PaymentMethode =item.PaymentMethode,
                   Cardnumber = item.Cardnumber,
                   CVV = item.CVV,
                   ZipCode =item.ZipCode,
                   Address=item.Address,
                   Email=item.Email,
                   PhoneNumber = item.PhoneNumber,
                   ClientName = item.ClientName,
                    OrderId = orderRepo.GetLastOrder().OrderId
                };
                paymentDTOs.Add(payment);
            }
            return Ok(paymentDTOs);
        }
        [HttpPost]
        public IActionResult PostPaymentData(PaymentDTO dtoPayment, string Email)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
                Payment payment = new Payment();
                payment.TransactionDate = DateTime.Now;
                payment.PaymentMethode = dtoPayment.PaymentMethode;
                payment.ZipCode = dtoPayment.ZipCode;
                payment.Cardnumber = dtoPayment.Cardnumber;
                payment.CVV = dtoPayment.CVV;
                payment.ShipDate = DateTime.Now.AddDays(1);
                payment.Address = dtoPayment.Address;
                payment.ClientName = dtoPayment.ClientName;
                payment.Email = dtoPayment.Email;
                payment.PhoneNumber = dtoPayment.PhoneNumber;
                payment.OrderId = orderRepo.GetLastOrder().OrderId;
            paymentRepository.Create(payment);
            Random random = new Random();
            var ShipmentNo = random.Next(1000, 10000);
            var messageBody = $"Thank you for your order MR {payment.ClientName}!\r\nDelivery Date: {payment.ShipDate}\r\nDelivery address: {payment.Address}\r\nShipment No: {ShipmentNo}\r\nOrder No: {payment.OrderId}\r\n";
            var message = new Message(new string[] { Email }, "Order Details", messageBody);
            emailService.SendEmail(message);
            return CreatedAtAction(nameof(getPayment), new { id = payment.Id }, payment);
        }
    }
}
