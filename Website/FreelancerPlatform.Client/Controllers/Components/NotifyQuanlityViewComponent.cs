using FreelancerPlatform.Application.Abstraction;
using FreelancerPlatform.Application.Abstraction.Service;
using FreelancerPlatform.Application.Extendsions;
using Microsoft.AspNetCore.Mvc;

namespace FreelancerPlatform.Client.Controllers.Components
{
    public class NotifyQuanlityViewComponent : ViewComponent
    {
        private readonly INotificationService _notificationService;
        public NotifyQuanlityViewComponent(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var freelancerId = HttpContext.User.GetUserId();
            var count = await _notificationService.CheckSeenStatusCount(freelancerId);
            return View("Default", count);
        }
    }
}
