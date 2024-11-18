using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.FavoriteJob;
using FreelancerPlatform.Application.Dtos.Freelancer;
using FreelancerPlatform.Application.Dtos.Job;
using FreelancerPlatform.Application.Dtos.Offer;
using FreelancerPlatform.Application.Dtos.Potient;
using FreelancerPlatform.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Abstraction.Service
{
	public interface IFavoriteJobService
	{
		/*Task<ServiceResult> CreateFavoriteJobAsync(FavoriteJobCreateRequest request);
		Task<ServiceResult> DeleteFavoriteJobAsync(FavoriteJobDeleteRequest request);
		Task<bool> CheckFavoriteJobAsync(FavoriteJobInfor request);
        Task<Pagination<FavoriteJobQuickViewModel>> GetAllFavoriteJobAsync(int id, int pageIndex, int pageTake, string? keyword, int? categoryId, int? addressId, int? startDeal, int? endDeal);
        Task<List<FavoriteJobQuickViewModel>> GetAllFavoriteJobNotFillterAsync();*/
        Task<List<int>> GetFavoriteJobOfFreelancerAsync(int id);
        Task<List<JobQuickViewModel>> GetFavoriteJobOfFreelancerSecondAsync(int id);
        Task<ServiceResult> CreateFavoriteJobAsync(FavoriteJobCreateRequest request);
        Task<ServiceResult> DeleteFavoriteJobAsync(FavoriteJobDeleteRequest request);
    }
}
