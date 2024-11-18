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
    public class LikePostService : BaseService<LikePost>, ILikePostService
    {
        private readonly ILikePostRepository _likePostRepository;
        public LikePostService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<LikePost> logger, ILikePostRepository likePostRepository) : base(unitOfWork, mapper, logger)
        {
            _likePostRepository = likePostRepository;
        }

        public async Task<ServiceResult> CreateLikePost(int postId, int freelancerId)
        {
            try
            {
                var newSavePist = new LikePost()
                {
                    FreelancerId = freelancerId,
                    PostId = postId
                };

                await _likePostRepository.CreateAsync(newSavePist);
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

        public async Task<ServiceResult> DeleteLikePost(int postId, int freelancerId)
        {
            try
            {
                var savePost = (await _likePostRepository.GetAllAsync()).FirstOrDefault(x => x.FreelancerId == freelancerId && x.PostId == postId);
                if (savePost == null)
                {
                    return new ServiceResult()
                    {
                        Message = "Không tìm thấy thông tin",
                        Status = StatusResult.ClientError,
                    };
                }


                _likePostRepository.Delete(savePost);
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

        public async Task<List<int>> GetLikePostOfFreelancer(int freelancerId)
        {
            var likePost = await _likePostRepository.GetAllAsync();
            return likePost.Where(x => x.FreelancerId == freelancerId).Select(x => x.PostId).ToList();
        }
    }
}
