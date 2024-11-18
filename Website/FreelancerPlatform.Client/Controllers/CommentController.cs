using FreelancerPlatform.Application.Abstraction.Service;
using FreelancerPlatform.Application.Dtos.Comment;
using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Extendsions;
using FreelancerPlatform.Application.ServiceImplementions;
using Microsoft.AspNetCore.Mvc;

namespace FreelancerPlatform.Client.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IFreelancerService _freelancerService;
        private readonly ILikeCommentService _likeCommentService;
        public CommentController(ICommentService commentService, IFreelancerService freelancerService, ILikeCommentService likeCommentService)
        {
            _commentService = commentService;
            _freelancerService = freelancerService;
            _likeCommentService = likeCommentService;
        }
        public async Task<IActionResult> CreateComment(CommentCreateRequest request)
        {
            var response = await _commentService.CreatePost(request);
            if (response.Status != StatusResult.Success)
            {
                return BadRequest();
            }
            //var freelancer = await _freelancerService.GetFreelancerAsync(request.FreelancerId);
            var result = (ServiceResultObject<CommentCreateSuccessResult>) response;
            return Ok(result.Object);
        }
        [HttpPost]
        public async Task<IActionResult> GetCommentOfPost(int postId)
        {
            var response = await _commentService.GetCommentParent(postId);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetCommentOfParent(int parent)
        {
            var response = await _commentService.GetCommentReply(parent);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddLikeComment(int commentId)
        {
            var response = await _likeCommentService.CreateLikeComment(commentId, User.GetUserId());
            if (response.Status != StatusResult.Success)
            {
                return BadRequest();
            }
            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> DeleteLikeComment(int commentId)
        {
            var response = await _likeCommentService.DeleteLikeComment(commentId, User.GetUserId());
            if (response.Status != StatusResult.Success)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
