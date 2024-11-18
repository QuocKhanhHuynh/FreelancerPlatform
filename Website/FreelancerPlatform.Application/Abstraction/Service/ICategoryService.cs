using FreelancerPlatform.Application.Dtos.Category;
using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.Job;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Abstraction.Service
{
    public interface ICategoryService
    {
        Task<ServiceResult> CreateCategoryAsync(CategoryCreateRequest request);
        Task<ServiceResult> UpdateCategoryAsync(int id, CategoryUpdateRequest request);
        Task<ServiceResult> DeleteCategoryAsync(int id);
        Task<Pagination<CategoryQuickViewModel>> GetAllCategoryAsync(int pageIndex, int pageTake, string? keywork = null);
		Task<List<CategoryQuickViewModel>> GetCategoryAsync();
	}
}
