using FreelancerPlatform.Application.Abstraction.Service;
using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.Freelancer;
using FreelancerPlatform.Application.Dtos.Job;
using FreelancerPlatform.Application.Dtos.Post;
using FreelancerPlatform.Application.Extendsions;
using FreelancerPlatform.Application.ServiceImplementions;
using FreelancerPlatform.Client.Models;
using FreelancerPlatform.Client.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace FreelancerPlatform.Client.Controllers
{
    public class CommunityController : Controller
    {
        private readonly IPostService _postService;
        private readonly IStorageService _storageService;
        private readonly ISavePostService _savePostService;
        private readonly ILikePostService _likePostService;
        private readonly ILikeCommentService _likeCommentService;
        private readonly ICommentService _commentService;
        public CommunityController(IPostService postService, IStorageService storageService, ISavePostService savePostService,  ILikePostService likePostService, ILikeCommentService likeCommentService, ICommentService commentService)
        {
            _postService = postService;
            _storageService = storageService;
            _savePostService = savePostService;
            _likePostService = likePostService;
            _likeCommentService = likeCommentService;
            _commentService = commentService;
        }
        public async Task<IActionResult> Index()
        {
            var posts = await _postService.GetPosts();
            var savePotRaw = await _postService.GetSavePostByFreelancerId(User.GetUserId());
            var savePostId = savePotRaw.Select(x => x.Id).ToList();
            var likePost = await _likePostService.GetLikePostOfFreelancer(User.GetUserId());
            var likeComment = await _commentService.GetLikeCommentOfFreelancer(User.GetUserId());

            ViewBag.LikePost = likePost;
            ViewBag.SavePost = savePostId;
            ViewBag.LikeComment = likeComment;

            return View(posts);
        }

        public async Task<IActionResult> UpdatePost(int id)
        {
            var post = await _postService.GetPost(id);

            return View(post);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdatePost([FromForm] PostUpdateRequestForm request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new
                {
                    Field = x.Key,
                    Errors = x.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                })
                .ToList();

                return BadRequest(new { Errors = errors });
            }

            var applicationRequest = new PostCreateRequest()
            {
                Content = request.Content,
                Title = request.Title,
                FreelancerId = User.GetUserId(),
            };
            if (request.ImageUrl != null)
            {
                var post = await _postService.GetPost(request.Id);
                if (!string.IsNullOrEmpty(post.ImageUrl))
                    await _storageService.DeleteFileAsync(post.ImageUrl);

                applicationRequest.ImageUrl = await SaveFileAsync(request.ImageUrl);
            }
            var response = await _postService.UpdatePost(request.Id,applicationRequest);
            if(response.Status != StatusResult.Success)
            {
                return BadRequest();
            }
            return Ok(response.Message);
        }


        public async Task<IActionResult> GetPost(int id)
        {
            var post = await _postService.GetPost(id);
            var likePost = await _likePostService.GetLikePostOfFreelancer(User.GetUserId());

            ViewBag.LikePost = likePost;

            return View(post);
        }

        public IActionResult CreatePost()
        {
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreatePost([FromForm]  PostCreateRequestForm request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new
                {
                    Field = x.Key,
                    Errors = x.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                })
                .ToList();

                return BadRequest(new { Errors = errors });
            }

            var applicationRequest = new PostCreateRequest()
            {
                Content = request.Content,
                Title = request.Title,
                FreelancerId = User.GetUserId(),
            };
            if (request.ImageUrl != null)
            {
                applicationRequest.ImageUrl = await SaveFileAsync(request.ImageUrl);
            }
            var response = await _postService.CreatePost(applicationRequest);

            return Ok(response.Message);
        }


        [HttpPost]
        public async Task<IActionResult> DeletePost(int id)
        {
            var response = await _postService.DeletePost(id);
            if (response.Status != StatusResult.Success)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SavePost(int postId)
        {
            var response = await _savePostService.CreateSavePost(postId, User.GetUserId());
            if (response.Status != StatusResult.Success)
            {
                return BadRequest();
            }
            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> UnSavePost(int postId)
        {
            var response = await _savePostService.DeleteSavePost(postId, User.GetUserId());
            if (response.Status != StatusResult.Success)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> LikePost(int postId)
        {
            var response = await _likePostService.CreateLikePost(postId, User.GetUserId());
            if (response.Status != StatusResult.Success)
            {
                return BadRequest();
            }
            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> UnLikePost(int postId)
        {
            var response = await _likePostService.DeleteLikePost(postId, User.GetUserId());
            if (response.Status != StatusResult.Success)
            {
                return BadRequest();
            }
            return Ok();
        }

        private async Task<string> SaveFileAsync(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = file.FileName; //$"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }
    }


}
