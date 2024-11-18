using FreelancerPlatform.Application.Abstraction.Repository;
using FreelancerPlatform.Application.Abstraction.Service;
using FreelancerPlatform.Application.Dtos.Transaction;
using FreelancerPlatform.Application.Extendsions;
using FreelancerPlatform.Client.Models;
using FreelancerPlatform.Client.Payments;

namespace FreelancerPlatform.Client.Services
{
    public class VnPaymentService : IVnPayService
    {
        private readonly IConfiguration _config;
        private readonly ITransactionService _transactionService;
        private readonly IHttpContextAccessor _contextAccessor;
        public VnPaymentService(IConfiguration config, ITransactionService transactionService, IHttpContextAccessor httpContextAccessor)
        {
            _config = config;
            _transactionService = transactionService;
            _contextAccessor = httpContextAccessor;
        }
        public string CreatePaymentUrl(HttpContext context, VnPayRequestModel model)
        {
            var tick = DateTime.Now.ToString();
            var pay = new VnPayLibrary();

            pay.AddRequestData("vnp_Version", _config["VnPay:Version"]);
            pay.AddRequestData("vnp_Command", _config["VnPay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _config["VnPay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", (model.Amount * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", model.CreatedDate.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _config["VnPay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _config["VnPay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"{model.Description}");
            pay.AddRequestData("vnp_OrderType", "contract");
            pay.AddRequestData("vnp_ReturnUrl", _config["VnPay:PaymentBackReturnUrl"]);
            pay.AddRequestData("vnp_TxnRef", $"{model.Id}-{model.ContractId}");

            var paymentUrl = pay.CreateRequestUrl(_config["VnPay:BaseUrl"], _config["VnPay:HashSecrect"]);
            return paymentUrl;

        }

        public async Task<VnPaymentResponseModel> PaymentExecute(IQueryCollection collection)
        {
            var pay = new VnPayLibrary();

            foreach (var(key, value) in collection)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    pay.AddResponseData(key, value.ToString());


                }
            }

            var vnp_orderId = pay.GetResponseData("vnp_TxnRef").Substring(pay.GetResponseData("vnp_TxnRef").IndexOf('-') + 1);
         //   var vnp_transaction_id = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo"));
         //   var vnp_secure_hash = collection.FirstOrDefault(x => x.Key == "vnp_SecureHash").Value;
            var vnp_response_code = pay.GetResponseData("vnp_ResponseCode");
            var vnp_order_info = pay.GetResponseData("vnp_OrderInfo");
            var vnp_amount = pay.GetResponseData("vnp_Amount");
         //   bool checkSignature = pay.ValidateSignature(vnp_secure_hash, _config["VnPay:HashSecrect"]);
         /*   if (checkSignature)
            {
                return new VnPaymentResponseModel()
                {
                    Success = false
                };
            }*/
            if (vnp_response_code.Equals("00"))
            {
                await _transactionService.CreateTransaction(new TransactionCreateRequest()
                {
                    Amount = int.Parse(vnp_amount) / 100,
                    Content = vnp_order_info,
                    ContractId = int.Parse(vnp_orderId),
                    FreelancerId = (_contextAccessor.HttpContext?.User.GetUserId()).GetValueOrDefault()
                });
            }
            return new VnPaymentResponseModel()
            {
          //      Success = true,
           //     PaymentMethod = "VnPay",
                OrderDescription = vnp_orderId,
              //  OrderId = vnp_orderId.ToString(),
           //     TransactionId = vnp_transaction_id.ToString(),
            //    Token = vnp_secure_hash,
                VnPayResponseCode = vnp_response_code,
                Amount = int.Parse(vnp_amount) / 100

            };
        }
    }
}
