using AutoMapper;
using FreelancerPlatform.Application.Abstraction.Repository;
using FreelancerPlatform.Application.Abstraction.Service;
using FreelancerPlatform.Application.Abstraction.UnitOfWork;
using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.Post;
using FreelancerPlatform.Application.ServiceImplementions.Base;
using FreelancerPlatform.Domain.Entity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FreelancerPlatform.Application.ServiceImplementions
{
    public class PostService : BaseService<Post>, IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IFreelancerRepository _freelancerRepository;
        private readonly ISavePostRepository _savePostRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly ILikePostRepository _likePostRepository;
        public PostService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<Post> logger, IPostRepository postRepository, IFreelancerRepository freelancerRepository,
            ISavePostRepository savePostRepository, ICommentRepository commentRepository, ILikePostRepository likePostRepository) : base(unitOfWork, mapper, logger)
        {
            _postRepository = postRepository;
            _freelancerRepository = freelancerRepository;
            _savePostRepository = savePostRepository;
            _commentRepository = commentRepository;
            _likePostRepository = likePostRepository;
        }

        /*public async Task<ServiceResult> CreateLikePost(int postId, int freelancerId)
        {
            try
            {
                var post = await _postRepository.GetByIdAsync(postId);
                if (post == null)
                {
                    return new ServiceResult()
                    {
                        Message = "Không tìm thấy thông tin thành công",
                        Status = StatusResult.ClientError
                    };
                }

                post.Title = request.Title;
                post.Content = request.Content;
                post.ImageUrl = request.ImageUrl;

                _postRepository.Update(post);
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
        }*/

        public async Task<ServiceResult> CreatePost(PostCreateRequest request)
        {
            try
            {
                var newpost = new Post()
                {
                    FreelancerId = request.FreelancerId,
                    Content = request.Content,
                    Title = request.Title
                };
                if (request.ImageUrl != null)
                {
                    newpost.ImageUrl = request.ImageUrl;
                }
                await _postRepository.CreateAsync(newpost);
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

        public async Task<ServiceResult> DeletePost(int id)
        {
            try
            {
                var post = await _postRepository.GetByIdAsync(id);
                if (post == null)
                {
                    return new ServiceResult()
                    {
                        Message = "Xóa thông tin thành công",
                        Status = StatusResult.ClientError
                    };
                }
                
                _postRepository.Delete(post);
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

        public async Task<PostViewModel> GetPost(int Id)
        {
            var post = await _postRepository.GetByIdAsync(Id);
            var freelancer = await _freelancerRepository.GetByIdAsync(post.FreelancerId);
            var comments = await _commentRepository.GetAllAsync();
            var likes = await _likePostRepository.GetAllAsync();
            return new PostViewModel()
            {
                Content = post.Content,
                CreateDay = post.CreateDay,
                FirstName = freelancer.FirstName,
                FreelancerId = freelancer.Id,
                Id = post.Id,
                FreelancerImageUrl = freelancer.ImageUrl,
                ImageUrl = post.ImageUrl,
                LastName = freelancer.LastName,
                Title = post.Title,
                CommentNumber = comments.Where(y => y.PostId == post.Id).Count(),
               LikeNumber = likes.Where(y => y.PostId == post.Id).Count()
            };
        }

        public async Task<List<PostViewModel>> GetPostByFreelancerId(int id)
        {
            var posts = await _postRepository.GetAllAsync();
            var freelancers = await _freelancerRepository.GetAllAsync();
            var query = from p in posts
                        join f in freelancers on p.FreelancerId equals f.Id
                        where p.FreelancerId == id
                        select new { p, f };
            query = query.OrderByDescending(x => x.p.CreateDay);
            var comments = await _commentRepository.GetAllAsync();
            var likes = await _likePostRepository.GetAllAsync();

            return query.Select(x => new PostViewModel()
            {
                Content = x.p.Content,
                CreateDay = x.p.CreateDay,
                FirstName = x.f.FirstName,
                FreelancerId = x.f.Id,
                FreelancerImageUrl = x.f.ImageUrl,
                Id = x.p.Id,
                ImageUrl = x.p.ImageUrl,
                LastName = x.f.LastName,
                Title = x.p.Title,
                CommentNumber = comments.Where(y => y.PostId == x.p.Id).Count(),
                LikeNumber = likes.Where(y => y.PostId == x.p.Id).Count()
            }).ToList();
        }

        public async Task<List<PostViewModel>> GetPosts()
        {
            var posts = await _postRepository.GetAllAsync();
            var freelancers = await _freelancerRepository.GetAllAsync();
            var query = from p in posts
                        join f in freelancers on p.FreelancerId equals f.Id
                        select new { p, f };
            query = query.OrderByDescending(x => x.p.CreateDay);
            var comments = await _commentRepository.GetAllAsync();
            var likes = await _likePostRepository.GetAllAsync();


            return query.Select(x => new PostViewModel()
            {
                Content = x.p.Content,
                CreateDay = x.p.CreateDay,
                FirstName = x.f.FirstName,
                FreelancerId = x.f.Id,
                FreelancerImageUrl = x.f.ImageUrl,
                Id = x.p.Id,
                ImageUrl = x.p.ImageUrl,
                LastName = x.f.LastName,
                Title = x.p.Title,
                CommentNumber = comments.Where(y => y.PostId == x.p.Id).Count(),
                LikeNumber = likes.Where(y => y.PostId == x.p.Id).Count()
            }).ToList();
        }

        public async Task<List<PostViewModel>> GetSavePostByFreelancerId(int freelancerId)
        {
            var savePostOfFreelancer = (await _savePostRepository.GetAllAsync()).Where(x => x.FreelancerId == freelancerId).Select(x => x.PostId);
            var posts = await _postRepository.GetAllAsync();
            var freelancers = await _freelancerRepository.GetAllAsync();
            var query = from p in posts
                        join f in freelancers on p.FreelancerId equals f.Id
                        where savePostOfFreelancer.Contains(p.Id)
                        select new { p, f };
            query = query.OrderByDescending(x => x.p.CreateDay);
            var likes = await _likePostRepository.GetAllAsync();


            return query.Select(x => new PostViewModel()
            {
                Content = x.p.Content,
                CreateDay = x.p.CreateDay,
                FirstName = x.f.FirstName,
                FreelancerId = x.f.Id,
                FreelancerImageUrl = x.f.ImageUrl,
                Id = x.p.Id,
                ImageUrl = x.p.ImageUrl,
                LastName = x.f.LastName,
                Title = x.p.Title,
                LikeNumber = likes.Where(y => y.PostId == x.p.Id).Count()
            }).ToList();
        }

        public async Task<ServiceResult> UpdatePost(int id, PostCreateRequest request)
        {
            try
            {
                var post = await _postRepository.GetByIdAsync(id);
                if (post == null)
                {
                    return new ServiceResult()
                    {
                        Message = "Không tìm thấy thông tin thành công",
                        Status = StatusResult.ClientError
                    };
                }

                post.Title = request.Title;
                post.Content = request.Content;
                post.ImageUrl = request.ImageUrl;

                _postRepository.Update(post);
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
