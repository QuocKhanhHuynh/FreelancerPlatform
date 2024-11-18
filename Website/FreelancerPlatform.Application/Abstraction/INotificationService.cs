using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Abstraction
{
    public interface INotificationService
    {
        Task<ServiceResult> CreateNotification(CreateNotificationRequest request);
        Task<ServiceResult> UpdateSeenStatus(int freelancerId);
        Task<int> CheckSeenStatusCount(int freelancerId);
        Task<List<NotificationQuickViewModel>> GetNotificationOfFreelancer(int freelancerId);
        Task<ServiceResult> DeleteNotification(int id);
    }
}
