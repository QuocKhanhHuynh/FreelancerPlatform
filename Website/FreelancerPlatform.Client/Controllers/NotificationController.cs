using Azure.Core;
using FreelancerPlatform.Application.Abstraction;
using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.Notification;
using FreelancerPlatform.Application.Extendsions;
using Microsoft.AspNetCore.Mvc;

namespace FreelancerPlatform.Client.Controllers
{
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotification(CreateNotificationRequest request)
        {
            var response = await _notificationService.CreateNotification(request);
            if (response.Status != StatusResult.Success)
            {
                return BadRequest();
            }
            return Ok();
        }

        public async Task<IActionResult> CheckSeenStatus()
        {
            var freelancerId = User.GetUserId();
            var response = await _notificationService.CheckSeenStatusCount(freelancerId);
            
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteNotification()
        {
            var freelancerId = User.GetUserId();
            var response = await _notificationService.DeleteNotification(freelancerId);
            if (response.Status != StatusResult.Success)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSeenStatus()
        {
            var freelancerId = User.GetUserId();
            var response = await _notificationService.UpdateSeenStatus(freelancerId);
            if (response.Status != StatusResult.Success)
            {
                return BadRequest();
            }
            return Ok();
        }

        public async Task<IActionResult> LoadListNotify()
        {
            var freelancerId = User.GetUserId();
            var response = await _notificationService.GetNotificationOfFreelancer(freelancerId);

            return Ok(response);
        }
    }
}
