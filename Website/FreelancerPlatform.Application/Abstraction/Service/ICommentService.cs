using FreelancerPlatform.Application.Dtos.Comment;
using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Abstraction.Service
{
    public interface ICommentService
    {
        Task<ServiceResult> CreatePost(CommentCreateRequest request);
        Task<List<CommentViewModel>> GetCommentParent(int postId);
        Task<List<CommentViewModel>> GetCommentReply(int parent);
        Task<List<int>> GetLikeCommentOfFreelancer(int freelancerId);
    }
}
