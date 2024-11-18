using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.Freelancer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Abstraction.Service
{
    public interface IFreelancerService : IAccountService, ILoginService
    {
        Task<FreelancerViewModel> GetFreelancerAsync(int id);
        Task<FreelancerViewModel> GetFreelancerByEmailAsync(string email);
        Task<ServiceResult> UpdateFreelancerAsync(int id, FreelancerUpdateRequest request);
        Task<ServiceResult> UpdatePaymentAsync(int id, FreelancerPaymentUpdateRequest request);
        Task<List<FreelancerQuickViewModel>> GetAllFreelancerAsync();
        Task<ServiceResult> LockAcountAsync(int adminId);
        Task<ServiceResult> UnLockAcountAsync(int adminId);
        Task<List<PaymentConfirmViewModel>> GetFeelancerVerifyPayment();
        Task<ServiceResult> UpdateVerifyPayment(int id, bool statusVerify);
        Task<bool> CheckExistFreelancerByEmail(string email);
        Task<List<FreelancerQuickViewModel>> GetFreelancerRecommendation(int freelancerId);
        //Task<ServiceResult> CreateFreelancerAsync(FreelancerCreateRequest request);
        /*Task<ServiceResult> CreateFreelancerAsync(FreelancerCreateRequest request);
        Task<ServiceResult> UpdateFreelancerAsync(int id, FreelancerUpdateRequest request);
        Task<ServiceResult> UpdateIdentityAsync(int id, FreelancerIdentityUpdateRequest request);
		Task<ServiceResult> UpdateAboutAsync(int id, FreelancerAboutUpdateRequest request);
		Task<Pagination<FreelancerQuickViewModel>> GetAllFreelancerAsync(int pageIndex, int pageTake, int? categoryId, int? addressId, string? keywork = null);
        Task<List<FreelancerQuickViewModel>> GetNotFilterAllFreelancerAsync();
        Task<Pagination<FreelancerQuickViewModel>> GetIdentityUpdateFreelancerAsync(int pageIndex, int pageTake, string? keywork = null);
        Task<FreelancerViewModel> GetFreelancerAsync(int id);*/
    }
}
