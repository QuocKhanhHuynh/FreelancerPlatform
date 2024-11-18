using FreelancerPlatform.Application.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Abstraction.Service
{
    public interface ILikePostService
    {
        Task<ServiceResult> CreateLikePost(int postId, int freelancerId);
        Task<ServiceResult> DeleteLikePost(int postId, int freelancerId);
        Task<List<int>> GetLikePostOfFreelancer(int freelancerId);
    }
}
