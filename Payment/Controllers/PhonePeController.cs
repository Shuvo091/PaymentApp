using Microsoft.AspNetCore.Mvc;
using PaymentService;

namespace Payment.Controllers
{
    public class PhonePeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> ProcessPayment()
        {
            PhonePeIntegration phonePeIntegration = new PhonePeIntegration();

            // Example: Initiating a payment
            decimal amount = 100.00m;
            string cardNumber = "4242424242424242";
            string cardType = "DEBIT_CARD";
            string cardIssuer = "VISA";
            int expiryMonth = 12;
            int expiryYear = 2023;
            string cvv = "936";

            // Initiate payment using the PhonePeIntegration class
            string paymentResult = await phonePeIntegration.InitiatePaymentAsync(amount, cardNumber, cardType, cardIssuer, expiryMonth, expiryYear, cvv);

            // Process the payment result as needed
            ViewBag.PaymentResult = paymentResult;

            return View(); // Return a view or perform further actions based on the payment result
        }
    }
}
