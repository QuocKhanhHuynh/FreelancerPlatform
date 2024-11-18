using AutoMapper;
using FreelancerPlatform.Application.Abstraction.Repository;
using FreelancerPlatform.Application.Abstraction.Service;
using FreelancerPlatform.Application.Abstraction.UnitOfWork;
using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.RequirementServiceByFreelancer;
using FreelancerPlatform.Application.Dtos.ServiceForFreelancer;
using FreelancerPlatform.Application.ServiceImplementions.Base;
using FreelancerPlatform.Domain.Entity;
using FreelancerPlatform.Domain.Enum;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.ServiceImplementions
{
    public class RequirementServiceByFreelancerService : BaseService<RequirementServiceByFreelancerService>//, IRequirementServiceByFreelancerService
    {
        private readonly IRequirementServiceByFreelancerRepository _requirementServiceByFreelancerRepository;
        public RequirementServiceByFreelancerService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<RequirementServiceByFreelancerService> logger, IRequirementServiceByFreelancerRepository requirementServiceByFreelancerRepository) : base(unitOfWork, mapper, logger)
        {
            _requirementServiceByFreelancerRepository = requirementServiceByFreelancerRepository;
        }
        /*public async Task<ServiceResult> CreateRequiremenServiceByFreelancerAsync(RequirementServiceByFreelancerCreateRequest request)
        {
            try
            {
                var entity = _mapper.Map<RequirementServiceByFreelancer>(request);
                await _requirementServiceByFreelancerRepository.CreateAsync(entity);
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

        public async Task<ServiceResult> DeleteRequirementServiceByFreelancerAsync(int id)
        {
            try
            {
                RequirementServiceByFreelancer entity = (await _requirementServiceByFreelancerRepository.GetAllAsync()).FirstOrDefault(x => x.Id == id);
                if (entity == null)
                {
                    return new ServiceResult()
                    {
                        Message = "Không tìm thấy thônng tin",
                        Status = StatusResult.ClientError
                    };
                }
                if (entity.Status == (int)RequirementServiceStatus.Approve)
                {
                    return new ServiceResult()
                    {
                        Message = "Dịch vụ đã được chấp thuận, không thể xóa",
                        Status = StatusResult.ClientError
                    };
                }
               
                _requirementServiceByFreelancerRepository.Update(entity);
                await _unitOfWork.SaveChangeAsync();

                return new ServiceResult()
                {
                    Message = "Xoá thông tin thành công",
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

        public async Task<Pagination<RequirementServiceByFreelancerQuickViewModel>> GetAllRequirementServiceByFreelanceAsync(int pageIndex, int pageTake)
        {
            var query = (await _requirementServiceByFreelancerRepository.GetAllAsync()).Where(x => !x.IsDeleted);
            var pagination = new Pagination<RequirementServiceByFreelancerQuickViewModel>();
            pagination.PageIndex = pageIndex;
            pagination.PageSize = pageTake;
            pagination.TotalRecords = query.Count();

            var items = query.Skip((pageIndex - 1) * pageTake).Take(pageTake).Select(x => _mapper.Map<RequirementServiceByFreelancerQuickViewModel>(x)).ToList();
            pagination.Items = items;

            return pagination;
        }

        public async Task<Pagination<RequirementServiceByFreelancerQuickViewModel>> GetRequirementServiceByFreelanceAsync(int id, int pageIndex, int pageTake)
        {
            var query = (await _requirementServiceByFreelancerRepository.GetAllAsync()).Where(x => !x.IsDeleted && x.FreelancerId == id);
            var pagination = new Pagination<RequirementServiceByFreelancerQuickViewModel>();
            pagination.PageIndex = pageIndex;
            pagination.PageSize = pageTake;
            pagination.TotalRecords = query.Count();

            var items = query.Skip((pageIndex - 1) * pageTake).Take(pageTake).Select(x => _mapper.Map<RequirementServiceByFreelancerQuickViewModel>(x)).ToList();
            pagination.Items = items;

            return pagination;
        }

        public async Task<ServiceResult> UpdateStatusRequiremenServiceByFreelancerAsync(int id, RequirementServiceByFreelancerUpdateRequest request)
        {
            try
            {
                RequirementServiceByFreelancer entity = await _requirementServiceByFreelancerRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return new ServiceResult()
                    {
                        Message = "Không tìm thấy thônng tin",
                        Status = StatusResult.ClientError
                    };
                }
                entity.Status = request.Status;
                entity.CreateUpdate = DateTime.Now;
                _requirementServiceByFreelancerRepository.Update(entity);
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
        }*/
    }
}
