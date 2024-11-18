using FreelancerPlatform.Application.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Abstraction.Service
{
    public interface ISavePostService
    {
        Task<ServiceResult> CreateSavePost(int postId, int freelancerId);
        Task<ServiceResult> DeleteSavePost(int postId, int freelancerId);
    }
}
