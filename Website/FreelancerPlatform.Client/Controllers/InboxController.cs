using FreelancerPlatform.Application.Abstraction.Service;
using FreelancerPlatform.Application.Dtos.Chat;
using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.Potient;
using FreelancerPlatform.Application.Extendsions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FreelancerPlatform.Client.Controllers
{
    public class InboxController : Controller
    {
        private readonly IChatService _chatService;
        private readonly IFreelancerService _freelancerService;
        public InboxController(IChatService chatService, IFreelancerService freelancerService)
        {
            _chatService = chatService;
            _freelancerService = freelancerService;
        }
        public async Task<IActionResult> GetInbox(int freelancerB = 0)
        {
            if (freelancerB != 0)
            {
                await _chatService.CheckAndCreateHubChat(new HubChatCreateViewModel()
                {
                    FreelancerA = User.GetUserId(),
                    FreelancerB = freelancerB
                });
            }
            var chats = await _chatService.GetAllChat(User.GetUserId());
            var Freelancer = await _freelancerService.GetFreelancerAsync(User.GetUserId());
            ViewBag.FreelancerA = User.GetUserId();
            ViewBag.FreelancerB = freelancerB;
            ViewBag.ImageUrl = Freelancer.ImageUrl;
            ViewBag.LastName = Freelancer.LastName;
            ViewBag.FirstName = Freelancer.FirstName;
            return View(chats);
        }

        [HttpPost]
        public async Task<IActionResult> CreateChat(ChatCreateRequest request)
        {
            request.FreelancerId = User.GetUserId();
            request.HubChatId = request.HubChatId != 0 ? request.HubChatId : null;
            var response = await _chatService.CreateChat(request);
            if (response.Status != StatusResult.Success)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSeenStatus(int id, int freelancerId)
        {
            var response = await _chatService.UpdateSeenStatus(id, freelancerId);
            if (response.Status != StatusResult.Success)
            {
                return BadRequest();
            }
            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> UpdateAllSeenStatus(int id)
        {
            var response = await _chatService.UpdateAllSeenStatus(id);
            if (response.Status != StatusResult.Success)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> ReloadChat()
        {
            var chats = await _chatService.GetAllChat(User.GetUserId());
            var chatHandle = new Dictionary<string, List<ChatViewModel>>();
            foreach(var i in chats)
            {
                var key = $"{i.Key.HubChatId}-{i.Key.FreelancerA}-{i.Key.LastNameA}-{i.Key.FirstNameA}-{i.Key.FreelancerB}-{i.Key.LastNameB}-{i.Key.FirstNameB}-{i.Key.ImageUrlA}-{i.Key.ImageUrlB}-{i.Key.SeenFreelancerA}-{i.Key.SeenFreelancerB}";
                chatHandle[key] = i.Value;
            }
            //var chatJson = JsonConvert.SerializeObject(chats);
            
            return Ok(chatHandle);
        }


        [HttpPost]
        public async Task<IActionResult> ReCallInbox(int id)
        {
            var result = await _chatService.RecallInbox(id);
            if (result.Status != StatusResult.Success)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CheckChatCountUnSeen(int id)
        {
            var result = await _chatService.CountChatUnSeen(id);
            return Ok(result);
        }
    }
}
