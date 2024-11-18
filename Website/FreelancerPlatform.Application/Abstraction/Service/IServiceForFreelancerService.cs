using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.Freelancer;
using FreelancerPlatform.Application.Dtos.ServiceForFreelancer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Abstraction.Service
{
    public interface IServiceForFreelancerService
    {
        Task<ServiceResult> CreateServiceForFreelancerAsync(ServiceForFreelancerCreateRequest request);
        Task<ServiceResult> UpdateServiceForFreelancerAsync(int id, ServiceForFreelancerUpdateRequest request);
        Task<ServiceResult> DeleteServiceForFreelancerAsync(int id);
        Task<Pagination<ServiceForFreelancerQuickViewModel>> GetAllServiceForFreelancerAsync(int pageIndex, int pageTake, string? keywork = null);

    }
}
