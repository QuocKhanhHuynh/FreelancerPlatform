using AutoMapper;
using FreelancerPlatform.Application.Abstraction.Repository;
using FreelancerPlatform.Application.Abstraction.Service;
using FreelancerPlatform.Application.Abstraction.UnitOfWork;
using FreelancerPlatform.Application.Dtos.Category;
using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.Skill;
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
    public class SkillService : BaseService<SKill>, ISkillService
    {
        private readonly ISkillRepository _skillRepository;
        public SkillService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<SKill> logger, ISkillRepository skillRepository) : base(unitOfWork, mapper, logger)
        {
            _skillRepository = skillRepository;
        }

        public async Task<ServiceResult> CreateSkillAsync(SkillCreateRequest request)
        {
            try
            {
               
                var newSkill = new SKill()
                {
                   Name = request.Name,
                };
               
                await _skillRepository.CreateAsync(newSkill);
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

                return new ServiceResultInt()
                {
                    Message = $"Lỗi hệ thống, ({ex.InnerException.Message})",
                    Status = StatusResult.SystemError
                };
            }
        }

        public async Task<ServiceResult> DeleteSkillAsync(int id)
        {
            try
            {
                var skill = await _skillRepository.GetByIdAsync(id);
                if (skill == null)
                {
                    return new ServiceResult()
                    {
                        Message = "Không tìm thấy thông tin",
                        Status = StatusResult.ClientError
                    };
                }
               

                _skillRepository.Delete(skill);
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

                return new ServiceResultInt()
                {
                    Message = $"Lỗi hệ thống, ({ex.InnerException.Message})",
                    Status = StatusResult.SystemError
                };
            }
        }

        public async Task<List<SkillQuickViewModel>> GetSkillAsync()
        {
            var query = (await _skillRepository.GetAllAsync());
            var items = query.Select(x => new SkillQuickViewModel() { Id = x.Id, Name = x.Name }).ToList();

            return items;
        }

        public async Task<ServiceResult> UpdateSkillAsync(SkillUpdateRequest request)
        {
            try
            {
                var skill = await _skillRepository.GetByIdAsync(request.Id);
                if (skill == null){
                    return new ServiceResult()
                    {
                        Message = "Không tìm thấy thông tin",
                        Status = StatusResult.ClientError
                    };
                }
               skill.Name = request.Name;

                _skillRepository.Update(skill);
                await _unitOfWork.SaveChangeAsync();

                return new ServiceResult()
                {
                    Message = "Cập nhật thông tin thành công",
                    Status = StatusResult.Success
                };
            }
            catch (Exception ex)
            {
                _unitOfWork.Cancel();
                _logger.LogError(ex.InnerException.Message);

                return new ServiceResultInt()
                {
                    Message = $"Lỗi hệ thống, ({ex.InnerException.Message})",
                    Status = StatusResult.SystemError
                };
            }
        }
    }
}
