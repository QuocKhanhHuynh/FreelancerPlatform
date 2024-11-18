using AutoMapper;
using FreelancerPlatform.Application.Abstraction.Repository;
using FreelancerPlatform.Application.Abstraction.Service;
using FreelancerPlatform.Application.Abstraction.UnitOfWork;
using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.Contract;
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
    public class ContractService : BaseService<Contract>, IContractService
    {
        private readonly IContractRepository _contractRepository;
        private readonly IFreelancerRepository _freelancerRepository;
        public ContractService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<Contract> logger, IContractRepository contractRepository
            , IFreelancerRepository freelancerRepository) : base(unitOfWork, mapper, logger)
        {
            _contractRepository = contractRepository;
            _freelancerRepository = freelancerRepository;
        }

        public async Task<ServiceResult> CreateContract(ContractCreateRequest request)
        {
            try
            {
                Contract project = (await _contractRepository.GetAllAsync()).FirstOrDefault(x => x.ProjectId == request.JobId);
                if (project != null)
                {
                    return new ServiceResult()
                    {
                        Message = "Dự án đã có hợp đồng",
                        Status = StatusResult.ClientError
                    };
                }
                    var contract = new Contract()
                {
                   Content = request.Content,
                   CreateUser = request.CreateUser,
                   Name = request.Name,
                   Partner = request.Partner,
                   ProjectId = request.JobId

                };
                await _contractRepository.CreateAsync(contract);

             
                await _unitOfWork.SaveChangeAsync();
                return new ServiceResult()
                {
                    Message = "Tạo hợp đồng thành công",
                    Status = StatusResult.Success
                };
            }
            catch (Exception ex)
            {
                _unitOfWork.Cancel();
                _logger.LogError(ex.InnerException.Message);

                return new ServiceResult()
                {
                    Message = $"Lỗi hệ thống, ({ex.InnerException.Message})",
                    Status = StatusResult.SystemError
                };
            }
        }

        public async Task<List<ContractQuickViewModel>> GetAllContract()
        {
            var result = (await _contractRepository.GetAllAsync()).Select(x => new ContractQuickViewModel()
            {
                AcceptStatus = x.AcceptStatus,
                ContractStatus = x.ContractStatus,
                CreateDay = x.CreateDay,
                Content = x.Content,
                Id = x.Id,
                Name = x.Name,
                ProjectId = x.ProjectId.GetValueOrDefault()
            }).ToList();
            return result;
        }

        public async Task<ContractViewModel> GetContract(int id)
        {
            var contract = await _contractRepository.GetByIdAsync(id);
            var recruiter = await _freelancerRepository.GetByIdAsync(contract.CreateUser);
            var freelancer = await _freelancerRepository.GetByIdAsync(contract.Partner);

            return new ContractViewModel()
            {
                AcceptStatus = contract.AcceptStatus,
                Content = contract.Content,
                ContractStatus = contract.ContractStatus,
                CreateDay = contract.CreateDay,
                Id = id,
                Name = contract.Name,
                PartnerFirstName = freelancer.FirstName,
                PartnerId = freelancer.Id,
                PartnerImageUrl = freelancer.ImageUrl,
                PartnerLastName = freelancer.LastName,
                RecruiterFirstName = recruiter.FirstName,
                RecruiterId = recruiter.Id,
                RecruiterImageUrl = recruiter.ImageUrl,
                RecruiterLastName = recruiter.LastName,
                PartnerPoint = contract.PartnerPoints.GetValueOrDefault(0),
                PartnerReview = contract.PartnerReview,
            };
        }

        public async Task<List<ContractQuickViewModel>> GetContractOfFreelancer(int id)
        {
            var result = (await _contractRepository.GetAllAsync()).Where(x => x.Partner == id).Select(x => new ContractQuickViewModel()
            {
                AcceptStatus = x.AcceptStatus,
                ContractStatus = x.ContractStatus,
                CreateDay = x.CreateDay,
                Content = x.Content,
                Id = x.Id,
                Name = x.Name
            }).OrderByDescending(x => x.CreateDay).ToList();
            return result;
        }
        
        public async Task<List<ContractQuickViewModel>> GetContractOfRecruiter(int id)
        {
            var result = (await _contractRepository.GetAllAsync()).Where(x => x.CreateUser == id).Select(x => new ContractQuickViewModel()
            {
                AcceptStatus = x.AcceptStatus,
                ContractStatus = x.ContractStatus,
                CreateDay = x.CreateDay,
                Content = x.Content,
                Id = x.Id,
                Name = x.Name,
                ProjectId = x.ProjectId.GetValueOrDefault()
            }).ToList();
            return result;
        }

        public async Task<List<ReviewOfFreelancerQuickViewModel>> GetReviewOfFreelancer(int freelancerId)
        {
            var freelancers = await _freelancerRepository.GetAllAsync();
            var contracts = (await _contractRepository.GetAllAsync()).Where(x => x.Partner == freelancerId && x.PartnerPoints != null);
            var query = from f in freelancers
                        join c in contracts on f.Id equals c.CreateUser
                        select new {f, c};
            return query.Select(x => new ReviewOfFreelancerQuickViewModel()
            {
                CreateDay = x.c.CreateDay,
                FirstName = x.f.FirstName,
                LastName = x.f.LastName,
                Point = x.c.PartnerPoints.GetValueOrDefault(),
                Review = x.c.PartnerReview,
                ImageUrl = x.f.ImageUrl
            }).ToList();
        }

        public async Task<ServiceResult> ReviewPartner(PartnerReviewRequest request)
        {
            try
            {

                var contract = await _contractRepository.GetByIdAsync(request.Id);
                contract.PartnerPoints = request.Point;
                contract.PartnerReview = request.Review;
                _contractRepository.Update(contract);
                await _unitOfWork.SaveChangeAsync();

                await _unitOfWork.SaveChangeAsync();
                return new ServiceResult()
                {
                    Message = "Đánh giá hợp đồngthành công",
                    Status = StatusResult.Success
                };
            }
            catch (Exception ex)
            {
                _unitOfWork.Cancel();
                _logger.LogError(ex.InnerException.Message);

                return new ServiceResult()
                {
                    Message = $"Lỗi hệ thống, ({ex.InnerException.Message})",
                    Status = StatusResult.SystemError
                };
            }
        }

        public async Task<ServiceResult> UpdateContract(ContractUpdateRequest request)
        {
            try
            {

                var contract = await _contractRepository.GetByIdAsync(request.Id);
                contract.Name = request.Name;
                contract.Content = request.Content;
                _contractRepository.Update(contract);
                await _unitOfWork.SaveChangeAsync();

                await _unitOfWork.SaveChangeAsync();
                return new ServiceResult()
                {
                    Message = "Cập nhật hợp đồng thành công",
                    Status = StatusResult.Success
                };
            }
            catch (Exception ex)
            {
                _unitOfWork.Cancel();
                _logger.LogError(ex.InnerException.Message);

                return new ServiceResult()
                {
                    Message = $"Lỗi hệ thống, ({ex.InnerException.Message})",
                    Status = StatusResult.SystemError
                };
            }
        }


        public async Task<ServiceResult> UpdateContractAcceptStatus(int contractId, int status)
        {
            try
            {
                
                var contract = await _contractRepository.GetByIdAsync(contractId);
                contract.AcceptStatus = status;
                _contractRepository.Update(contract);
                await _unitOfWork.SaveChangeAsync();

                await _unitOfWork.SaveChangeAsync();
                return new ServiceResult()
                {
                    Message = "Cập nhật hợp đồng thành công",
                    Status = StatusResult.Success
                };
            }
            catch (Exception ex)
            {
                _unitOfWork.Cancel();
                _logger.LogError(ex.InnerException.Message);

                return new ServiceResult()
                {
                    Message = $"Lỗi hệ thống, ({ex.InnerException.Message})",
                    Status = StatusResult.SystemError
                };
            }
        }

        public async Task<ServiceResult> UpdateContractStatus(int contractId)
        {
            try
            {

                var contract = await _contractRepository.GetByIdAsync(contractId);
                contract.ContractStatus = false;
                _contractRepository.Update(contract);
                await _unitOfWork.SaveChangeAsync();

                await _unitOfWork.SaveChangeAsync();
                return new ServiceResult()
                {
                    Message = "Cập nhật hợp đồng thành công",
                    Status = StatusResult.Success
                };
            }
            catch (Exception ex)
            {
                _unitOfWork.Cancel();
                _logger.LogError(ex.InnerException.Message);

                return new ServiceResult()
                {
                    Message = $"Lỗi hệ thống, ({ex.InnerException.Message})",
                    Status = StatusResult.SystemError
                };
            }
        }
    }
}
