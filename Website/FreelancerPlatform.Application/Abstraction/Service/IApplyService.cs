using FreelancerPlatform.Application.Dtos.Apply;
using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.Job;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Abstraction.Service
{
    public interface IApplyService
    {
        Task<ServiceResultInt> CreateApplyAsync(ApplyCreateRequest request);
        Task<List<ApplyOfJobQuickViewModel>> GetApplyOfJobAsync(int jobId);
        Task<ApplyOfJobQuickViewModel> GetApplyAsync(int id);
        Task<List<JobQuickViewModel>> GetApplyByFreelancerAsync(int id);
        Task<ServiceResult> DeleteApplyAsync(int id);
        //Task<List<Apply>> CheckApplyOfFreelancer(int freelancerId);
        /*Task<ServiceResult> CreateApplyAsync(ApplyCreateRequest request);
        Task<ServiceResult> UpdateApplyAsync(int id, ApplyUpdateRequest request);
        Task<ServiceResult> UpdateStatusApplyAsync(int id, int status);
        Task<ServiceResult> DeleteApplyAsync(int id);
        Task<Pagination<ApplyQuickViewModel>> GetAllApplyAsync(int pageIndex, int pageTake);
        Task<ApplyViewModel> GetApplyAsync(int id);
        Task<Pagination<ApplyQuickViewModel>> GetApplyOfRecruiterAsync(int recruiterId, int pageIndex, int pageTake);
		Task<Pagination<ApplyQuickViewModel>> GetApplyByFreelancerAsync(int freelancerId, int pageIndex, int pageTake);
        Task<Pagination<ApplyQuickViewModel>> GetJobApplyByFreelancerAsync(int pageIndex, int pageTake, int freelancerId, string? keyword, int? categoryId, int? addressId, int? startDeal, int? endDeal);
        Task<List<ApplyQuickViewModel>> GetJobApplyByFreelancerNptFilterAsync(int freelancerId);
		Task<List<ApplyOfJobQuickViewModel>> GetApplyOfJobAsync(int jobId);
        Task<Pagination<ApplyApproveViewModel>> GetApplyApproveAsync(int recruiterId, int pageIndex, int pageTake, int? jobId);*/

    }
}
