using AutoMapper;
using FreelancerPlatform.Application.Abstraction.Repository;
using FreelancerPlatform.Application.Abstraction.Service;
using FreelancerPlatform.Application.Abstraction.UnitOfWork;
using FreelancerPlatform.Application.Dtos.Category;
using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.Freelancer;
using FreelancerPlatform.Application.Dtos.Job;
using FreelancerPlatform.Application.Dtos.Offer;
using FreelancerPlatform.Application.Dtos.Skill;
using FreelancerPlatform.Application.ServiceImplementions.Base;
using FreelancerPlatform.Domain.Constans;
using FreelancerPlatform.Domain.Entity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.ServiceImplementions
{
    public class OfferService : BaseService<Offer>, IOfferService
    {
        private readonly IOfferRepository _offerRepository;
        private readonly IFreelancerRepository _freelancerRepository;
		private readonly IFreelancerCategoryRepository _freelancerCategoryRepository;
		private readonly ITransactionRepository _transactionRepository;
		private readonly ICategoryRepository _categoryRepository;
        private readonly IJobRepository _jobRepository;
        private readonly IApplyRepository _applyRepository;
        private readonly IJobSkillRepository _jobSkillRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly IContractRepository _contractRepository;
        public OfferService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<Offer> logger, IOfferRepository offerRepository, IFreelancerRepository freelancerRepository
            , IFreelancerCategoryRepository freelancerCategoryRepository , IContractRepository contractRepository,
			ITransactionRepository transactionRepository, ICategoryRepository categoryRepository, IJobRepository jobRepository,
            IApplyRepository applyRepository, IJobSkillRepository jobSkillRepository, ISkillRepository skillRepository) : base(unitOfWork, mapper, logger)
        {
            _offerRepository = offerRepository;
            _freelancerRepository = freelancerRepository;
            _freelancerCategoryRepository = freelancerCategoryRepository;
            _transactionRepository = transactionRepository;
            _categoryRepository = categoryRepository;
            _jobRepository = jobRepository;
            _applyRepository = applyRepository;
            _jobSkillRepository = jobSkillRepository;
            _skillRepository = skillRepository;
            _contractRepository = contractRepository;
        }

        public async Task<bool> CheckIsOffer(int jobId, int freelancerId)
        {
            var isOffer = (await _offerRepository.GetAllAsync()).Where(x => x.JobId == jobId && x.FreelancerOfferId == freelancerId).Any();
            return isOffer;
        }

        public async Task<ServiceResult> CreateOfferAsync(OfferCreateRequest request)
        {
            try
            {
                var offer = (await _offerRepository.GetAllAsync()).FirstOrDefault(x => x.FreelancerOfferId == request.FreelancerOfferId && x.JobId == request.JobId );
                if (offer != null)
                {
                    return new ServiceResult()
                    {
                        Message = "Mời  thành công",
                        Status = StatusResult.Success
                    };
                }
                await _offerRepository.CreateAsync(new Offer()
                {
                    FreelancerId = request.FreelancerId,
                    JobId = request.JobId,
                    FreelancerOfferId = request.FreelancerOfferId

                });

                await _unitOfWork.SaveChangeAsync();

                return new ServiceResult()
                {
                    Message = "Mời thành công",
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

        public async Task<ServiceResult> DeleteOfferAsync(int id)
        {
            try
            {
                Offer entity = (await _offerRepository.GetAllAsync()).FirstOrDefault(x => x.Id == id);
                if (entity == null)
                {
                    return new ServiceResult()
                    {
                        Message = "Không tìm thấy thônng tin",
                        Status = StatusResult.ClientError
                    };
                }
                _offerRepository.Delete(entity);
                await _unitOfWork.SaveChangeAsync();

                return new ServiceResult()
                {
                    Message = "Xoá tin thành công",
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

        public async Task<List<OfferQuicckViewModel>> GetAllOffer()
        {
            var offers = (await _offerRepository.GetAllAsync());
            var jobs = (await _jobRepository.GetAllAsync());
            var categories = (await _categoryRepository.GetAllAsync());
            var jobSkills = await _jobSkillRepository.GetAllAsync();
            var skills = await _skillRepository.GetAllAsync();
            var freelancers = await _freelancerRepository.GetAllAsync();
            var contracts = await _contractRepository.GetAllAsync();
            var jobContract = contracts.Select(x => x.ProjectId).ToList();


            var query = from j in jobs
                        join c in categories on j.CategoryId equals c.Id
                        join o in offers on j.Id equals o.JobId
                        join f in freelancers on o.FreelancerOfferId equals f.Id
                        select new { j, c, o, f };

            var queryJobSkill = from j in jobSkills
                                join s in skills on j.SkillId equals s.Id
                                select new { j, s };

            return query.Select(x => new OfferQuicckViewModel
            {
                Id = x.j.Id,
                Category = new CategoryQuickViewModel() { Id = x.c.Id, Name = x.c.Name },

                CreateDay = x.j.CreateDay,
                FreelancerId = x.f.Id,
                MaxDeal = x.j.MaxDeal.GetValueOrDefault(),
                MinDeal = x.j.MinDeal,
                Name = x.j.Name,
                Priority = x.j.Priority,
                Description = x.j.Description,
                JobType = x.j.JobType,
                SalaryType = x.j.SalaryType,
                IsHiden = x.j.IsHiden,
                FirstName = x.f.FirstName,
                ImageUrl = x.f.ImageUrl,
                LastName = x.f.LastName,
                RecruiterId = x.o.FreelancerId,
                OfferId = x.o.Id,
                InContract = jobContract.Contains(x.j.Id) ? true : false,
                Skills = queryJobSkill.Where(y => y.j.JobId == x.j.Id).Select(y => new SkillQuickViewModel()
                {
                    Id = y.s.Id,
                    Name = y.s.Name
                }).ToList()
            }).ToList();
        }

        /*public async Task<ServiceResult> CreateOfferAsync(OfferCreateRequest request)
        {
            try
            {
                var offer = (await _offerRepository.GetAllAsync()).FirstOrDefault(x => !x.IsDeleted && x.JobId == request.JobId && x.FreelancerId == request.FreelancerId && x.RecruiterId == request.RecruiterId);
                if (offer != null)
                {
					return new ServiceResult()
					{
						Message = "Tạo thông tin thành công",
						Status = StatusResult.Success
					};
				}
                await _offerRepository.CreateAsync(new Offer()
                {
                    FreelancerId = request.FreelancerId,
                    JobId = request.JobId,
                    RecruiterId = request.RecruiterId,
                    
                });

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

                return new ServiceResult()
                {
                    Message = $"Lỗi hệ thống, ({ex.InnerException.Message})",
                    Status = StatusResult.SystemError
                };
            }
        }

        public async Task<ServiceResult> DeleteOfferAsync(int id)
        {
            try
            {
                Offer entity = await _offerRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return new ServiceResult()
                    {
                        Message = "Không tìm thấy thônng tin",
                        Status = StatusResult.ClientError
                    };
                }
                if (entity.Status == 1)
                {
                    return new ServiceResult()
                    {
                        Message = "Freelancer đã đồng ý, không thể xóa",
                        Status = StatusResult.ClientError
                    };
                }
                entity.IsDeleted = true;
                _offerRepository.Update(entity);
                await _unitOfWork.SaveChangeAsync();

                return new ServiceResult()
                {
                    Message = "Xoá tin thành công",
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

        public async Task<Pagination<OfferQuickViewModel>> GetAllOfferByRecruiterAsync(int pageIndex, int pageTake, string? keywork = null)
        {

            var query = (await _offerRepository.GetAllAsync()).Where(x => !x.IsDeleted);
            if (keywork != null)
            {
                var freelancer = (await _freelancerRepository.GetAllAsync()).Where(x => x.UserName.Contains(keywork)).Select(x => x.Id);
                query = query.Where(x => freelancer.Contains(x.FreelancerId));
            }
            var pagination = new Pagination<OfferQuickViewModel>();
            pagination.PageIndex = pageIndex;
            pagination.PageSize = pageTake;
            pagination.TotalRecords = query.Count();

            var items = query.Skip((pageIndex - 1) * pageTake).Take(pageTake).Select(x => _mapper.Map<OfferQuickViewModel>(x)).ToList();
            pagination.Items = items;

            return pagination;
        }

        public async Task<Pagination<OfferQuickViewModel>> GetAllOfferByFreelancerAsync(int pageIndex, int pageTake, string? keywork = null)
        {
            var query = (await _offerRepository.GetAllAsync()).Where(x => !x.IsDeleted);
            if (keywork != null)
            {
                var freelancer = (await _recruiterRepository.GetAllAsync()).Where(x => x.UserName.Contains(keywork)).Select(x => x.Id);
                query = query.Where(x => freelancer.Contains(x.FreelancerId));
            }
            var pagination = new Pagination<OfferQuickViewModel>();
            pagination.PageIndex = pageIndex;
            pagination.PageSize = pageTake;
            pagination.TotalRecords = query.Count();

            var items = query.Skip((pageIndex - 1) * pageTake).Take(pageTake).Select(x => _mapper.Map<OfferQuickViewModel>(x)).ToList();
            pagination.Items = items;

            return pagination;
        }

        public async Task<OfferViewModel> GetOfferAsync(int id)
        {
            var offer = await _offerRepository.GetByIdAsync(id);
            var jobs = (await _jobRepository.GetAllAsync()).Where(x => !x.IsDeleted);
            var category = (await _freelancerCategoryRepository.GetAllAsync()).Where(x => x.IsDeleted == false && x.FreelancerId == offer.FreelancerId).Select(x => x.CategoryId).ToList();
            Freelancer entity = await _freelancerRepository.GetByIdAsync(offer.FreelancerId);

            var category1 = (await _freelancerCategoryRepository.GetAllAsync()).Where(x => x.IsDeleted == false && x.FreelancerId == offer.FreelancerId).Select(x => new CategoryQuickViewModel()
            {
                Id = x.CategoryId
            }).ToList();
            var freelancerCategoties = await _freelancerCategoryRepository.GetAllAsync();
            var categories = await _categoryRepository.GetAllAsync();
            var categoryOfUserQuery = from c in categories
                                      join fc in freelancerCategoties on c.Id equals fc.CategoryId
                                      where fc.FreelancerId == offer.FreelancerId
                                      select new { c, fc };

            var categoryOfUser = categoryOfUserQuery.Select(x => new CategoryQuickViewModel()
            {
                Id = x.c.Id,
                Name = x.c.Name,
            }).ToList();

            var contracts = await _contractRepository.GetAllAsync();
            var transactions = await _transactionRepository.GetAllAsync();
            var projects = (await _projectRepository.GetAllAsync()).Where(x => x.FreelancerId == offer.FreelancerId).Select(x => new ProjectViewModel()
            {
                Id = x.Id,
                FreelancerId = x.FreelancerId,
                CreateDay = x.CreateDay,
                CreateUpdate = x.CreateUpdate.GetValueOrDefault(),
                ImageUrl = x.ImageUrl,
                Name = x.Name,
                ProjectUrl = x.ProjectUrl,
                Description = x.Decsription
            }).ToList();

            return new OfferViewModel()
            {

                PhoneNumber = entity.PhoneNumber,
                Email = entity.Email,
                Id = entity.Id,
                Identity = entity.Identity,
                BankNumber = entity.BankNumber,
                About = entity.About,
                BirthDay = entity.BirthDay,
                CreateDay = entity.CreateDay,
                CreateUpdate = entity.CreateDay,
                Experence = entity.Experence,
                FullName = entity.FullName,
                Gender = entity.Gender,
                ImageUrl = entity.ImageUrl,
                IsDelete = entity.IsDeleted,
                NeedVerify = entity.NeedVerify,
                Priority = entity.Priority,
                Skill = entity.Skill,
                Status = entity.Status,
                SystemManagementId = entity.SystemManagementId,
                UserName = entity.UserName,
                Verification = entity.Verification,
                CategoryIds = category,
                Address = entity.Address,
                Categoroies = categoryOfUser,
                AddressName = entity.Address != null ? Constans.Address.Find(y => y.Id == entity.Address).Name : "",
                ContractQuanlity = contracts.Count(y => y.FreelancerId == entity.Id),
                Money = transactions.Sum(y => y.Amount),
                StartQuanlity = (contracts.Where(y => y.FreelancerId == entity.Id).Sum(z => z.Recruiter.GetValueOrDefault()) / (contracts.Where(y => y.FreelancerId == entity.Id).Count(h => h.Recruiter.GetValueOrDefault() > 0) > 0 ? (contracts.Where(y => y.FreelancerId == entity.Id).Count(h => h.Recruiter.GetValueOrDefault() > 0)) : 1)),
                Projects = projects,
                JobId = offer.JobId,
                OfferId = offer.Id,
                StatusAgree = offer.Status,
                JobName = jobs.FirstOrDefault(y => y.Id == offer.JobId).Name
            };
        }

        public async Task<ServiceResult> UpdateofferAsync(int id, OfferUpdateRequest request)
        {
            try
            {
                Offer entity = await _offerRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return new ServiceResult()
                    {
                        Message = "Không tìm thấy thônng tin",
                        Status = StatusResult.ClientError
                    };
                }
                var updateEntity = _mapper.Map<Offer>(request);
                entity.CreateUpdate = DateTime.Now;
                _offerRepository.Update(updateEntity);
                await _unitOfWork.SaveChangeAsync();

                return new ServiceResult()
                {
                    Message = "cập nhật thành công",
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

        public async Task<ServiceResult> UpdateOfferStatusAsync(int id, int status)
        {
            try
            {
                Offer entity = await _offerRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return new ServiceResult()
                    {
                        Message = "Không tìm thấy thônng tin",
                        Status = StatusResult.ClientError
                    };
                }
                entity.Status = status;
                entity.CreateUpdate = DateTime.Now;
                _offerRepository.Update(entity);
                await _unitOfWork.SaveChangeAsync();

                return new ServiceResult()
                {
                    Message = "cập nhật thành công",
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
        public async Task<Pagination<OfferQuickViewModel>> GetOfferOfRecruiterAsync(int id, int pageIndex, int pageTake, string? keyword, int? CategoryId, int? AddressId)
        {
            var categories = await _categoryRepository.GetAllAsync();
            var freelancerCategories = await _freelancerCategoryRepository.GetAllAsync();
            var offerOfRecruiter = (await _offerRepository.GetAllAsync()).Where(x => !x.IsDeleted && x.RecruiterId == id);
            var freelancers = (await _freelancerRepository.GetAllAsync()).Where(x => !x.IsDeleted);
            var query = from o in offerOfRecruiter
                        join f in freelancers on o.FreelancerId equals f.Id
                        select new { o, f };
            if (keyword != null)
            {
                query = query.Where(x => x.f.FullName.Contains(keyword));
            }
            if (AddressId != null)
            {
                query = query.Where(x => x.f.Address == AddressId);
            }
            if (CategoryId != null)
            {
                var freelancerCategory = (await _freelancerCategoryRepository.GetAllAsync()).Where(x => !x.IsDeleted && x.CategoryId == CategoryId).Select(x => x.FreelancerId);
                query = query.Where(x => freelancerCategory.Contains(x.f.Id));
            }
            var pagination = new Pagination<OfferQuickViewModel>();
            pagination.PageIndex = pageIndex;
            pagination.PageSize = pageTake;
            pagination.TotalRecords = query.Count();

            var items = query.Skip((pageIndex - 1) * pageTake).Take(pageTake).Select(x => new OfferQuickViewModel()
            {
                Id = x.o.Id,
                ImageUrl = x.f.ImageUrl,
                StatusAgree = x.o.Status,
                JobId = x.o.JobId,
                Fullname = x.f.FullName,
                CategoryName = categories.Where(y => (freelancerCategories.Where(z => z.FreelancerId == x.f.Id).Select(z => z.CategoryId)).Contains(y.Id)).Select(y => y.Name).ToList(),
                Address = x.f.Address != null ? Constans.Address.Find(y => y.Id == x.f.Address).Name : "",
            }).ToList();
            pagination.Items = items;
            return pagination;
        }

        public async Task<List<FreelancerQuickViewModel>> GetNotFilterAllFreelancerAsync(int id)
        {
			var freelancers = (await _freelancerRepository.GetAllAsync()).Where(x => !x.IsDeleted);
			var potients = (await _offerRepository.GetAllAsync()).Where(x => x.RecruiterId == id && !x.IsDeleted).Select(x => x.FreelancerId);
			var query = freelancers.Where(x => potients.Contains(x.Id));
			var contracts = await _contractRepository.GetAllAsync();
			var transactions = await _transactionRepository.GetAllAsync();
			var freelancerCategories = await _freelancerCategoryRepository.GetAllAsync();
			var categories = await _categoryRepository.GetAllAsync();
			return query.Select(x => new FreelancerQuickViewModel()
			{
				Id = x.Id,
				CreateUpdate = x.CreateUpdate.GetValueOrDefault(),
				FullName = x.FullName,
				Address = x.Address != null ? Constans.Address.Find(y => y.Id == x.Address).Name : "",
				Priority = x.Priority,
				Status = x.Status,
				UserName = x.UserName,
				Verification = x.Verification,
				IsDelete = x.IsDeleted,
				ContractQuanlity = contracts.Count(y => y.FreelancerId == x.Id),
				Money = transactions.Sum(y => y.Amount),
				StartQuanlity = (contracts.Where(y => y.FreelancerId == x.Id).Sum(z => z.Recruiter.GetValueOrDefault()) / (contracts.Where(y => y.FreelancerId == x.Id).Count(h => h.Recruiter.GetValueOrDefault() > 0) > 0 ? (contracts.Where(y => y.FreelancerId == x.Id).Count(h => h.Recruiter.GetValueOrDefault() > 0)) : 1)),
				ImageUrl = x.ImageUrl,
				CategoryName = categories.Where(y => (freelancerCategories.Where(z => z.FreelancerId == x.Id).Select(z => z.CategoryId)).Contains(y.Id)).Select(y => y.Name).ToList()
			}).ToList();
		}

        public async Task<Pagination<OfferQuicckViewModel>> GetOfferOfFreelancerAsync(int id, int pageIndex, int pageTake, string? keyword, int? categoryId, int? addressId, int? startDeal, int? endDeal )
        {
            var offers = (await _offerRepository.GetAllAsync()).Where(x => !x.IsDeleted && x.FreelancerId == id);
            var contracts = await _contractRepository.GetAllAsync();
            var applies = (await _applyRepository.GetAllAsync()).Where(x => !x.IsDeleted);
            var query = (await _jobRepository.GetAllAsync()).Where(x => !x.IsDeleted && x.StartDay < DateTime.Now && !contracts.Select(y => y.JobId).Contains(x.Id));
            var newQuery = from q in query
                           join o in offers on q.Id equals o.JobId
                           select new { q, o };

            if (categoryId != null)
            {
                newQuery = newQuery.Where(x => x.q.CategoryId == categoryId.GetValueOrDefault());
            }
            if (addressId != null)
            {
                var recruiterAdressQuery = (await _recruiterRepository.GetAllAsync()).Where(x => x.Address == addressId).Select(x => x.Id);
                newQuery = newQuery.Where(x => recruiterAdressQuery.Contains(x.q.RecruiterId));
            }
            if (keyword != null)
            {
                newQuery = newQuery.Where(x => x.q.Name.Contains(keyword));
            }
            if (startDeal != null && endDeal != null)
            {
                newQuery = newQuery.Where(x => x.q.StartDeal >= startDeal && x.q.EndDeal <= endDeal);
            }
            var pagination = new Pagination<OfferQuicckViewModel>();
            pagination.PageIndex = pageIndex;
            pagination.PageSize = pageTake;
            pagination.TotalRecords = newQuery.Count();
            var categories = await _categoryRepository.GetAllAsync();
            var recruiter = await _recruiterRepository.GetAllAsync();

            var items = newQuery.Skip((pageIndex - 1) * pageTake).Take(pageTake).Select(x => new OfferQuicckViewModel()
            {
                Id = x.q.Id,
                CreateUpdate = x.q.CreateUpdate.GetValueOrDefault(),
                Name = x.q.Name,
                BussinisName = (recruiter.FirstOrDefault(y => y.Id == x.q.RecruiterId)).Name,
                Address = (Constans.Address.FirstOrDefault(z => recruiter.FirstOrDefault(y => y.Id == x.q.RecruiterId).Address.GetValueOrDefault() == z.Id)) != null ? (Constans.Address.FirstOrDefault(z => recruiter.FirstOrDefault(y => y.Id == x.q.RecruiterId).Address.GetValueOrDefault() == z.Id)).Name : null,
                Priority = x.q.Priority,
                IsDelete = x.q.IsDeleted,
                ImageUrl = (recruiter.FirstOrDefault(y => y.Id == x.q.RecruiterId)).ImageUrl,
                CategoryName = categories.FirstOrDefault(y => y.Id == x.q.CategoryId).Name,
                CategoryId = x.q.CategoryId,
                AppliesNumber = applies.Where(y => y.JobId == x.q.Id).Count(),
                RecruiterId = x.q.RecruiterId,
                StartDeal = x.q.StartDeal,
                EndDeal = x.q.EndDeal,
                StartDay = x.q.StartDay,
                StatusAgree =  x.o.Status
            }).ToList();
            pagination.Items = items;

            return pagination;
        }

        public async Task<List<OfferQuicckViewModel>> GetOfferOfFreelancerNotFilterAsync(int id)
        {
            var offers = (await _offerRepository.GetAllAsync()).Where(x => !x.IsDeleted && x.FreelancerId == id);
            var contracts = await _contractRepository.GetAllAsync();
            var applies = (await _applyRepository.GetAllAsync()).Where(x => !x.IsDeleted);
            var query = (await _jobRepository.GetAllAsync()).Where(x => !x.IsDeleted && x.StartDay < DateTime.Now && !contracts.Select(y => y.JobId).Contains(x.Id));
            var newQuery = from q in query
                           join o in offers on q.Id equals o.JobId
                           select new { q, o };

        
            var categories = await _categoryRepository.GetAllAsync();
            var recruiter = await _recruiterRepository.GetAllAsync();

            var items = newQuery.Select(x => new OfferQuicckViewModel()
            {
                Id = x.q.Id,
                CreateUpdate = x.q.CreateUpdate.GetValueOrDefault(),
                Name = x.q.Name,
                BussinisName = (recruiter.FirstOrDefault(y => y.Id == x.q.RecruiterId)).Name,
                Address = (Constans.Address.FirstOrDefault(z => recruiter.FirstOrDefault(y => y.Id == x.q.RecruiterId).Address.GetValueOrDefault() == z.Id)) != null ? (Constans.Address.FirstOrDefault(z => recruiter.FirstOrDefault(y => y.Id == x.q.RecruiterId).Address.GetValueOrDefault() == z.Id)).Name : null,
                Priority = x.q.Priority,
                IsDelete = x.q.IsDeleted,
                ImageUrl = (recruiter.FirstOrDefault(y => y.Id == x.q.RecruiterId)).ImageUrl,
                CategoryName = categories.FirstOrDefault(y => y.Id == x.q.CategoryId).Name,
                CategoryId = x.q.CategoryId,
                AppliesNumber = applies.Where(y => y.JobId == x.q.Id).Count(),
                RecruiterId = x.q.RecruiterId,
                StartDeal = x.q.StartDeal,
                EndDeal = x.q.EndDeal,
                StartDay = x.q.StartDay,
                StatusAgree = x.o.Status
            }).ToList();

            return items;
        }

		public async  Task<OfferDetailViewModel> GetOfferDetailAsync(int id)
		{
			Job entity = await _jobRepository.GetByIdAsync(id);
			var category = await _categoryRepository.GetAllAsync();
			var applies = (await _applyRepository.GetAllAsync()).Where(x => !x.IsDeleted);
			var recruiters = await _recruiterRepository.GetAllAsync();
			return new OfferDetailViewModel()
			{
				CategoryId = entity.CategoryId,
				Id = entity.Id,
				Description = entity.Description,
				EndDeal = entity.EndDeal,
				Name = entity.Name,
				Priority = entity.Priority,
				RecruiterId = entity.RecruiterId,
				Requirement = entity.Requirement,
				StartDay = entity.StartDay,
				StartDeal = entity.StartDeal,
				SystemManagementId = entity.SystemManagementId,
				CategoryName = category.FirstOrDefault(x => x.Id == entity.CategoryId).Name,
				ApplyNumber = applies.Where(x => x.JobId == id).Count(),
				RecruiterName = recruiters.FirstOrDefault(x => x.Id == entity.RecruiterId).Name
			};
		}

		public async Task<bool> CheckPotientOfRecruiter(int recruiterId, int freelancerId)
		{
			var items = (await _offerRepository.GetAllAsync()).Where(x => x.IsDeleted && x.RecruiterId == recruiterId && x.FreelancerId == freelancerId).ToList();
            return items.Count() > 0 ? true : false;
		}

        public async Task<bool> CheckExisOffer(int freelancerId, int jobId)
        {
            var offer = (await _offerRepository.GetAllAsync()).Where(x => x.FreelancerId == freelancerId && x.JobId == jobId && !x.IsDeleted);
            return offer.Count() > 0;
        }*/
    }
}