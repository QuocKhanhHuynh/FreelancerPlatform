using FreelancerPlatform.Application.Abstraction;
using FreelancerPlatform.Application.Extendsions;
using Microsoft.AspNetCore.Mvc;

namespace FreelancerPlatform.Client.Controllers.Components
{
    public class NotifyListViewComponent  : ViewComponent
    {
        private readonly INotificationService _notificationService;
        public NotifyListViewComponent(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var freelancerId = HttpContext.User.GetUserId();
            var result = await _notificationService.GetNotificationOfFreelancer(freelancerId);
            return View("Default", result);
        }
    }
}
