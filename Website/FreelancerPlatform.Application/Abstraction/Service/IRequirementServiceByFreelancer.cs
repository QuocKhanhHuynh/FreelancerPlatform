using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.RequirementServiceByFreelancer;
using FreelancerPlatform.Application.Dtos.ServiceForFreelancer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Abstraction.Service
{
    public interface IRequirementServiceByFreelancerService
    {
        Task<ServiceResult> CreateRequiremenServiceByFreelancerAsync(RequirementServiceByFreelancerCreateRequest request);
        Task<ServiceResult> UpdateStatusRequiremenServiceByFreelancerAsync(int id, RequirementServiceByFreelancerUpdateRequest request);
        Task<ServiceResult> DeleteRequirementServiceByFreelancerAsync(int id);
        Task<Pagination<RequirementServiceByFreelancerQuickViewModel>> GetAllRequirementServiceByFreelanceAsync(int pageIndex, int pageTake);
        Task<Pagination<RequirementServiceByFreelancerQuickViewModel>> GetRequirementServiceByFreelanceAsync(int id, int pageIndex, int pageTake);
    }
}
