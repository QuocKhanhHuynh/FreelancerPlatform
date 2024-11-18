using AutoMapper;
using FreelancerPlatform.Application.Abstraction.Repository;
using FreelancerPlatform.Application.Abstraction.Service;
using FreelancerPlatform.Application.Abstraction.UnitOfWork;
using FreelancerPlatform.Application.Dtos.Apply;
using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.Role;
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
    public class RoleService : BaseService<Role>//, IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        public RoleService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<Role> logger, IRoleRepository roleRepository) : base(unitOfWork, mapper, logger)
        {
            _roleRepository = roleRepository;
        }
        public async Task<ServiceResult> CreateRoleAsync(RoleCreateRequest request)
        {
            try
            {
                var entity = _mapper.Map<Role>(request);
                await _roleRepository.CreateAsync(entity);
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

        public async Task<ServiceResult> DeleteRoleAsync(int id)
        {
            try
            {
                Role entity = (await _roleRepository.GetAllAsync()).FirstOrDefault(x => x.Id == id);
                if (entity == null)
                {
                    return new ServiceResult()
                    {
                        Message = "Không tìm thấy thônng tin",
                        Status = StatusResult.ClientError
                    };
                }
                
                _roleRepository.Update(entity);
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

        public async Task<Pagination<RoleQuickViewModel>> GetAllRoleAsync(int pageIndex, int pageTake)
        {
            var query = (await _roleRepository.GetAllAsync());
            var pagination = new Pagination<RoleQuickViewModel>();
            pagination.PageIndex = pageIndex;
            pagination.PageSize = pageTake;
            pagination.TotalRecords = query.Count();

            var items = query.Skip((pageIndex - 1) * pageTake).Take(pageTake).Select(x => _mapper.Map<RoleQuickViewModel>(x)).ToList();
            pagination.Items = items;

            return pagination;
        }

        /*public async Task<RoleQuickViewModel> GetRoleAsync(int id)
        {
            Role entity = await _roleRepository.GetByIdAsync(id);
            return _mapper.Map<RoleQuickViewModel>(entity) : new RoleQuickViewModel();
        }*/

        public async Task<ServiceResult> UpdateRoleAsync(int id, RoleUpdateRequest request)
        {
            try
            {
                Role entity = await _roleRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return new ServiceResult()
                    {
                        Message = "Không tìm thấy thônng tin",
                        Status = StatusResult.ClientError
                    };
                }
                var updateEntity = _mapper.Map<Role>(request);
                entity.CreateUpdate = DateTime.Now;
                _roleRepository.Update(updateEntity);
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
