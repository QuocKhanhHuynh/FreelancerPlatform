using AutoMapper;
using FreelancerPlatform.Application.Abstraction.Repository;
using FreelancerPlatform.Application.Abstraction.Service;
using FreelancerPlatform.Application.Abstraction.UnitOfWork;
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
    public class SavePostService : BaseService<SavePost>, ISavePostService
    {
        private readonly ISavePostRepository _savePostRepository;
        public SavePostService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<SavePost> logger, ISavePostRepository savePostRepository) : base(unitOfWork, mapper, logger)
        {
            _savePostRepository = savePostRepository;
        }

        public async Task<ServiceResult> CreateSavePost(int postId, int freelancerId)
        {
            try
            {
                var newSavePist = new SavePost()
                {
                    FreelancerId = freelancerId,
                    PostId = postId
                };

                await _savePostRepository.CreateAsync(newSavePist);
                await _unitOfWork.SaveChangeAsync();

                return new ServiceResult()
                {
                    Message = "Tạo thông tin thành công",
                    Status = StatusResult.Success,
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

        public async Task<ServiceResult> DeleteSavePost(int postId, int freelancerId)
        {
            try
            {
                var savePost = (await _savePostRepository.GetAllAsync()).FirstOrDefault(x => x.FreelancerId == freelancerId && x.PostId == postId);
                if (savePost == null)
                {
                    return new ServiceResult()
                    {
                        Message = "Không tìm thấy thông tin",
                        Status = StatusResult.ClientError,
                    };
                }


                _savePostRepository.Delete(savePost);
                await _unitOfWork.SaveChangeAsync();

                return new ServiceResult()
                {
                    Message = "Xoá thông tin thành công",
                    Status = StatusResult.Success,
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
