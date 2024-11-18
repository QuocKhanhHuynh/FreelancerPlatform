using FreelancerPlatform.Client.Models;
using FreelancerPlatform.Client.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FreelancerPlatform.Client.Controllers
{
   
    public class TransactionController : Controller
    {
        private readonly IVnPayService _vpnPayService;
        public TransactionController(IVnPayService vpnPayService)
        {
            _vpnPayService = vpnPayService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
       // [EnableCors("_myAllowSpecificOrigins")]
        public IActionResult CreateTransaction(TransactionCreateRequestClient clientRequest)
        {
            string cleanedNumberString = clientRequest.Amount.Replace(".", "");
            int amount = int.Parse(cleanedNumberString);
            var request = new VnPayRequestModel()
            {
                Amount = amount,
                CreatedDate = DateTime.Now,
                Description = clientRequest.Description,
                FullName = "Nguyễn Văn A",
                Id = new Random().Next(1, 10000),
                ContractId = clientRequest.ContractId,
            };
            var result = _vpnPayService.CreatePaymentUrl(HttpContext, request);

            return Redirect(result);
        }

        public async Task<IActionResult> PaymentCallback()
        {
            var response =await _vpnPayService.PaymentExecute(Request.Query);
            return View(response);
        }
    }
}
