using AutoMapper;
using FreelancerPlatform.Application.Abstraction.Repository;
using FreelancerPlatform.Application.Abstraction.Service;
using FreelancerPlatform.Application.Abstraction.UnitOfWork;
using FreelancerPlatform.Application.Dtos.Category;
using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.FavoriteJob;
using FreelancerPlatform.Application.Dtos.Job;
using FreelancerPlatform.Application.Dtos.Skill;
using FreelancerPlatform.Application.ServiceImplementions.Base;
using FreelancerPlatform.Domain.Constans;
using FreelancerPlatform.Domain.Entity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.ServiceImplementions
{
	public class FavoriteJobService : BaseService<FavoriteJob>, IFavoriteJobService
	{
		private readonly IFavoriteJobRepository _favoriteJobRepository;
        private readonly IJobRepository _jobRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IJobSkillRepository _jobSkillRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly IContractRepository _contractRepository;

        public FavoriteJobService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<FavoriteJob> logger, IFavoriteJobRepository favoriteJobRepository, IContractRepository contractRepository,
            IJobRepository jobRepository, ICategoryRepository categoryRepository, IJobSkillRepository jobSkillRepository, ISkillRepository skillRepository
            ) : base(unitOfWork, mapper, logger)
		{
			_favoriteJobRepository = favoriteJobRepository;
            _jobRepository = jobRepository;
            _categoryRepository = categoryRepository;
            _skillRepository = skillRepository;
            _jobSkillRepository = jobSkillRepository;
            _contractRepository = contractRepository;
		}

        public async Task<ServiceResult> CreateFavoriteJobAsync(FavoriteJobCreateRequest request)
        {
            try
            {
                var offer = (await _favoriteJobRepository.GetAllAsync()).FirstOrDefault(x => x.JobId == request.JobId && x.FreelancerId == request.FreelancerId);
                if (offer != null)
                {
                    return new ServiceResult()
                    {
                        Message = "Tạo thông tin thành công",
                        Status = StatusResult.Success
                    };
                }
                await _favoriteJobRepository.CreateAsync(new FavoriteJob()
                {
                    FreelancerId = request.FreelancerId,
                    JobId = request.JobId

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

        public async Task<ServiceResult> DeleteFavoriteJobAsync(FavoriteJobDeleteRequest request)
        {
            try
            {
                FavoriteJob entity = (await _favoriteJobRepository.GetAllAsync()).FirstOrDefault(x => x.FreelancerId == request.FreelancerId && x.JobId == request.JobId);
                if (entity == null)
                {
                    return new ServiceResult()
                    {
                        Message = "Không tìm thấy thônng tin",
                        Status = StatusResult.ClientError
                    };
                }
                _favoriteJobRepository.Delete(entity);
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

        public async Task<List<int>> GetFavoriteJobOfFreelancerAsync(int id)
        {
            var favoriteJobOfFreelancers = (await _favoriteJobRepository.GetAllAsync()).Where(x => x.FreelancerId == id).Select(x => x.JobId).ToList();
            return favoriteJobOfFreelancers;
        }

        public async Task<List<JobQuickViewModel>> GetFavoriteJobOfFreelancerSecondAsync(int id)
        {
            var jobs = (await _jobRepository.GetAllAsync());
            var categories = (await _categoryRepository.GetAllAsync());
            var jobSkills = await _jobSkillRepository.GetAllAsync();
            var skills = await _skillRepository.GetAllAsync();
            var favoriteJobOfFreelancer = (await _favoriteJobRepository.GetAllAsync()).Where(x => x.FreelancerId == id);

            var contracts = await _contractRepository.GetAllAsync();
            var jobContract = contracts.Select(x => x.ProjectId).ToList();

            var query = from j in jobs
                        join c in categories on j.CategoryId equals c.Id
                        join f in favoriteJobOfFreelancer on j.Id equals f.JobId
                        select new { j, c };

            var queryJobSkill = from j in jobSkills
                                join s in skills on j.SkillId equals s.Id
                                select new { j, s };

            return query.Select(x => new JobQuickViewModel
            {
                Id = x.j.Id,
                Category = new CategoryQuickViewModel() { Id = x.c.Id, Name = x.c.Name },

                CreateDay = x.j.CreateDay,
                FreelancerId = x.j.FreelancerId,
                MaxDeal = x.j.MaxDeal.GetValueOrDefault(),
                MinDeal = x.j.MinDeal,
                Name = x.j.Name,
                Priority = x.j.Priority,
                Description = x.j.Description,
                JobType = x.j.JobType,
                SalaryType = x.j.SalaryType,
                IsHiden = x.j.IsHiden,
                InContract = jobContract.Contains(x.j.Id) ? true : false,
                Skills = queryJobSkill.Where(y => y.j.JobId == x.j.Id).Select(y => new SkillQuickViewModel()
                {
                    Id = y.s.Id,
                    Name = y.s.Name
                }).ToList()
            }).ToList();
        }

        /*public async Task<bool> CheckFavoriteJobAsync(FavoriteJobInfor request)
		{
			var favoriteJob = (await _favoriteJobRepository.GetAllAsync()).FirstOrDefault(x => !x.IsDeleted && x.JobId == request.JobId && x.FreelancerId == request.FreelancerId);
			return favoriteJob != null ? true : false;
		}

		public async Task<ServiceResult> CreateFavoriteJobAsync(FavoriteJobCreateRequest request)
		{
			try
			{
				var offer = (await _favoriteJobRepository.GetAllAsync()).FirstOrDefault(x => x.JobId == request.JobId && x.FreelancerId == request.FreelancerId);
				if (offer != null)
				{
					offer.IsDeleted = false;
					_favoriteJobRepository.Update(offer);
					await _unitOfWork.SaveChangeAsync();
					return new ServiceResult()
					{
						Message = "Tạo thông tin thành công",
						Status = StatusResult.Success
					};
				}
				await _favoriteJobRepository.CreateAsync(new FavoriteJob()
				{
					FreelancerId = request.FreelancerId,
					JobId = request.JobId

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

		public async Task<ServiceResult> DeleteFavoriteJobAsync(FavoriteJobDeleteRequest request)
		{
			try
			{
				FavoriteJob entity = (await _favoriteJobRepository.GetAllAsync()).FirstOrDefault(x => !x.IsDeleted && x.FreelancerId == request.FreelancerId && x.JobId == request.JobId);
				if (entity == null)
				{
					return new ServiceResult()
					{
						Message = "Không tìm thấy thônng tin",
						Status = StatusResult.ClientError
					};
				}
				entity.IsDeleted = true;
				_favoriteJobRepository.Update(entity);
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

        public async Task<Pagination<FavoriteJobQuickViewModel>> GetAllFavoriteJobAsync(int id, int pageIndex, int pageTake, string? keyword, int? categoryId, int? addressId, int? startDeal, int? endDeal)
        {
			var favoriteJob = await _favoriteJobRepository.GetAllAsync();
            var contracts = await _contractRepository.GetAllAsync();
            var applies = (await _applyRepository.GetAllAsync()).Where(x => !x.IsDeleted);
            var query = (await _jobRepository.GetAllAsync()).Where(x => !x.IsDeleted && x.StartDay < DateTime.Now && !contracts.Select(y => y.JobId).Contains(x.Id));
			var newQuery = from q in query
                           join f in favoriteJob on q.Id equals f.JobId
                           where f.IsDeleted == false
                           select new { q, f };

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
            if (startDeal != null)
            {
                query = query.Where(x => startDeal.GetValueOrDefault() >= x.StartDeal);
            }
            if (endDeal != null)
            {
                query = query.Where(x => endDeal.GetValueOrDefault() <= x.EndDeal);
            }
            var pagination = new Pagination<FavoriteJobQuickViewModel>();
            pagination.PageIndex = pageIndex;
            pagination.PageSize = pageTake;
            pagination.TotalRecords = newQuery.Count();
            var categories = await _categoryRepository.GetAllAsync();
            var recruiter = await _recruiterRepository.GetAllAsync();

            var items = newQuery.Skip((pageIndex - 1) * pageTake).Take(pageTake).Select(x => new FavoriteJobQuickViewModel()
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
            }).ToList();
            pagination.Items = items;

            return pagination;
        }

        public async Task<List<FavoriteJobQuickViewModel>> GetAllFavoriteJobNotFillterAsync()
        {
            var favoriteJob = await _favoriteJobRepository.GetAllAsync();
            var contracts = await _contractRepository.GetAllAsync();
            var applies = (await _applyRepository.GetAllAsync()).Where(x => !x.IsDeleted);
            var query = (await _jobRepository.GetAllAsync()).Where(x => !x.IsDeleted && x.StartDay < DateTime.Now && !contracts.Select(y => y.JobId).Contains(x.Id));
            var newQuery = from q in query
                           join f in favoriteJob on q.Id equals f.JobId
                           where f.IsDeleted == false
                           select new { q, f };
           
            var categories = await _categoryRepository.GetAllAsync();
            var recruiter = await _recruiterRepository.GetAllAsync();

            var items = newQuery.Select(x => new FavoriteJobQuickViewModel()
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
            }).ToList();

            return items;
        }*/
    }
}
