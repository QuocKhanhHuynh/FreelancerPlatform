using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Abstraction.Service
{
    public interface IPostService
    {
        Task<ServiceResult> CreatePost(PostCreateRequest request);
      ///  Task<ServiceResult> CreateLikePost(int postId, int freelancerId);
        Task<ServiceResult> UpdatePost(int id, PostCreateRequest request);
        Task<ServiceResult> DeletePost(int id);
        Task<PostViewModel> GetPost(int Id);
        Task<List<PostViewModel>> GetPosts();
        Task<List<PostViewModel>> GetPostByFreelancerId(int id);
        Task<List<PostViewModel>> GetSavePostByFreelancerId(int freelancerId); 
    }
}
