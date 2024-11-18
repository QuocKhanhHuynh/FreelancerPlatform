using FreelancerPlatform.Application.Abstraction.Service;
using FreelancerPlatform.Application.ServiceImplementions;
using Microsoft.AspNetCore.Mvc;
using FreelancerPlatform.Application.Extendsions;

namespace FreelancerPlatform.Client.Controllers.Components
{
    public class ChatCountUnSeenViewComponent : ViewComponent
    {
        private readonly IChatService _chatService;
        public ChatCountUnSeenViewComponent(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var freelancerId = HttpContext.User.GetUserId();
            var count = await _chatService.CountChatUnSeen(freelancerId);
            return View("Default", count);
        }
    }
}
