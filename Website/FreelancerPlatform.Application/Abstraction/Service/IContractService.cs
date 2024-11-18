using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Abstraction.Service
{
    public interface IContractService
    {
        Task<ServiceResult> CreateContract(ContractCreateRequest request);
        Task<ServiceResult> ReviewPartner(PartnerReviewRequest request);
        Task<ServiceResult> UpdateContractAcceptStatus(int contractId, int status);
        Task<ServiceResult> UpdateContract(ContractUpdateRequest request);
        Task<ServiceResult> UpdateContractStatus(int contractId);
        Task<List<ContractQuickViewModel>> GetContractOfFreelancer(int id);
        Task<List<ContractQuickViewModel>> GetContractOfRecruiter(int id);
        Task<List<ReviewOfFreelancerQuickViewModel>> GetReviewOfFreelancer(int freelancerId);
        Task<ContractViewModel> GetContract(int id);
        Task<List<ContractQuickViewModel>> GetAllContract();
    }
}
