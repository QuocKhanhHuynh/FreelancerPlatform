using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.Freelancer;
using FreelancerPlatform.Application.Dtos.Offer;
using FreelancerPlatform.Application.Dtos.Potient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Abstraction.Service
{
	public interface IPotientSerivice
	{
        /*Task<ServiceResult> CreatePotientAsync(PotientCreateRequest request);
        Task<Pagination<FreelancerQuickViewModel>> GetPotienFreelancerOfUerAsync(int id, int pageIndex, int pageTake, string? keyword, int? CategoryId, int? AddressId);
		Task<List<FreelancerQuickViewModel>> GetNotFilterAllFreelancerAsync(int id);
        Task<ServiceResult> DeletePotientAsync(int freelancerId, int recruiterId);
		Task<int> CheckPotientOfRecruiter(int recruiterId, int freelancerId);*/
        Task<List<int>> GetPotientFreelancerAsync(int id);
        Task<List<PotientQuickViewModel>> GetAllPotientFreelancerAsync(int id);
        Task<ServiceResult> AddPotientFreelancerAsync(PotientInfor request);
        Task<ServiceResult> RemovePotientFreelancerAsync(PotientInfor request);
    }
}
