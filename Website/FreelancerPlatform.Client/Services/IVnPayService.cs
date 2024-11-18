using FreelancerPlatform.Client.Models;
using System.Net.Http;

namespace FreelancerPlatform.Client.Services
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext content, VnPayRequestModel model);
        Task<VnPaymentResponseModel> PaymentExecute(IQueryCollection collection);
    }
}
