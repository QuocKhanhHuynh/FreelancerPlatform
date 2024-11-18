using AutoMapper;
using FreelancerPlatform.Application.Abstraction.Repository;
using FreelancerPlatform.Application.Abstraction.Service;
using FreelancerPlatform.Application.Abstraction.UnitOfWork;
using FreelancerPlatform.Application.Dtos.Chat;
using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.ServiceImplementions.Base;
using FreelancerPlatform.Domain.Entity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.ServiceImplementions
{
    public class ChatService : BaseService<Chat>, IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IHubChatRepository _hubChatRepository;
        private readonly IFreelancerRepository _fareelancerRepository;
        public ChatService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<Chat> logger, IChatRepository chatRepository, IHubChatRepository hubChatRepository,
            IFreelancerRepository freelancerRepository) : base(unitOfWork, mapper, logger)
        {
            _chatRepository = chatRepository;
            _hubChatRepository = hubChatRepository;
            _fareelancerRepository = freelancerRepository;
        }

        public async Task<ServiceResult> CheckAndCreateHubChat(HubChatCreateViewModel request)
        {
            try
            {
                HubChat hubChat = (await _hubChatRepository.GetAllAsync()).FirstOrDefault(x => (x.FreelancerA == request.FreelancerA && x.FreelancerB == request.FreelancerB) || (x.FreelancerA == request.FreelancerB && x.FreelancerB == request.FreelancerA));
                if (hubChat == null)
                {
                    hubChat = new HubChat()
                    {
                        FreelancerA = request.FreelancerA,
                        FreelancerB = request.FreelancerB,
                    };
                    await _hubChatRepository.CreateAsync(hubChat);
                    await _unitOfWork.SaveChangeAsync();
                }


                return new ServiceResultInt()
                {
                    Message = "Kiểm tra thông tin thành công",
                    Status = StatusResult.Success,
                    Result = hubChat.Id
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

        public async Task<int> CountChatUnSeen(int freelancerId)
        {
            var count = 0;
            var hubChats = await _hubChatRepository.GetAllAsync();
            foreach (var hubChat in hubChats)
            {
                if (hubChat.FreelancerA == freelancerId || hubChat.FreelancerB == freelancerId)
                {
                    if (hubChat.FreelancerA == freelancerId)
                    {
                        if (hubChat.SeenStatusA == false)
                        {
                            count++;
                        }
                    }
                    else
                    {
                        if (hubChat.SeenStatusB == false)
                        {
                            count++;
                        }
                    }
                }
            }
            return count;
        }

        public async Task<ServiceResult> CreateChat(ChatCreateRequest request)
        {
            try
            {
                HubChat hubChat = (await _hubChatRepository.GetAllAsync()).FirstOrDefault(x => (x.FreelancerA == request.FreelancerA && x.FreelancerB == request.FreelancerB) || (x.FreelancerA == request.FreelancerB && x.FreelancerB == request.FreelancerA));
                if (hubChat == null)
                {
                    hubChat = new HubChat()
                    {
                        FreelancerA = request.FreelancerA,
                        FreelancerB = request.FreelancerB,
                    };
                    await _hubChatRepository.CreateAsync(hubChat);
                    await _unitOfWork.SaveChangeAsync();
                }
               
                var chat = new Chat()
                {
                    HubChatId = request.HubChatId != null ? request.HubChatId.GetValueOrDefault() : hubChat.Id,
                    Content = request.Content,
                    FreelancerId = request.FreelancerId,
                   
                };
                await _chatRepository.CreateAsync(chat);
                await _unitOfWork.SaveChangeAsync();

                this.UpdateSeenStatus(hubChat.Id, request.FreelancerId);


                return new ServiceResultInt()
                {
                    Message = "Tạo thông tin thành công",
                    Status = StatusResult.Success,
                    Result = chat.HubChatId
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

        public async Task<Dictionary<KeyChatViewModel, List<ChatViewModel>>> GetAllChat(int freelancerId)
        {
            var chatQuery = await _chatRepository.GetAllAsync();
            var freelancerQuery = await _fareelancerRepository.GetAllAsync();
            var hubChats = await _hubChatRepository.GetAllAsync();
            var query = from c in chatQuery
                        join f in freelancerQuery on c.FreelancerId equals f.Id
                        join h in hubChats on c.HubChatId equals h.Id
                        select new {c, f, h};
            var chatOfFreelancer = query.Where(x => x.h.FreelancerA == freelancerId || x.h.FreelancerB == freelancerId);
            var chatByGroup = chatOfFreelancer.GroupBy(x => x.c.HubChatId);
            var result = new Dictionary<KeyChatViewModel, List<ChatViewModel>>();
            foreach ( var chatHub in chatByGroup)
            {
                var key = new KeyChatViewModel();
                key.HubChatId = chatHub.Key;

                var hub = hubChats.FirstOrDefault(x => x.Id == chatHub.Key);
                var freelanerA = freelancerQuery.FirstOrDefault(x => x.Id == hub.FreelancerA);
                key.FirstNameA = freelanerA.FirstName;
                key.LastNameA = freelanerA.LastName;
                key.FreelancerA = hub.FreelancerA;

                var freelanerB = freelancerQuery.FirstOrDefault(x => x.Id == hub.FreelancerB);
                key.FirstNameB = freelanerB.FirstName;
                key.LastNameB = freelanerB.LastName;
                key.FreelancerB = hub.FreelancerB;

                key.ImageUrlA = freelanerA.ImageUrl;
                key.ImageUrlB = freelanerB.ImageUrl;

                key.SeenFreelancerA = hub.SeenStatusA;
                key.SeenFreelancerB = hub.SeenStatusB;

                var chats = new List<ChatViewModel>();
                foreach(var i  in chatHub)
                {
                    var chat = new ChatViewModel()
                    {
                        CreateDay = i.c.CreateDay,
                        Id = i.c.Id,
                        FreelancerId = i.c.FreelancerId,
                        Content = i.c.Content,
                        ImageUrl = i.f.ImageUrl
                    };
                    chats.Add(chat);
                   
                }
                chats = chats.OrderBy(x => x.CreateDay).ToList();
                result[key] = chats;
            }
            var keyOfResult = result.Keys.Select(x => x.HubChatId).ToList();
            foreach(var i in hubChats)
            {
                if (!keyOfResult.Contains(i.Id) && (i.FreelancerA == freelancerId || i.FreelancerB == freelancerId))
                {
                    var key = new KeyChatViewModel();
                    key.HubChatId = i.Id;
                    var freelanerA = freelancerQuery.FirstOrDefault(x => x.Id == i.FreelancerA);
                    key.FirstNameA = freelanerA.FirstName;
                    key.LastNameA = freelanerA.LastName;
                    key.FreelancerA = i.FreelancerA;

                    var freelanerB = freelancerQuery.FirstOrDefault(x => x.Id == i.FreelancerB);
                    key.FirstNameB = freelanerB.FirstName;
                    key.LastNameB = freelanerB.LastName;
                    key.FreelancerB = i.FreelancerB;

                    var chats = new List<ChatViewModel>();
                    result[key] = chats;
                }
            }
            return result;
        }

        public async Task<ServiceResult> RecallInbox(int id)
        {
            try
            {
                Chat chat = await _chatRepository.GetByIdAsync(id);
                chat.Content = "Đã thu hồi";
                chat.CreateUpdate = DateTime.Now;
                _chatRepository.Update(chat);
                await _unitOfWork.SaveChangeAsync();

                return new ServiceResult()
                {
                    Message = "Thu hồi tin thành công",
                    Status = StatusResult.Success,
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

        public async Task<ServiceResult> UpdateAllSeenStatus(int id)
        {
            try
            {
                var hubChat = await _hubChatRepository.GetByIdAsync(id);
               
                hubChat.SeenStatusA = true;
                hubChat.SeenStatusB = true;
                
                _hubChatRepository.Update(hubChat);
                await _unitOfWork.SaveChangeAsync();

                return new ServiceResult()
                {
                    Message = "Cập nhật trạng thái xem thành công",
                    Status = StatusResult.Success,
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

        public async Task<ServiceResult> UpdateSeenStatus(int id, int freelancerId)
        {

            try
            {
                var hubChat = await _hubChatRepository.GetByIdAsync(id);
                if (hubChat.FreelancerA == freelancerId)
                {
                    hubChat.SeenStatusA = true;
                    hubChat.SeenStatusB = false;
                }
                else
                {
                    hubChat.SeenStatusB = true;
                    hubChat.SeenStatusA = false;
                }
                _hubChatRepository.Update(hubChat);
                await _unitOfWork.SaveChangeAsync();

                return new ServiceResult()
                {
                    Message = "Cập nhật trạng thái xem thành công",
                    Status = StatusResult.Success,
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
