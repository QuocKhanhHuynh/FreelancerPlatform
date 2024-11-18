using FreelancerPlatform.Application.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Abstraction.Service
{
    public interface ILikeCommentService
    {
        Task<ServiceResult> CreateLikeComment(int commentId, int freelancerId);
        Task<ServiceResult> DeleteLikeComment(int commentId, int freelancerId);
    }
}
