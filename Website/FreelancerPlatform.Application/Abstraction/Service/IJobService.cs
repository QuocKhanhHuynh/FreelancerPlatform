using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.Job;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Abstraction.Service
{
    public interface IJobService
    {
        Task<ServiceResult> CreateJobAsync(JobCreateRequest request);
        Task<List<JobQuickViewModel>> GetAllJobsAsync();
        Task<JobViewModel> GetJobAsync(int id);
        Task<ServiceResult> HidenJobAsync(int id);
        Task<ServiceResult> DeleteHidenJobAsync(int id);
        Task<ServiceResult> UpdateJobAsync(int id, JobUpdateRequest request);
        Task<ServiceResult> AddViewRecent(int freelancerId, int jobId);
        Task<List<JobQuickViewModel>> GetRecentView();
        Task<List<int>> GetJobIdRecentViewOfFreelancer(int freelancerId);
        /*Task<ServiceResult> UpdateJobAsync(int id, JobUpdateRequest request);
        Task<ServiceResult> UpdateHidenJobAsync(int id);
        Task<bool> CheckHidenJobAsync(int id);
        Task<ServiceResult> DeleteJobAsync(int id);
        Task<Pagination<JobQuickViewModel>> GetAllJobAsync(int pageIndex, int pageTake, int? categoryId, int? addressId, string? keywork = null, int? startDeal = null, int? endDeal = null);
		
		Task<JobViewModel> GetJobAsync(int id);
        Task<Pagination<JobQuickViewModel>> GetJobOfRecruiterAsync(int recruiterId, int pageIndex, int pageTake, int? categoryId, string? keywork = null, int? startDeal = null, int? endDeal = null);
		Task<List<JobQuickViewModel>> GetJobOfRecruiterNuotFilterAsync(int recruiterId);
        Task<List<JobQuickViewModel>> GetJobNotFilterAsync();

        Task<ServiceResult> AddFavoriteJobAsync(FavoriteJobAddRequest request);
        Task<ServiceResult> DeleteFavoriteJobAsync(int freelancerId, int jobId);
        Task<Pagination<JobQuickViewModel>> GetFavoriteJobAsync(int freelancerId, int pageIndex, int pageTake);*/

    }
}
