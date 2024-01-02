using Microsoft.AspNetCore.Mvc;
using Payment.Models;
using PaymentService;
using System.Diagnostics;

namespace Payment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
