using AutoMapper;
using FreelancerPlatform.Application.Abstraction.Repository;
using FreelancerPlatform.Application.Abstraction.Service;
using FreelancerPlatform.Application.Abstraction.UnitOfWork;
using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.Freelancer;
using FreelancerPlatform.Application.Dtos.ServiceForFreelancer;
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
    public class ServiceForFreelancerService : BaseService<ServiceForFreelancer>//, IServiceForFreelancerService
    {
        private readonly IServiceForFreelancerRepository _serviceForFreelancerRepository;
        public ServiceForFreelancerService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ServiceForFreelancer> logger, IServiceForFreelancerRepository serviceForFreelancerRepository) : base(unitOfWork, mapper, logger)
        {
            _serviceForFreelancerRepository = serviceForFreelancerRepository;
        }
        /*public async Task<ServiceResult> CreateServiceForFreelancerAsync(ServiceForFreelancerCreateRequest request)
        {
            try
            {
                var entity = _mapper.Map<ServiceForFreelancer>(request);
                await _serviceForFreelancerRepository.CreateAsync(entity);
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

        public async Task<ServiceResult> DeleteServiceForFreelancerAsync(int id)
        {
            try
            {
                ServiceForFreelancer entity = (await _serviceForFreelancerRepository.GetAllAsync()).FirstOrDefault(x => x.Id == id);
                if (entity == null)
                {
                    return new ServiceResult()
                    {
                        Message = "Không tìm thấy thônng tin",
                        Status = StatusResult.ClientError
                    };
                }
                entity.IsDeleted = true;
                _serviceForFreelancerRepository.Update(entity);
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

        public async Task<Pagination<ServiceForFreelancerQuickViewModel>> GetAllServiceForFreelancerAsync(int pageIndex, int pageTake, string? keywork = null)
        {
            var query = (await _serviceForFreelancerRepository.GetAllAsync()).Where(x => !x.IsDeleted);
            if (keywork != null)
            {
                query = query.Where(x => x.Name.Contains(keywork));
            }
            var pagination = new Pagination<ServiceForFreelancerQuickViewModel>();
            pagination.PageIndex = pageIndex;
            pagination.PageSize = pageTake;
            pagination.TotalRecords = query.Count();

            var items = query.Skip((pageIndex - 1) * pageTake).Take(pageTake).Select(x => _mapper.Map<ServiceForFreelancerQuickViewModel>(x)).ToList();
            pagination.Items = items;

            return pagination; 
        }

        public async Task<ServiceResult> UpdateServiceForFreelancerAsync(int id, ServiceForFreelancerUpdateRequest request)
        {
            try
            {
                ServiceForFreelancer entity = await _serviceForFreelancerRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return new ServiceResult()
                    {
                        Message = "Không tìm thấy thônng tin",
                        Status = StatusResult.ClientError
                    };
                }
                var updateEntity = _mapper.Map<ServiceForFreelancer>(request);
                entity.CreateUpdate = DateTime.Now;
                _serviceForFreelancerRepository.Update(updateEntity);
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
