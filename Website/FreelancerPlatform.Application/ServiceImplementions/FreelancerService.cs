using AutoMapper;
using FreelancerPlatform.Application.Abstraction.Repository;
using FreelancerPlatform.Application.Abstraction.Service;
using FreelancerPlatform.Application.Abstraction.UnitOfWork;
using FreelancerPlatform.Application.Consts;
using FreelancerPlatform.Application.Dtos.Account;
using FreelancerPlatform.Application.Dtos.Category;
using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.Freelancer;
using FreelancerPlatform.Application.Dtos.Login;
using FreelancerPlatform.Application.Dtos.Skill;
using FreelancerPlatform.Application.Helpers;
using FreelancerPlatform.Application.ServiceImplementions.Base;
using FreelancerPlatform.Domain.Constans;
using FreelancerPlatform.Domain.Entity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace FreelancerPlatform.Application.ServiceImplementions
{
    public class FreelancerService : BaseService<Freelancer>, IFreelancerService
    {
        private readonly IFreelancerRepository _freelancerRepository;
        private readonly IFreelancerCategoryRepository _freelancerCategoryRepository;
        //private readonly ITransactionRepository _transactionRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly IFreelancerSkilRepository _freelancerSkilRepository;
        private readonly IContractRepository _contractRepository;
		public FreelancerService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<Freelancer> logger,
            IFreelancerRepository freelancerRepository, IFreelancerCategoryRepository freelancerCategoryRepository, IContractRepository contractRepository,
            ICategoryRepository categoryRepository, ISkillRepository skillRepository, IFreelancerSkilRepository freelancerSkilRepository) : base(unitOfWork, mapper, logger)
        {
            _freelancerRepository = freelancerRepository;
            _freelancerCategoryRepository = freelancerCategoryRepository;
            //_transactionRepository = transactionRepository;
            _categoryRepository = categoryRepository;
            _skillRepository = skillRepository;
            _freelancerSkilRepository = freelancerSkilRepository;
            _contractRepository = contractRepository;
        }

        public async Task<List<FreelancerQuickViewModel>> GetAllFreelancerAsync()
        {
            var freelancerCategories = await _freelancerCategoryRepository.GetAllAsync();
            var categories = await _categoryRepository.GetAllAsync();
            var freelancerSkills = await _freelancerSkilRepository.GetAllAsync();
            var skills = await _skillRepository.GetAllAsync();
            var contracts = await _contractRepository.GetAllAsync();

            var categoryOfFreelancer = from fc in freelancerCategories
                                       join c in categories on fc.CategoryId equals c.Id
                                       select new { c, fc };

            var skillOfFreelancer = from fk in freelancerSkills
                                    join s in skills on fk.SkillId equals s.Id
                                    select new { fk, s };

            var freelancers = await _freelancerRepository.GetAllAsync();
            var query = from f in freelancers
                        join c in categoryOfFreelancer on f.Id equals c.fc.FreelancerId
                        join s in skillOfFreelancer on f.Id equals s.fk.FreelancerId
                        select new { f, c, s };
            query = query.DistinctBy(x => x.f.Id).OrderBy(x => x.f.CreateDay);

            return query.Select(x => new FreelancerQuickViewModel()
            {
                About = x.f.About,
                FirstName = x.f.FirstName,
                Id = x.f.Id,
                ImageUrl = x.f.ImageUrl,
                RateHour = x.f.RateHour,
                LastName = x.f.LastName,
                Categories = categoryOfFreelancer.Where(y => y.fc.FreelancerId == x.f.Id).Select(y => new CategoryQuickViewModel() { Id = y.c.Id, ImageUrl = y.c.ImageUrl, Name = y.c.Name, }).ToList(),
                Skills = skillOfFreelancer.Where(y => y.fk.FreelancerId == x.f.Id).Select(y => new SkillQuickViewModel() { Id = y.s.Id, Name = y.s.Name }).ToList(),
                Point = (contracts.Where(y => y.Partner == x.f.Id && y.PartnerPoints != null).Sum(y => y.PartnerPoints).GetValueOrDefault() > 0? contracts.Where(y => y.Partner == x.f.Id && y.PartnerPoints != null).Sum(y => y.PartnerPoints).GetValueOrDefault() / contracts.Where(y => y.Partner == x.f.Id && y.PartnerPoints != null).Count() : 0),
                ReviewQuanlity = contracts.Where(y => y.Partner == x.f.Id && y.PartnerPoints != null).Count(),
                ContractQuanlity = contracts.Where(y => y.Partner == x.f.Id).Count(),
                Status = x.f.Status,
                CreateDay = x.f.CreateDay
            }).ToList();
        }

        public async  Task<List<PaymentConfirmViewModel>> GetFeelancerVerifyPayment()
        {

            var freelancers = (await _freelancerRepository.GetAllAsync()).Where(x => x.PaymentVerification == false);

            return freelancers.Select(x => new PaymentConfirmViewModel()
            {
                FirstName = x.FirstName,
                Id = x.Id,
                LastName = x.LastName,
                UpdateDay = x.CreateUpdate.GetValueOrDefault(),
                BankName = x.BankName,
                BankNumber = x.BankNumber,
                VerifyStatus = x.PaymentVerification
            }).OrderByDescending(x => x.UpdateDay).ToList();
        }

        public async Task<ServiceResult> UpdateVerifyPayment(int id, bool statusVerify)
        {
            try
            {
                var entities = await _freelancerRepository.GetByIdAsync(id);
                if (entities == null)
                {
                    return new ServiceResult()
                    {
                        Status = StatusResult.ClientError,
                        Message = "Không tìm thấy thông tin"
                    };
                }
                entities.PaymentVerification = statusVerify;
                _freelancerRepository.Update(entities);
                await _unitOfWork.SaveChangeAsync();

                return new ServiceResult()
                {
                    Message = "Cập nhật thành công",
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

        public async Task<FreelancerViewModel> GetFreelancerAsync(int id)
        {
           var freelancer = await _freelancerRepository.GetByIdAsync(id);
           var freelancerCategories = await _freelancerCategoryRepository.GetAllAsync();
           var categories = await _categoryRepository.GetAllAsync();
           var freelancerSkills = await _freelancerSkilRepository.GetAllAsync();
           var skills = await _skillRepository.GetAllAsync();
            var contracts = (await _contractRepository.GetAllAsync()).Where(x => x.Partner == freelancer.Id && x.PartnerPoints != null);
            var contractQuanlity = (await _contractRepository.GetAllAsync()).Where(x => x.Partner == freelancer.Id).Count();

            var categoryOfFreelancer = from fc in freelancerCategories
                                           join c in categories on fc.CategoryId equals c.Id
                                           where fc.FreelancerId == id
                                           select c;

            var skillOfFreelancer = from fk in freelancerSkills
                                           join s in skills on fk.SkillId equals s.Id
                                           where fk.FreelancerId == id
                                           select s;
            return new FreelancerViewModel()
            {
                About = freelancer.About,
                BankName = freelancer.BankName,
                BankNumber = freelancer.BankNumber,
                Categories = categoryOfFreelancer.Select(x => new CategoryQuickViewModel() { Id = x.Id, Name = x.Name}).ToList(),
                Email = freelancer.Email,
                FirstName = freelancer.FirstName,
                Id = freelancer.Id,
                ImageUrl = freelancer.ImageUrl,
                LastName = freelancer.LastName,
                PaymentVerification = freelancer.PaymentVerification,
                RateHour = freelancer.RateHour,
                Skills = skillOfFreelancer.Select(x => new SkillQuickViewModel() { Id = x.Id, Name = x.Name }).ToList(),
                Point = (contracts.Sum(y => y.PartnerPoints).GetValueOrDefault() > 0 ? contracts.Sum(y => y.PartnerPoints).GetValueOrDefault() / contracts.Count() : 0),
                ReviewQuanlity = contracts.Count(),
                ContractQuanlity = contractQuanlity,
                Archive = freelancer.Archive,
                Certification = freelancer.Certification,
                Education = freelancer.Education,
                Experience = freelancer.Experience
            };
        }

        public async Task<ServiceResult> LockAcountAsync(int adminId)
        {
            try
            {
                var entities = await _freelancerRepository.GetByIdAsync(adminId);
                if (entities == null)
                {
                    return new ServiceResult()
                    {
                        Status = StatusResult.ClientError,
                        Message = "Không tìm thấy thông tin"
                    };
                }
                entities.Status = true;
                _freelancerRepository.Update(entities);
                await _unitOfWork.SaveChangeAsync();

                return new ServiceResult()
                {
                    Message = "Cập nhật thành công",
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



        /*public async Task<ServiceResult> CreateFreelancerAsync(FreelancerCreateRequest request)
        {
            try
            {
                var entity = _mapper.Map<Freelancer>(request);
               
                if (request.CategoryIds != null)
                {
                    entity.FreelancerCategories = new List<FreelancerCategory>();
                    foreach(var categoryId in request.CategoryIds)
                    {
                        entity.FreelancerCategories.Add(new FreelancerCategory()
                        {
                            CategoryId = categoryId
                        });
                    }
                }
                await _freelancerRepository.CreateAsync(entity);
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

        public async Task<ServiceResult> DeleteAccountAsync(int id)
        {
            try
            {
                Freelancer entity = await _freelancerRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return new ServiceResult()
                    {
                        Message = "Không tìm thấy thônng tin",
                        Status = StatusResult.ClientError
                    };
                }
                entity.IsDeleted = true;
                entity.CreateUpdate = DateTime.Now;
                _freelancerRepository.Update(entity);
                var categories = ( await _freelancerCategoryRepository.GetAllAsync()).Where(x => x.FreelancerId.Equals(id));
                foreach (var category in categories)
                {
                    category.IsDeleted = true;
                    _freelancerCategoryRepository.Update(category);
                }
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

        public async Task<Pagination<FreelancerQuickViewModel>> GetAllFreelancerAsync(int pageIndex, int pageTake, int? categoryId, int? addressId, string? keywork = null)
        {
            var query = (await _freelancerRepository.GetAllAsync()).Where(x => !x.IsDeleted);
            if (categoryId != null)
            {
                var freelancerOfCategory = (await _freelancerCategoryRepository.GetAllAsync()).Where(x => x.CategoryId == categoryId).Select(x => x.FreelancerId);
                query = query.Where(x => freelancerOfCategory.Contains(x.Id));
            }
            if (addressId != null)
            {
                query = query.Where(x => x.Address == addressId);
            }
            if (keywork != null)
            {
                query = query.Where(x => x.FullName.Contains(keywork));
            }
            var pagination = new Pagination<FreelancerQuickViewModel>();
            pagination.PageIndex = pageIndex;
            pagination.PageSize = pageTake;
            pagination.TotalRecords = query.Count();
            var contracts = await _contractRepository.GetAllAsync();
            var transactions = await _transactionRepository.GetAllAsync();
            var freelancerCategories = await _freelancerCategoryRepository.GetAllAsync(); 
            var categories = await _categoryRepository.GetAllAsync();

            var items = query.Skip((pageIndex -1) * pageTake).Take(pageTake).Select(x => new FreelancerQuickViewModel()
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
                StartQuanlity = (contracts.Where(y => y.FreelancerId == x.Id).Sum(z => z.Recruiter.GetValueOrDefault())/ (contracts.Where(y => y.FreelancerId == x.Id).Count(h => h.Recruiter.GetValueOrDefault() > 0) > 0 ? (contracts.Where(y => y.FreelancerId == x.Id).Count(h => h.Recruiter.GetValueOrDefault() > 0)) : 1)),
                ImageUrl = x.ImageUrl,
                CategoryName = categories.Where(y => (freelancerCategories.Where(z => z.FreelancerId == x.Id).Select(z => z.CategoryId)).Contains(y.Id)).Select(y => y.Name).ToList()
            }).ToList();
            pagination.Items = items;

            return pagination;
        }

        public async Task<FreelancerViewModel> GetFreelancerAsync(int id)
        {
            var category = (await _freelancerCategoryRepository.GetAllAsync()).Where(x => x.IsDeleted == false && x.FreelancerId == id).Select(x => x.CategoryId).ToList();
            Freelancer entity = await _freelancerRepository.GetByIdAsync(id);

			var category1 = (await _freelancerCategoryRepository.GetAllAsync()).Where(x => x.IsDeleted == false && x.FreelancerId == id).Select(x => new CategoryQuickViewModel()
			{
				Id = x.CategoryId
			}).ToList();
			var freelancerCategoties = await _freelancerCategoryRepository.GetAllAsync();
			var categories = await _categoryRepository.GetAllAsync();
			var categoryOfUserQuery = from c in categories
								 join fc in freelancerCategoties on c.Id equals fc.CategoryId
								 where fc.FreelancerId == id
								 select new { c, fc };

            var categoryOfUser = categoryOfUserQuery.Select(x => new CategoryQuickViewModel()
            {
                Id = x.c.Id,
                Name = x.c.Name,
            }).ToList();

			var contracts = await _contractRepository.GetAllAsync();
			var transactions = await _transactionRepository.GetAllAsync();
            var projects = (await _projectRepository.GetAllAsync()).Where(x => x.FreelancerId == id).Select(x => new ProjectViewModel()
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

			return new FreelancerViewModel()
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
                Projects = projects
			
            };
        }

		/*
         var category1 = (await _freelancerCategoryRepository.GetAllAsync()).Where(x => x.IsDeleted == false && x.FreelancerId == id).Select(x => new CategoryQuickViewModel()
            {
                Id = x.CategoryId
            }).ToList();
			var freelancerCategoties = await _freelancerCategoryRepository.GetAllAsync();
			var categories = await _categoryRepository.GetAllAsync();
			var categoryOfUser = from c in categories
								 join fc in freelancerCategoties on c.Id equals fc.CategoryId
								 where fc.FreelancerId == id
								 select new { c, fc };
			Freelancer entity = await _freelancerRepository.GetByIdAsync(id);
			return new FreelancerViewModel()
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
				Categoroies = categoryOfUser.Select(x => new CategoryQuickViewModel() { Id = x.c.Id, Name = x.c.Name }).ToList(),
				AddressId = entity.Address,
				CategoryIds = category
			};
			Freelancer entity = await _freelancerRepository.GetByIdAsync(id);
            return new FreelancerViewModel()
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
                AddressId = entity.Address,
                
            }; 
        
         */

        /*public async Task<Pagination<FreelancerQuickViewModel>> GetIdentityUpdateFreelancerAsync(int pageIndex, int pageTake, string? keywork = null)
        {
            var query = (await _freelancerRepository.GetAllAsync()).Where(x => !x.IsDeleted && x.NeedVerify == true);
            if (keywork != null)
            {
                query = query.Where(x => x.UserName.Equals(keywork));
            }
            var pagination = new Pagination<FreelancerQuickViewModel>();
            pagination.PageIndex = pageIndex;
            pagination.PageSize = pageTake;
            pagination.TotalRecords = query.Count();

            var items = query.Skip((pageIndex - 1) * pageTake).Take(pageTake).Select(x => _mapper.Map<FreelancerQuickViewModel>(x)).ToList();
            pagination.Items = items;

            return pagination;
        }

        public async Task<List<FreelancerQuickViewModel>> GetNotFilterAllFreelancerAsync()
        {
            var query = (await _freelancerRepository.GetAllAsync()).Where(x => !x.IsDeleted);
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
                StartQuanlity = (contracts.Where(y => y.FreelancerId == x.Id).Sum(z => z.FreelancerReview.GetValueOrDefault()) / (contracts.Where(y => y.FreelancerId == x.Id).Count(h => h.FreelancerReview.GetValueOrDefault() > 0) > 0 ? (contracts.Where(y => y.FreelancerId == x.Id).Count(h => h.FreelancerReview.GetValueOrDefault() > 0)) : 1)),
                ImageUrl = x.ImageUrl,
                CategoryName = categories.Where(y => (freelancerCategories.Where(z => z.FreelancerId == x.Id).Select(z => z.CategoryId)).Contains(y.Id)).Select(y => y.Name).ToList()
            }).ToList();
        }

        public async Task<ServiceResult> LockAccountAsync(int id)
        {
            try
            {
                Freelancer entity = await _freelancerRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return new ServiceResult()
                    {
                        Message = "Không tìm thấy thônng tin",
                        Status = StatusResult.ClientError
                    };
                }
                entity.Status = !entity.Status;
                entity.CreateUpdate = DateTime.Now;
                _freelancerRepository.Update(entity);
                await _unitOfWork.SaveChangeAsync();

                return new ServiceResult()
                {
                    Message = "Khóa tài khoản thành công",
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
        }*/

        public async Task<LoginResult> LoginAsync(LoginRequest request)
        {
            try
            {
                var entities = await _freelancerRepository.GetAllAsync();
                var freelancer = entities.FirstOrDefault(x => x.Email.Equals(request.Email));
                if (freelancer == null)
                {
                    return new LoginResult()
                    {
                        Message = "Không tìm thấy thônng tin email",
                        Status = StatusResult.ClientError
                    };
                }
                if (freelancer.Status)
                {
                    return new LoginResult()
                    {
                        Message = "Tài khoản đã bị khóa",
                        Status = StatusResult.ClientError
                    };
                }
                var verifyPassword = PasswordHelper.VerifyPassword(freelancer.Password, request.Password);
                if (!verifyPassword)
                {
                    return new LoginResult()
                    {
                        Message = "Sai mật khẩu",
                        Status = StatusResult.ClientError
                    };
                }

                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier,freelancer.Id.ToString()),
                    new Claim(ClaimTypes.Role, Const.Freelancer),
                    new Claim("firstName", freelancer.FirstName)
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourStrongSecretKey1234567890123456"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                        "khanhhuynh.paptech",
                        "khanhhuynh.paptech",
                        claims,
                        expires: DateTime.Now.AddMinutes(60),
                        signingCredentials: creds
                    );
                return new LoginResult()
                {
                    Status = StatusResult.Success,
                    Message = "Đăng nhập thành công",
                    Token = new JwtSecurityTokenHandler().WriteToken(token)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.Message);

                return new LoginResult()
                {
                    Message = $"Lỗi hệ thống, ({ex.InnerException.Message})",
                    Status = StatusResult.SystemError
                };
            }
        }

        public async Task<ServiceResult> RegisterAccountAsync(AccountRegisterRequest request)
        {
            try
            {
                var entities = await _freelancerRepository.GetAllAsync();
                var freelancerExis = entities.FirstOrDefault(x => x.Email.Equals(request.Email));
                if (freelancerExis != null)
                {
                    return new ServiceResult()
                    {
                        Status = StatusResult.ClientError,
                        Message = "Email đã tồn tại, vui lòng chọn email khác"
                    };
                }
                var account = new Freelancer();
                var passwordHasher = PasswordHelper.HashPassword(request.Password);
                account.Email = request.Email;
                account.FirstName = request.FirstName;
                account.LastName = request.LastName;
                account.Password = passwordHasher;
                account.CreateUpdate = DateTime.Now;
                await _freelancerRepository.CreateAsync(account);
                await _unitOfWork.SaveChangeAsync(); 

                return new ServiceResult()
                {
                    Message = "Đăng ký thành công",
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

        public async Task<ServiceResult> UnLockAcountAsync(int adminId)
        {
            try
            {
                var entities = await _freelancerRepository.GetByIdAsync(adminId);
                if (entities == null)
                {
                    return new ServiceResult()
                    {
                        Status = StatusResult.ClientError,
                        Message = "Không tìm thấy thông tin"
                    };
                }
                entities.Status = false;
                _freelancerRepository.Update(entities);
                await _unitOfWork.SaveChangeAsync();

                return new ServiceResult()
                {
                    Message = "Cập nhật thành công",
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

        public async Task<ServiceResult> UpdateFreelancerAsync(int id, FreelancerUpdateRequest request)
        {
            try
            {
                Freelancer entity = await _freelancerRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return new ServiceResult()
                    {
                        Message = "Không tìm thấy thônng tin",
                        Status = StatusResult.ClientError
                    };
                }
                entity.About = request.About;
                entity.RateHour = request.RateHour;
                entity.LastName = request.LastName;
                entity.FirstName = request.FirstName;
                entity.ImageUrl = request.ImageUrl != null? request.ImageUrl : entity.ImageUrl;
                if (request.Experience != null && request.Experience.Trim() != "")
                {
                    entity.Experience = request.Experience.Trim();
                }
                if (request.Education != null && request.Education.Trim() != "")
                {
                    entity.Experience = request.Education.Trim();
                }
                if (request.Certification != null && request.Certification.Trim() != "")
                {
                    entity.Certification = request.Certification.Trim();
                }
                if (request.Archive != null && request.Archive.Trim() != "")
                {
                    entity.Archive = request.Archive.Trim();
                }
                _freelancerRepository.Update(entity);

                var categoryIdOfFreelancer = (await _freelancerCategoryRepository.GetAllAsync()).Where(x => x.FreelancerId == id);
                foreach(var i in categoryIdOfFreelancer)
                {
                    _freelancerCategoryRepository.Delete(i);
                }
                foreach(var i in request.CategoryIds)
                {
                    await _freelancerCategoryRepository.CreateAsync(new FreelancerCategory()
                    {
                        CategoryId = i,
                        FreelancerId = id
                    });
                }

                var skillIdOfFreelancer = (await _freelancerSkilRepository.GetAllAsync()).Where(x => x.FreelancerId == id);
                foreach (var i in skillIdOfFreelancer)
                {
                    _freelancerSkilRepository.Delete(i);
                }
                foreach (var i in request.SkillIds)
                {
                    await _freelancerSkilRepository.CreateAsync(new FreelancerSkill()
                    {
                        SkillId = i,
                        FreelancerId = id
                    });
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

                return new ServiceResult()
                {
                    Message = $"Lỗi hệ thống, ({ex.InnerException.Message})",
                    Status = StatusResult.SystemError
                };
            }
        }

        public async Task<ServiceResult> UpdatePasswordAsync(int id, PasswordUpdateRequest request)
        {
            try
            {
                Freelancer entity = await _freelancerRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return new ServiceResult()
                    {
                        Message = "Không tìm thấy thônng tin",
                        Status = StatusResult.ClientError
                    };
                }
                var verify = PasswordHelper.VerifyPassword(entity.Password, request.CurrentPassword);
                if (!verify)
                {
                    return new ServiceResult()
                    {
                        Message = "Sai mật khẩu hiện tại",
                        Status = StatusResult.ClientError
                    };
                }
                if (!request.NewPassword.Equals(request.ConfirmPassword))
                {
                    return new ServiceResult()
                    {
                        Message = "Sai mật khẩu xác nhận",
                        Status = StatusResult.ClientError
                    };
                }
                var passwordHasher = PasswordHelper.HashPassword(request.NewPassword);
                entity.Password = passwordHasher;
                entity.CreateUpdate = DateTime.Now;
                _freelancerRepository.Update(entity);
                await _unitOfWork.SaveChangeAsync();

                return new ServiceResult()
                {
                    Message = "Cập nhật mật khẩu thành công",
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

        public async Task<ServiceResult> UpdatePaymentAsync(int id, FreelancerPaymentUpdateRequest request)
        {
            try
            {
                Freelancer entity = await _freelancerRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return new ServiceResult()
                    {
                        Message = "Không tìm thấy thônng tin",
                        Status = StatusResult.ClientError
                    };
                }
                entity.BankName = request.BankName;
                entity.BankNumber = request.BankNumber;
                entity.PaymentVerification = false;
                _freelancerRepository.Update(entity);


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

                return new ServiceResult()
                {
                    Message = $"Lỗi hệ thống, ({ex.InnerException.Message})",
                    Status = StatusResult.SystemError
                };
            }
        }

        public async Task<bool> CheckExistFreelancerByEmail(string email)
        {
            var freelancer = (await _freelancerRepository.GetAllAsync()).FirstOrDefault(x => x.Email.Equals(email));
            return freelancer != null;
        }

        public async Task<FreelancerViewModel> GetFreelancerByEmailAsync(string email)
        {
            var freelancer = (await _freelancerRepository.GetAllAsync()).FirstOrDefault(x => x.Email.Equals(email));
            return new FreelancerViewModel()
            {
                Id = freelancer.Id,
                Email = freelancer.Email,
                FirstName = freelancer.FirstName
            };
        }

        public async Task<ServiceResult> RegisterAccountByGoogleAsync(AccountRegisterByGoogleRequest request)
        {
            try
            {
                var entities = await _freelancerRepository.GetAllAsync();
                var freelancerExis = entities.FirstOrDefault(x => x.Email.Equals(request.Email));
                if (freelancerExis != null)
                {
                    return new ServiceResult()
                    {
                        Status = StatusResult.ClientError,
                        Message = "Email đã tồn tại, vui lòng chọn email khác"
                    };
                }
                var account = new Freelancer();
                account.Email = request.Email;
                account.FirstName = request.FirstName;
                account.LastName = request.LastName;
                account.CreateUpdate = DateTime.Now;
                await _freelancerRepository.CreateAsync(account);
                await _unitOfWork.SaveChangeAsync();

                return new ServiceResult()
                {
                    Message = "Đăng ký thành công",
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

        public async Task<List<FreelancerQuickViewModel>> GetFreelancerRecommendation(int freelancerId)
        {
            var freelancers = await _freelancerRepository.GetAllAsync();
            var freelancerSkills = await _freelancerSkilRepository.GetAllAsync();
            var contracts = await _contractRepository.GetAllAsync();
            var freelancerCategories = await _freelancerCategoryRepository.GetAllAsync();
            var categories = await _categoryRepository.GetAllAsync();
            var skills = await _skillRepository.GetAllAsync();
            var categoryOfFreelancer = from fc in freelancerCategories
                                       join c in categories on fc.CategoryId equals c.Id
                                       select new { c, fc };
            var skillOfFreelancer = from fk in freelancerSkills
                                    join s in skills on fk.SkillId equals s.Id
                                    select new { fk, s };

            var sharedSkillsCount = freelancerSkills
                .Where(f => f.FreelancerId != freelancerId &&
                            freelancerSkills
                                .Where(fs => fs.FreelancerId == freelancerId)
                                .Select(fs => fs.SkillId)
                                .Contains(f.SkillId))
                .GroupBy(f => f.FreelancerId)
                .Select(g => new
                {
                    FreelancerId = g.Key,
                    SharedSkills = g.Count()
                }).Where(g => g.SharedSkills > 0)
                .OrderByDescending(g => g.SharedSkills)
                .Take(1);

            return freelancers.Where(x => sharedSkillsCount.Select(y => y.FreelancerId).Contains(x.Id)).Select(x => new FreelancerQuickViewModel()
            {
                About = x.About,
                FirstName = x.FirstName,
                Id = x.Id,
                ImageUrl = x.ImageUrl,
                RateHour = x.RateHour,
                LastName = x.LastName,
                Categories = categoryOfFreelancer.Where(y => y.fc.FreelancerId == x.Id).Select(y => new CategoryQuickViewModel() { Id = y.c.Id, ImageUrl = y.c.ImageUrl, Name = y.c.Name, }).ToList(),
                Skills = skillOfFreelancer.Where(y => y.fk.FreelancerId == x.Id).Select(y => new SkillQuickViewModel() { Id = y.s.Id, Name = y.s.Name }).ToList(),
                Point = (contracts.Where(y => y.Partner == x.Id && y.PartnerPoints != null).Sum(y => y.PartnerPoints).GetValueOrDefault() > 0 ? contracts.Where(y => y.Partner == x.Id && y.PartnerPoints != null).Sum(y => y.PartnerPoints).GetValueOrDefault() / contracts.Where(y => y.Partner == x.Id && y.PartnerPoints != null).Count() : 0),
                ReviewQuanlity = contracts.Where(y => y.Partner == x.Id && y.PartnerPoints != null).Count(),
                ContractQuanlity = contracts.Where(y => y.Partner == x.Id).Count(),
                Status = x.Status,
                CreateDay = x.CreateDay

            }).ToList();

        }

        /*public async Task<ServiceResult> UpdateAboutAsync(int id, FreelancerAboutUpdateRequest request)
		{
			try
			{
				Freelancer entity = await _freelancerRepository.GetByIdAsync(id);
				if (entity == null)
				{
					return new ServiceResult()
					{
						Message = "Không tìm thấy thônng tin",
						Status = StatusResult.ClientError
					};
				}
				entity.About = request.About;
				_freelancerRepository.Update(entity);
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

				return new ServiceResult()
				{
					Message = $"Lỗi hệ thống, ({ex.InnerException.Message})",
					Status = StatusResult.SystemError
				};
			}
		}

		public async Task<ServiceResult> UpdateFreelancerAsync(int id, FreelancerUpdateRequest request)
        {
            try
            {
                Freelancer entity = await _freelancerRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return new ServiceResult()
                    {
                        Message = "Không tìm thấy thônng tin",
                        Status = StatusResult.ClientError
                    };
                }
                // entity = _mapper.Map<Freelancer>(request);
                entity.FullName = request.FullName ?? entity.FullName;
				entity.BirthDay = request.BirthDay ?? entity.BirthDay;
				entity.Gender = request.Gender ?? entity.Gender;
				entity.Experence = request.Experence ?? entity.Experence;
				entity.Skill = request.Skill ?? entity.Skill;
				entity.ImageUrl = request.ImageUrl != null ? request.ImageUrl : null;
                entity.Address = request.Address ?? entity.Address;



				entity.CreateUpdate = DateTime.Now;
                _freelancerRepository.Update(entity);
                if (request.CategoryIds != null)
                {
                    var categoryIdList = (await _categoryRepository.GetAllAsync()).Where(x => !x.IsDeleted).Select(x => x.Id);
                    foreach(var i in request.CategoryIds)
                    {
                        if (!categoryIdList.Contains(i))
                        {
							return new ServiceResult()
							{
								Message = "Không tìm thấy thônng tin lĩnh vực",
								Status = StatusResult.ClientError
							};
						}
                    }

					var categories = (await _freelancerCategoryRepository.GetAllAsync()).Where(x => x.FreelancerId.Equals(id));
					/*foreach (var category in categories)
                    {
                        category.IsDeleted = true;
                        _freelancerCategoryRepository.Update(category);
                    }

                    foreach (var categoryId in request.CategoryIds)
                    {
						await _freelancerCategoryRepository.CreateAsync(new FreelancerCategory()
                        {
                            CategoryId = categoryId,
                            FreelancerId = id
                        });
                    }*/
        /*foreach (var category in categories)
        {

            category.IsDeleted = request.CategoryIds.Contains(category.CategoryId) ? false : true;
            _freelancerCategoryRepository.Update(category);
        }
        var categoryIdOfUser = categories.Select(x => x.CategoryId);
        foreach(var newCategory in request.CategoryIds)
        {
            if (!categoryIdOfUser.Contains(newCategory)){
                await _freelancerCategoryRepository.CreateAsync(new FreelancerCategory()
                {
                    CategoryId = newCategory,
                    FreelancerId = id
                });
            }

        }

    }

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

public async Task<ServiceResult> UpdateIdentityAsync(int id, FreelancerIdentityUpdateRequest request)
{
try
{
    Freelancer entity = await _freelancerRepository.GetByIdAsync(id);
    if (entity == null)
    {
        return new ServiceResult()
        {
            Message = "Không tìm thấy thônng tin",
            Status = StatusResult.ClientError
        };
    }
    entity.BankNumber = request.BankNumber ?? entity.BankNumber;
    entity.Identity = request.Identity ?? entity.Identity;
    entity.Email = request.Email ?? entity.Email;
    entity.PhoneNumber = request.PhoneNumber ?? entity.PhoneNumber;
    entity.NeedVerify = true; 
    _freelancerRepository.Update(entity);
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

    return new ServiceResult()
    {
        Message = $"Lỗi hệ thống, ({ex.InnerException.Message})",
        Status = StatusResult.SystemError
    };
}
}

public  async Task<ServiceResult> UpdatePasswordAsync(int id, PasswordUpdateRequest request)
{
try
{
    Freelancer entity = await _freelancerRepository.GetByIdAsync(id);
    if (entity == null)
    {
        return new ServiceResult()
        {
            Message = "Không tìm thấy thônng tin",
            Status = StatusResult.ClientError
        };
    }
    var verify = PasswordHelper.VerifyPassword(entity.Password, request.CurrentPassword);
    if (!verify)
    {
        return new ServiceResult()
        {
            Message = "Sai mật khẩu hiện tại",
            Status = StatusResult.ClientError
        };
    }
    if (!request.NewPassword.Equals(request.ConfirmPassword))
    {
        return new ServiceResult()
        {
            Message = "Sai mật khẩu xác nhận",
            Status = StatusResult.ClientError
        };
    }
    var passwordHasher = PasswordHelper.HashPassword(request.NewPassword);
    entity.Password = passwordHasher;
    entity.CreateUpdate = DateTime.Now;
    _freelancerRepository.Update(entity);
    await _unitOfWork.SaveChangeAsync();

    return new ServiceResult()
    {
        Message = "Cập nhật mật khẩu thành công",
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

public async Task<ServiceResult> UpdatePriorityAsync(int id, PriorityUpdateRequest request)
{
try
{
    Freelancer entity = await _freelancerRepository.GetByIdAsync(id);
    if (entity == null)
    {
        return new ServiceResult()
        {
            Message = "Không tìm thấy thônng tin",
            Status = StatusResult.ClientError
        };
    }
    entity.Priority = request.Priority;
    entity.CreateUpdate = DateTime.Now;
    _freelancerRepository.Update(entity);
    await _unitOfWork.SaveChangeAsync();

    return new ServiceResult()
    {
        Message = "Cập nhật ưu tiên thành công",
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

public async Task<ServiceResult> VerifyAccountAsync(int id, VerificationRequest request)
{
try
{
    Freelancer entity = await _freelancerRepository.GetByIdAsync(id);
    if (entity == null)
    {
        return new ServiceResult()
        {
            Message = "Không tìm thấy thônng tin",
            Status = StatusResult.ClientError
        };
    }
    entity.Verification = request.Verification;
    entity.CreateUpdate = DateTime.Now;
    entity.NeedVerify = false;
    _freelancerRepository.Update(entity);
    await _unitOfWork.SaveChangeAsync();

    return new ServiceResult()
    {
        Message = "Cập nhật xác minh danh tính thành công",
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
}*/
    }
}
