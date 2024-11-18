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
    public class LikeCommentService : BaseService<LikeComment>, ILikeCommentService
    {
        private readonly ILikeCommentRepository _likeCommentRepository;
        public LikeCommentService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<LikeComment> logger, ILikeCommentRepository likeCommentRepository) : base(unitOfWork, mapper, logger)
        {
            _likeCommentRepository = likeCommentRepository;
        }
        public async Task<ServiceResult> CreateLikeComment(int commentId, int freelancerId)
        {

            try
            {
                var entity = (await _likeCommentRepository.GetAllAsync()).FirstOrDefault(x => x.FreelancerId == freelancerId && x.CommentId == commentId);
                if (entity != null)
                {
                    return new ServiceResult()
                    {
                        Message = "Tạo thông tin thành công",
                        Status = StatusResult.Success,
                    };
                }
                var newSavePist = new LikeComment()
                {
                    FreelancerId = freelancerId,
                    CommentId = commentId
                };

                await _likeCommentRepository.CreateAsync(newSavePist);
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

        public async Task<ServiceResult> DeleteLikeComment(int commentId, int freelancerId)
        {
            try
            {
                var savePost = (await _likeCommentRepository.GetAllAsync()).FirstOrDefault(x => x.FreelancerId == freelancerId && x.CommentId == commentId);
                if (savePost == null)
                {
                    return new ServiceResult()
                    {
                        Message = "Không tìm thấy thông tin",
                        Status = StatusResult.ClientError,
                    };
                }


                _likeCommentRepository.Delete(savePost);
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
