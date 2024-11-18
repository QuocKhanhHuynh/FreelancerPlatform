using AutoMapper;
using FreelancerPlatform.Application.Abstraction.Repository;
using FreelancerPlatform.Application.Abstraction.Service;
using FreelancerPlatform.Application.Abstraction.UnitOfWork;
using FreelancerPlatform.Application.Dtos.Apply;
using FreelancerPlatform.Application.Dtos.Category;
using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.ServiceImplementions.Base;
using FreelancerPlatform.Domain.Entity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.ServiceImplementions
{
    public class CategoryService : BaseService<Category>, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<Category> logger, ICategoryRepository categoryRepository) : base(unitOfWork, mapper, logger)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<ServiceResult> CreateCategoryAsync(CategoryCreateRequest request)
        {
            try
            {
                var category = new Category()
                {
                    Name = request.Name,
                    ImageUrl = request.ImageUrl
                };
                await _categoryRepository.CreateAsync(category);
                await _unitOfWork.SaveChangeAsync();

                return new ServiceResult()
                {
                    Message = "Tạo thông tin thành công",
                    Status = StatusResult.Success
                };
            }
            catch (Exception ex)
            {
                _unitOfWork.Cancel();
                _logger.LogError(ex.InnerException.Message);

                return new ServiceResult()
                {
                    Message = $"Lỗi hệ thống, ({ex.InnerException.Message})",
                    Status = StatusResult.SystemError
                };
            }
        }

        public async Task<ServiceResult> DeleteCategoryAsync(int id)
        {
            try
            {
                Category entity = (await _categoryRepository.GetAllAsync()).FirstOrDefault(x => x.Id == id);
                if (entity == null)
                {
                    return new ServiceResult()
                    {
                        Message = "Không tìm thấy thônng tin",
                        Status = StatusResult.ClientError
                    };
                }
                _categoryRepository.Update(entity);
                await _unitOfWork.SaveChangeAsync();

                return new ServiceResult()
                {
                    Message = "Xoá tin thành công",
                    Status = StatusResult.Success
                };
            }
            catch (Exception ex)
            {
                _unitOfWork.Cancel();
                _logger.LogError(ex.InnerException.Message);

                return new ServiceResult()
                {
                    Message = $"Lỗi hệ thống, ({ex.InnerException.Message})",
                    Status = StatusResult.SystemError
                };
            }
        }

        public async Task<Pagination<CategoryQuickViewModel>> GetAllCategoryAsync(int pageIndex, int pageTake, string? keywork = null)
        {
            var query = (await _categoryRepository.GetAllAsync());
            if (keywork != null)
            {
                query = query.Where(x => x.Name.Contains(keywork));
            }
            var pagination = new Pagination<CategoryQuickViewModel>();
            pagination.PageIndex = pageIndex;
            pagination.PageSize = pageTake;
            pagination.TotalRecords = query.Count();

            var items = query.Skip((pageIndex - 1) * pageTake).Take(pageTake).Select(x => _mapper.Map<CategoryQuickViewModel>(x)).ToList();
            pagination.Items = items;

            return pagination;
        }

		public async Task<List<CategoryQuickViewModel>> GetCategoryAsync()
		{
			var query = (await _categoryRepository.GetAllAsync());
			var items = query.Select(x => new CategoryQuickViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                ImageUrl = x.ImageUrl
            }).ToList();
			return items;

		}

		public async Task<ServiceResult> UpdateCategoryAsync(int id, CategoryUpdateRequest request)
        {
            try
            {
                Category entity = await _categoryRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return new ServiceResult()
                    {
                        Message = "Không tìm thấy thônng tin",
                        Status = StatusResult.ClientError
                    };
                }
                entity.Name = request.Name;
                entity.ImageUrl = request.ImageUrl != null ? request.ImageUrl : entity.ImageUrl;
                entity.CreateUpdate = DateTime.Now;
                _categoryRepository.Update(entity);
                await _unitOfWork.SaveChangeAsync();

                return new ServiceResult()
                {
                    Message = "cập nhật thành công",
                    Status = StatusResult.Success
                };
            }
            catch (Exception ex)
            {
                _unitOfWork.Cancel();
                _logger.LogError(ex.InnerException.Message);

                return new ServiceResult()
                {
                    Message = $"Lỗi hệ thống, ({ex.InnerException.Message})",
                    Status = StatusResult.SystemError
                };
            }
        }
    }
}
