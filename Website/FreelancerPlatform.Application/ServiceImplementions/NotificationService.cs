using AutoMapper;
using FreelancerPlatform.Application.Abstraction;
using FreelancerPlatform.Application.Abstraction.Repository;
using FreelancerPlatform.Application.Abstraction.Service;
using FreelancerPlatform.Application.Abstraction.UnitOfWork;
using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.Notification;
using FreelancerPlatform.Application.ServiceImplementions.Base;
using FreelancerPlatform.Domain.Entity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.ServiceImplementions
{
    public class NotificationService : BaseService<Notification>, INotificationService
    {
        private INotificationRepository _notificationRepository;
        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<Notification> logger, INotificationRepository notificationRepository) : base(unitOfWork, mapper, logger)
        {
            _notificationRepository = notificationRepository;
        }
        public async  Task<int> CheckSeenStatusCount(int freelancerId)
        {
            var notifies = (await _notificationRepository.GetAllAsync()).Where(x => x.FreelancerId == freelancerId);
            var result = 0;
            foreach(var i in notifies)
            {
                if(i.SeenStatus == false)
                {
                    result++;
                }
            }
            return result;
        }

        public async Task<ServiceResult> CreateNotification(CreateNotificationRequest request)
        {
            try
            {
                var newNotification = new Notification()
                {
                 Content =  request.Content,
                 FreelancerId = request.FreelancerId,
               
                };
               
                await _notificationRepository.CreateAsync(newNotification);
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

        public async Task<ServiceResult> DeleteNotification(int id)
        {
            try
            {
                var notify = await _notificationRepository.GetByIdAsync(id);
                if (notify == null)
                {
                    return new ServiceResult()
                    {
                        Message = "Không tìm thấy thông tin",
                        Status = StatusResult.ClientError
                    };
                }

                _notificationRepository.Delete(notify);
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

        public async Task<List<NotificationQuickViewModel>> GetNotificationOfFreelancer(int freelancerId)
        {
            var notifies = (await _notificationRepository.GetAllAsync()).Where(x => x.FreelancerId == freelancerId);
            return notifies.Select(x => new NotificationQuickViewModel()
            {
                Content = x.Content,
                FreelancerId = x.FreelancerId,
                Id = x.Id,
                CreateDay = x.CreateDay
            }).OrderByDescending(x => x.CreateDay).ToList();
        }

        public async Task<ServiceResult> UpdateSeenStatus(int freelancerId)
        {
            try
            {
                var notifies = (await _notificationRepository.GetAllAsync()).Where(x => x.FreelancerId == freelancerId);
                foreach (var notification in notifies)
                {
                    notification.SeenStatus = true;
                    _notificationRepository.Update(notification);
                }

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
