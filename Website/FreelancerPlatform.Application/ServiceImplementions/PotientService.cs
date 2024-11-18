using AutoMapper;
using FreelancerPlatform.Application.Abstraction.Repository;
using FreelancerPlatform.Application.Abstraction.Service;
using FreelancerPlatform.Application.Abstraction.UnitOfWork;
using FreelancerPlatform.Application.Dtos.Category;
using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.Freelancer;
using FreelancerPlatform.Application.Dtos.Offer;
using FreelancerPlatform.Application.Dtos.Potient;
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
	public class PotientService : BaseService<PotentialFreelancer>, IPotientSerivice
	{ 
		private readonly IPotientialFreelancerRepository _potientialFreelancerRepository;
		private readonly IFreelancerRepository _freelancerRepository;
        private readonly IFreelancerCategoryRepository _freelancerCategoryRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly IFreelancerSkilRepository _frequelancerSkilRepository;
        private readonly IContractRepository _contractRepository;
        public PotientService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PotentialFreelancer> logger,
            IPotientialFreelancerRepository potientialFreelancerRepository, IContractRepository contractRepository
, IFreelancerRepository freelancerRepository, IFreelancerCategoryRepository freelancerCategoryRepository , 
            ITransactionRepository transactionRepository, ICategoryRepository categoryRepository,
            ISkillRepository skillRepository, IFreelancerSkilRepository freelancerSkilRepository) : base(unitOfWork, mapper, logger)
        {
            _potientialFreelancerRepository = potientialFreelancerRepository;
            _freelancerRepository = freelancerRepository;
            _transactionRepository = transactionRepository;
            _categoryRepository = categoryRepository;
			_freelancerCategoryRepository = freelancerCategoryRepository;
            _skillRepository = skillRepository;
            _frequelancerSkilRepository = freelancerSkilRepository;
            _contractRepository = contractRepository;
        }

        public async Task<ServiceResult> AddPotientFreelancerAsync(PotientInfor request)
        {
            try
            {
                PotentialFreelancer potient = (await _potientialFreelancerRepository.GetAllAsync()).FirstOrDefault(x => x.FreelancerId == request.FreelancerId && x.FreelancerPotientId == request.FreelancerPotientId);
                if (potient != null)
                {
                    return new ServiceResult()
                    {
                        Message = "Tạo thông tin thành công",
                        Status = StatusResult.Success
                    };
                }
                await _potientialFreelancerRepository.CreateAsync(new PotentialFreelancer()
                {
                    FreelancerId = request.FreelancerId,
                    FreelancerPotientId = request.FreelancerPotientId
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

        public async Task<List<PotientQuickViewModel>> GetAllPotientFreelancerAsync(int id)
        {
            var freelancerCategories = await _freelancerCategoryRepository.GetAllAsync();
            var categories = await _categoryRepository.GetAllAsync();
            var freelancerSkills = await _frequelancerSkilRepository.GetAllAsync();
            var skills = await _skillRepository.GetAllAsync();
            var contracts = await _contractRepository.GetAllAsync();

            var categoryOfFreelancer = from fc in freelancerCategories
                                       join c in categories on fc.CategoryId equals c.Id
                                       select new { c, fc };

            var skillOfFreelancer = from fk in freelancerSkills
                                    join s in skills on fk.SkillId equals s.Id
                                    select new { fk, s };

            var freelancers = await _freelancerRepository.GetAllAsync();
            var potients = (await _potientialFreelancerRepository.GetAllAsync()).Where(x => x.FreelancerId == id);
            var query = from f in freelancers
                        join c in categoryOfFreelancer on f.Id equals c.fc.FreelancerId
                        join s in skillOfFreelancer on f.Id equals s.fk.FreelancerId
                        join    p in potients on f.Id equals p.FreelancerPotientId
                        select new { f, c, s, p };
            query = query.DistinctBy(x => x.f.Id).OrderBy(x => x.p.CreateDay);

            return query.Select(x => new PotientQuickViewModel()
            {
                CreateAddPotient = x.p.CreateDay,
                About = x.f.About,
                FirstName = x.f.FirstName,
                Id = x.f.Id,
                ImageUrl = x.f.ImageUrl,
                RateHour = x.f.RateHour,
                LastName = x.f.LastName,
                Categories = categoryOfFreelancer.Where(y => y.fc.FreelancerId == x.f.Id).Select(y => new CategoryQuickViewModel() { Id = y.c.Id, ImageUrl = y.c.ImageUrl, Name = y.c.Name, }).ToList(),
                Skills = skillOfFreelancer.Where(y => y.fk.FreelancerId == x.f.Id).Select(y => new SkillQuickViewModel() { Id = y.s.Id, Name = y.s.Name }).ToList(),
                Point = (contracts.Where(y => y.Partner == x.f.Id && y.PartnerPoints != null).Sum(y => y.PartnerPoints).GetValueOrDefault() > 0 ? contracts.Where(y => y.Partner == x.f.Id && y.PartnerPoints != null).Sum(y => y.PartnerPoints).GetValueOrDefault() / contracts.Where(y => y.Partner == x.f.Id && y.PartnerPoints != null).Count() : 0),
                ReviewQuanlity = contracts.Where(y => y.Partner == x.f.Id && y.PartnerPoints != null).Count(),
                ContractQuanlity = contracts.Where(y => y.Partner == x.f.Id).Count()
            }).ToList();
        }

        public async Task<List<int>> GetPotientFreelancerAsync(int id)
        {
            var potientOfFreelancers = (await _potientialFreelancerRepository.GetAllAsync()).Where(x => x.FreelancerId == id).Select(x => x.FreelancerPotientId).ToList();
            return potientOfFreelancers;
        }

        public async Task<ServiceResult> RemovePotientFreelancerAsync(PotientInfor request)
        {
            try
            {
                PotentialFreelancer potient = (await _potientialFreelancerRepository.GetAllAsync()).FirstOrDefault(x => x.FreelancerId == request.FreelancerId && x.FreelancerPotientId == request.FreelancerPotientId);
                if (potient == null)
                {
                    return new ServiceResult()
                    {
                        Message = "Không tồn tại thông tin",
                        Status = StatusResult.ClientError
                    };
                }
                _potientialFreelancerRepository.Delete(potient);
                await _unitOfWork.SaveChangeAsync();

                return new ServiceResult()
                {
                    Message = "Xóa thông tin thành công",
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

        /*public async Task<ServiceResult> CreatePotientAsync(PotientCreateRequest request)
		{
			try
			{
			     PotentialFreelancer potient = (await _potientialFreelancerRepository.GetAllAsync()).FirstOrDefault(x => !x.IsDeleted && x.FreelancerId == request.FreelancerId && x.RecruiterId == request.RecruiterId);
                if (potient != null)
                {
					return new ServiceResult()
					{
						Message = "Tạo thông tin thành công",
						Status = StatusResult.Success
					};
				}
				await _potientialFreelancerRepository.CreateAsync(new PotentialFreelancer()
				{
					FreelancerId = request.FreelancerId,
					RecruiterId = request.RecruiterId
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

        public async Task<Pagination<FreelancerQuickViewModel>> GetPotienFreelancerOfUerAsync(int id, int pageIndex, int pageTake, string? keyword, int? CategoryId, int? AddressId)
        {
            var freelancers = (await _freelancerRepository.GetAllAsync()).Where(x => !x.IsDeleted);
            var potients = (await _potientialFreelancerRepository.GetAllAsync()).Where(x => x.RecruiterId == id && !x.IsDeleted).Select(x => x.FreelancerId);
            var query = freelancers.Where(x => potients.Contains(x.Id));
            if (keyword != null)
            {
                query = query.Where(x => x.FullName.Contains(keyword));
            }
            if (AddressId != null)
            {
                query = query.Where(x => x.Address == AddressId);
            }
            if (CategoryId != null)
            {
                var freelancerCategory = (await _freelancerCategoryRepository.GetAllAsync()).Where(x => !x.IsDeleted && x.CategoryId == CategoryId).Select(x => x.FreelancerId);
                query = query.Where(x => freelancerCategory.Contains(x.Id));
            }
            var pagination = new Pagination<FreelancerQuickViewModel>();
            pagination.PageIndex = pageIndex;
            pagination.PageSize = pageTake;
            pagination.TotalRecords = query.Count();
            var contracts = await _contractRepository.GetAllAsync();
            var transactions = await _transactionRepository.GetAllAsync();
            var freelancerCategories = await _freelancerCategoryRepository.GetAllAsync();
            var categories = await _categoryRepository.GetAllAsync();

            var items = query.Skip((pageIndex - 1) * pageTake).Take(pageTake).Select(x => new FreelancerQuickViewModel()
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
            pagination.Items = items;

            return pagination;
        }

        public async Task<List<FreelancerQuickViewModel>> GetNotFilterAllFreelancerAsync(int id)
        {
            var freelancers = (await _freelancerRepository.GetAllAsync()).Where(x => !x.IsDeleted);
            var potients = (await _potientialFreelancerRepository.GetAllAsync()).Where(x => x.RecruiterId == id).Select(x => x.FreelancerId);
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

        public async Task<ServiceResult> DeletePotientAsync(int freelancerId, int recruiterId)
        {
            try
            {
                var entities = (await _potientialFreelancerRepository.GetAllAsync()).Where(x => !x.IsDeleted);
                var entity = entities.FirstOrDefault(x => x.FreelancerId == freelancerId && x.RecruiterId == recruiterId);
                if (entity == null)
                {
                    return new ServiceResult()
                    {
                        Message = "Không tìm thấy thônng tin",
                        Status = StatusResult.ClientError
                    };
                }
                entity.IsDeleted = !entity.IsDeleted;
                _potientialFreelancerRepository.Update(entity);
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

		public async Task<int> CheckPotientOfRecruiter(int recruiterId, int freelancerId)
		{
			var item = (await _potientialFreelancerRepository.GetAllAsync()).FirstOrDefault(x => !x.IsDeleted && x.RecruiterId == recruiterId && x.FreelancerId == freelancerId);
			return item != null? item.Id : 0;
		}*/
    }
}
