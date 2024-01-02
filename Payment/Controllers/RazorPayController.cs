using Microsoft.AspNetCore.Mvc;
using PaymentService.RazorPay.Models;
using PaymentService.RazorPay;
using PaymentService;

namespace Payment.Controllers
{
    public class RazorPayController : Controller
    {
        private readonly ILogger<RazorPayController> _logger;
        private readonly RazorPayIntegration _service;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RazorPayController(ILogger<RazorPayController> logger, RazorPayIntegration service, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ProcessRequestOrder(PaymentRequest _paymentRequest)
        {
            MerchantOrder _marchantOrder = await _service.ProcessMerchantOrder(_paymentRequest);
            return View("Payment", _marchantOrder);
        }
        [HttpPost]
        public async Task<IActionResult> CompleteOrderProcess()
        {
            string PaymentMessage = await _service.CompleteOrderProcess(_httpContextAccessor);
            if (PaymentMessage == "captured")
            {
                return RedirectToAction("Success");
            }
            else
            {
                return RedirectToAction("Failed");
            }
        }
        public IActionResult Success()
        {
            return View();
        }
        public IActionResult Failed()
        {
            return View();
        }
    }
}
