using AutoMapper;
using FreelancerPlatform.Application.Abstraction.Repository;
using FreelancerPlatform.Application.Abstraction.Service;
using FreelancerPlatform.Application.Abstraction.UnitOfWork;
using FreelancerPlatform.Application.Consts;
using FreelancerPlatform.Application.Dtos.Category;
using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.Freelancer;
using FreelancerPlatform.Application.Dtos.Job;
using FreelancerPlatform.Application.Dtos.Skill;
using FreelancerPlatform.Application.ServiceImplementions.Base;
using FreelancerPlatform.Domain.Constans;
using FreelancerPlatform.Domain.Entity;
using FreelancerPlatform.Domain.Entity.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FreelancerPlatform.Application.ServiceImplementions
{
    public class JobService : BaseService<Job>, IJobService
    {
        private readonly IJobRepository _jobRepository;
        private readonly IFavoriteJobRepository _favoriteJobRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IApplyRepository _applyRepository;
        private readonly IJobSkillRepository _jobSkillRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IRecentViewRepository _recentViewRepository;


        public JobService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<Job> logger, IJobRepository jobRepository, 
            IFavoriteJobRepository favoriteJobRepository, IContractRepository contractRepository
            , ITransactionRepository transactionRepository, ICategoryRepository categoryRepository, ISkillRepository skillRepository,
            IApplyRepository applyRepository, IJobSkillRepository jobSkillRepository, IRecentViewRepository recentViewRepository) : base(unitOfWork, mapper, logger)
        {
            _jobRepository = jobRepository;
            _jobRepository = jobRepository;
            _favoriteJobRepository = favoriteJobRepository;
            _transactionRepository = transactionRepository;
            _categoryRepository = categoryRepository;
            _applyRepository = applyRepository;
            _jobSkillRepository = jobSkillRepository;
            _skillRepository = skillRepository;
            _contractRepository = contractRepository;
            _recentViewRepository = recentViewRepository;
        }

        public async Task<ServiceResult> AddViewRecent(int freelancerId, int jobId)
        {
            try
            {
                var recentViews = await _recentViewRepository.GetAllAsync();
                var entity = recentViews.FirstOrDefault(x => x.FreelancerId == freelancerId && x.JobId == jobId);
                if (entity != null)
                {
                    return new ServiceResult()
                    {
                        Message = "Tạo thông tin thành công",
                        Status = StatusResult.Success
                    };
                }
                var viewOfUsers = recentViews.Where(x => x.FreelancerId == freelancerId).OrderBy(x => x.CreateDay).ToList();
                if (viewOfUsers.Count() == 10)
                {
                    var viewOfUser = viewOfUsers[0];
                    _recentViewRepository.Delete(viewOfUser);
                }

                var recentView = new RecentView()
                {
                    FreelancerId = freelancerId,
                    JobId = jobId
                };
                    
                await _recentViewRepository.CreateAsync(recentView);
            
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

        public async Task<ServiceResult> CreateJobAsync(JobCreateRequest request)
        {
            try
            {
               
                var job = new Job()
                {
                    CategoryId = request.CategoryId,
                    Description = request.Description,
                    MaxDeal = request.MaxDeal,
                    Name = request.Name,
                    FreelancerId = request.FreelancerId,
                    MinDeal = request.MinDeal,
                    SalaryType = request.SalaryType,
                    JobType = request.JobType,
                    WorkType = request.WorkType,
                };
                if(request.Requirement != null)
                {
                    job.Requirement = request.Requirement;
                }
                if (request.EstimatedCompletion.HasValue && request.EstimatedCompletion.Value > 0)
                {
                    job.EstimatedCompletion = request.EstimatedCompletion.Value;
                }
                if (request.HourPerDay.HasValue && request.HourPerDay > 0)
                {
                    job.HourPerDay = request.HourPerDay.Value;
                }
                await _jobRepository.CreateAsync(job);

                await _unitOfWork.SaveChangeAsync();
                if (request.Skills != null && request.Skills.Count > 0)
                {
                    foreach (var i in request.Skills)
                    {
                        await _jobSkillRepository.CreateAsync(new JobSkill()
                        {
                            JobId = job.Id,
                            SkillId = i
                        });
                    }
                }
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

        public async Task<ServiceResult> DeleteHidenJobAsync(int id)
        {
            try
            {
                Job entity = await _jobRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return new ServiceResult()
                    {
                        Message = "Không tìm thấy thônng tin",
                        Status = StatusResult.ClientError
                    };
                }
                entity.IsHiden = false;

                entity.CreateUpdate = DateTime.Now;
                _jobRepository.Update(entity);
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

        public async Task<List<JobQuickViewModel>> GetAllJobsAsync()
        {
            var jobs = (await _jobRepository.GetAllAsync());
            var categories = (await _categoryRepository.GetAllAsync());
            var jobSkills =  await _jobSkillRepository.GetAllAsync();
            var skills = await _skillRepository.GetAllAsync();

            var contracts = await _contractRepository.GetAllAsync();
            var jobContract = contracts.Select(x => x.ProjectId).ToList();

            var query = from j in jobs 
                        join c in categories on j.CategoryId equals c.Id
                        select new {j, c};

            var queryJobSkill = from j in jobSkills
                                join s in skills on j.SkillId equals s.Id
                                select new {j, s};

            return query.Select(x => new JobQuickViewModel
            {
                Id = x.j.Id,
                Category = new CategoryQuickViewModel() { Id = x.c.Id, Name = x.c.Name},

                CreateDay = x.j.CreateDay,
                FreelancerId = x.j.FreelancerId,
                MaxDeal = x.j.MaxDeal.GetValueOrDefault(),
                MinDeal = x.j.MinDeal,
                Name = x.j.Name,
                Priority = x.j.Priority,
                Description = x.j.Description,
                JobType = x.j.JobType,
                SalaryType = x.j.SalaryType,
                IsHiden =  x.j.IsHiden,
                WorkType = x.j.WorkType,
                InContract = jobContract.Contains(x.j.Id) ? true : false,
                Skills = queryJobSkill.Where(y => y.j.JobId == x.j.Id).Select(y => new SkillQuickViewModel()
                {
                    Id = y.s.Id,
                    Name = y.s.Name
                }).ToList()
            }).ToList();
        }

        public async Task<JobViewModel> GetJobAsync(int id)
        {
            var job = (await _jobRepository.GetAllAsync()).FirstOrDefault(x => x.Id == id);
            var category = (await _categoryRepository.GetAllAsync()).FirstOrDefault(x => x.Id == job.CategoryId);
            var jobSkills = await _jobSkillRepository.GetAllAsync();
            var skills = await _skillRepository.GetAllAsync();


            var queryJobSkill = from j in jobSkills
                                join s in skills on j.SkillId equals s.Id
                                where j.JobId == job.Id
                                select new { j, s };

            return new JobViewModel
            {
                Id = job.Id,
                Category = new CategoryQuickViewModel() { Id = category.Id, Name = category.Name },

                CreateDay = job.CreateDay,
                FreelancerId = job.FreelancerId,
                MaxDeal = job.MaxDeal.GetValueOrDefault(),
                MinDeal = job.MinDeal,
                Name = job.Name,
                Priority = job.Priority,
                Description = job.Description,
                JobType = job.JobType,
                SalaryType = job.SalaryType,
                WorkType = job.WorkType,
                Skills = queryJobSkill.Select(y => new SkillQuickViewModel()
                {
                    Id = y.s.Id,
                    Name = y.s.Name
                }).ToList(),
                IsHiden = job.IsHiden,
                EstimatedCompletion = job.EstimatedCompletion,
                HourPerDay = job.HourPerDay,
                Requirement = job.Requirement,
            };
        }

        public async Task<List<int>> GetJobIdRecentViewOfFreelancer(int freelancerId)
        {
            var recentViewJob = (await _recentViewRepository.GetAllAsync()).Where(x => x.FreelancerId == freelancerId).Select(x => x.JobId).ToList();
            return recentViewJob;
        }

        public async Task<List<JobQuickViewModel>> GetRecentView()
        {
            var recentViewJob = (await _recentViewRepository.GetAllAsync()).GroupBy(x => x.JobId);
            var jobRecentView = new Dictionary<int, int>();
            foreach(var i in recentViewJob)
            {
                var key = i.Key;
                jobRecentView[i.Key] = i.Count();
            }
            var recentViewJobId = (jobRecentView.OrderByDescending(x => x.Value)).Take(10).Select(x => x.Key).ToList();

            var jobs = (await _jobRepository.GetAllAsync());
            var categories = (await _categoryRepository.GetAllAsync());
            var jobSkills = await _jobSkillRepository.GetAllAsync();
            var skills = await _skillRepository.GetAllAsync();

            var contracts = await _contractRepository.GetAllAsync();
            var jobContract = contracts.Select(x => x.ProjectId).ToList();

            var query = from j in jobs
                        join c in categories on j.CategoryId equals c.Id
                        where recentViewJobId.Contains(j.Id)
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
                WorkType = x.j.WorkType,
                IsHiden = x.j.IsHiden,
                InContract = jobContract.Contains(x.j.Id) ? true : false,
                Skills = queryJobSkill.Where(y => y.j.JobId == x.j.Id).Select(y => new SkillQuickViewModel()
                {
                    Id = y.s.Id,
                    Name = y.s.Name
                }).ToList()
            }).ToList();
        }

        public async Task<ServiceResult> HidenJobAsync(int id)
        {
            try
            {
                Job entity = await _jobRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return new ServiceResult()
                    {
                        Message = "Không tìm thấy thônng tin",
                        Status = StatusResult.ClientError
                    };
                }
                entity.IsHiden = true;

                entity.CreateUpdate = DateTime.Now;
                _jobRepository.Update(entity);
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

        public async Task<ServiceResult> UpdateJobAsync(int id, JobUpdateRequest request)
        {
            try
            {
                Job entity = await _jobRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return new ServiceResult()
                    {
                        Message = "Không tìm thấy thônng tin",
                        Status = StatusResult.ClientError
                    };
                }
                entity.CategoryId = request.CategoryId;
                entity.Description = request.Description;
                entity.MaxDeal = request.MaxDeal;
                entity.Name = request.Name;
                entity.FreelancerId = request.FreelancerId;
                entity.MinDeal = request.MinDeal;
                entity.SalaryType = request.SalaryType;
                entity.JobType = request.JobType;
                entity.Requirement = request.Requirement;
                entity.CreateUpdate = DateTime.Now;
                entity.HourPerDay = request.HourPerDay;
                entity.WorkType = request.WorkType;
               
                if (request.EstimatedCompletion.HasValue && request.EstimatedCompletion > 0)
                {
                    entity.EstimatedCompletion = request.EstimatedCompletion.Value;
                    entity.HourPerDay = null;
                }
                if (request.HourPerDay.HasValue && request.HourPerDay > 0)
                {
                    entity.HourPerDay = request.HourPerDay.Value;
                    entity.EstimatedCompletion = null;
                }
                _jobRepository.Update(entity);

                var oldJobSkill = (await _jobSkillRepository.GetAllAsync()).Where(x => x.JobId == entity.Id);
                foreach(var i in oldJobSkill)
                {
                    _jobSkillRepository.Delete(i);
                }

                foreach (var i in request.Skills)
                {
                    await _jobSkillRepository.CreateAsync(new JobSkill()
                    {
                        JobId = entity.Id,
                        SkillId = i
                    });
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
        /*public async Task<ServiceResult> AddFavoriteJobAsync(FavoriteJobAddRequest request)
{
try
{
var entity = new FavoriteJob()
{
FreelancerId = request.FreelancerId,
JobId = request.JobId
};
await _favoriteJobRepository.CreateAsync(entity);
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

public async Task<ServiceResult> CreateJobAsync(JobCreateRequest request)
{
try
{

await _jobRepository.CreateAsync(new Job()
{
CategoryId = request.CategoryId,
Description = request.Description,
EndDeal = request.EndDeal,
Name = request.Name,
RecruiterId = request.RecruiterId,
Requirement = request.Requirement,
StartDay = request.StartDay,
StartDeal = request.StartDeal,

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

public async Task<ServiceResult> DeleteFavoriteJobAsync(int freelancerId, int jobId)
{
try
{
FavoriteJob entity = (await _favoriteJobRepository.GetAllAsync()).FirstOrDefault(x => x.FreelancerId == freelancerId && x.JobId == jobId);
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

public async Task<ServiceResult> DeleteJobAsync(int id)
{
try
{
Job entity = await _jobRepository.GetByIdAsync(id);
if (entity == null)
{
return new ServiceResult()
{
Message = "Không tìm thấy thônng tin",
Status = StatusResult.ClientError
};
}
entity.IsDeleted = true;
_jobRepository.Update(entity);
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

public async Task<Pagination<JobQuickViewModel>> GetAllJobAsync(int pageIndex, int pageTake, int? categoryId, int? addressId, string? keywork = null, int? startDeal = null, int? endDeal = null)
{
var contracts = await _contractRepository.GetAllAsync();
var applies = (await _applyRepository.GetAllAsync()).Where(x => !x.IsDeleted);
var query = (await _jobRepository.GetAllAsync()).Where(x => !x.IsDeleted && x.StartDay < DateTime.Now && !contracts.Select(y => y.JobId).Contains(x.Id));

if (categoryId != null)
{
query = query.Where(x => x.CategoryId == categoryId.GetValueOrDefault());
}
if (addressId != null)
{
var recruiterAdressQuery = (await _recruiterRepository.GetAllAsync()).Where(x => x.Address == addressId).Select(x => x.Id);
query = query.Where(x => recruiterAdressQuery.Contains(x.RecruiterId));
}
if (keywork != null)
{
query = query.Where(x => x.Name.Contains(keywork));
}
if (startDeal != null)
{
query = query.Where(x => startDeal.GetValueOrDefault()  >= x.StartDeal);
}
if (endDeal != null)
{
query = query.Where(x => endDeal.GetValueOrDefault() <= x.EndDeal);
}
var pagination = new Pagination<JobQuickViewModel>();
pagination.PageIndex = pageIndex;
pagination.PageSize = pageTake;
pagination.TotalRecords = query.Count();
var categories = await _categoryRepository.GetAllAsync();
var recruiter = await _recruiterRepository.GetAllAsync();

var items = query.Skip((pageIndex - 1) * pageTake).Take(pageTake).Select(x => new JobQuickViewModel()
{
Id = x.Id,
CreateUpdate = x.CreateUpdate.GetValueOrDefault(),
Name = x.Name,
BussinisName = (recruiter.FirstOrDefault(y => y.Id == x.RecruiterId)).Name,
Address = (Constans.Address.FirstOrDefault(z => recruiter.FirstOrDefault(y => y.Id == x.RecruiterId).Address.GetValueOrDefault() == z.Id)) != null ? (Constans.Address.FirstOrDefault(z => recruiter.FirstOrDefault(y => y.Id == x.RecruiterId).Address.GetValueOrDefault() == z.Id)).Name : null,
Priority = x.Priority,
IsDelete = x.IsDeleted,
ImageUrl = (recruiter.FirstOrDefault(y => y.Id == x.RecruiterId)).ImageUrl,
CategoryName = categories.FirstOrDefault(y => y.Id == x.CategoryId).Name,
CategoryId = x.CategoryId,
AppliesNumber = applies.Where(y => y.JobId == x.Id).Count(),
RecruiterId = x.RecruiterId,
StartDeal = x.StartDeal,
EndDeal = x.EndDeal,
}).ToList();
pagination.Items = items;

return pagination;
}

public async Task<List<JobQuickViewModel>> GetJobNotFilterAsync()
{
var contracts = await _contractRepository.GetAllAsync();
var query = (await _jobRepository.GetAllAsync()).Where(x => !x.IsDeleted && x.StartDay < DateTime.Now && !contracts.Select(y => y.JobId).Contains(x.Id));
var transactions = await _transactionRepository.GetAllAsync();
var categories = await _categoryRepository.GetAllAsync();
var recruiter = await _recruiterRepository.GetAllAsync();
var applies = (await _applyRepository.GetAllAsync()).Where(x => !x.IsDeleted);
return query.Select(x => new JobQuickViewModel()
{
Id = x.Id,
CreateUpdate = x.CreateUpdate.GetValueOrDefault(),
BussinisName = (recruiter.FirstOrDefault(y => y.Id == x.RecruiterId)).Name,
Address = (Constans.Address.FirstOrDefault(z => recruiter.FirstOrDefault(y => y.Id == x.RecruiterId).Address.GetValueOrDefault() == z.Id)) != null ? (Constans.Address.FirstOrDefault(z => recruiter.FirstOrDefault(y => y.Id == x.RecruiterId).Address.GetValueOrDefault() == z.Id)).Name : null,
Priority = x.Priority,
IsDelete = x.IsDeleted,
ImageUrl = (recruiter.FirstOrDefault(y => y.Id == x.RecruiterId)).ImageUrl,
CategoryName = categories.FirstOrDefault(y => y.Id == x.CategoryId).Name,
CategoryId = x.CategoryId,
AppliesNumber = applies.Where(y => y.JobId == x.Id).Count()
}).ToList();
}

public async Task<Pagination<JobQuickViewModel>> GetFavoriteJobAsync(int freelancerId, int pageIndex, int pageTake)
{
var queryFavoriteJob = (await _favoriteJobRepository.GetAllAsync()).Where(x => !x.IsDeleted && x.FreelancerId == freelancerId).Select(x => x.JobId);
var queryJob = (await _jobRepository.GetAllAsync()).Where(x => !x.IsDeleted && queryFavoriteJob.Contains(x.Id));

var pagination = new Pagination<JobQuickViewModel>();
pagination.PageIndex = pageIndex;
pagination.PageSize = pageTake;
pagination.TotalRecords = queryJob.Count();

var items = queryJob.Skip((pageIndex - 1) * pageTake).Take(pageTake).Select(x => _mapper.Map<JobQuickViewModel>(x)).ToList();
pagination.Items = items;

return pagination;
}

public async Task<JobViewModel> GetJobAsync(int id)
{
Job entity = await _jobRepository.GetByIdAsync(id);
var category = await _categoryRepository.GetAllAsync();
var applies = (await _applyRepository.GetAllAsync()).Where(x => !x.IsDeleted);
var recruiters = await _recruiterRepository.GetAllAsync();
return new JobViewModel()
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

public async Task<Pagination<JobQuickViewModel>> GetJobOfRecruiterAsync(int recruiterId, int pageIndex, int pageTake, int? categoryId, string? keywork = null, int? startDeal = null, int? endDeal = null)
{
var contracts = await _contractRepository.GetAllAsync();
var applies = (await _applyRepository.GetAllAsync()).Where(x => !x.IsDeleted);
var query = (await _jobRepository.GetAllAsync()).Where(x => !x.IsDeleted && x.StartDay < DateTime.Now && !contracts.Select(y => y.JobId).Contains(x.Id) && x.RecruiterId == recruiterId);

if (categoryId != null)
{
query = query.Where(x => x.CategoryId == categoryId.GetValueOrDefault());
}
if (keywork != null)
{
query = query.Where(x => x.Name.Contains(keywork));
}
if (startDeal != null)
{
query = query.Where(x => startDeal.GetValueOrDefault() >= x.StartDeal);
}
if (endDeal != null)
{
query = query.Where(x => endDeal.GetValueOrDefault() <= x.EndDeal);
}

var pagination = new Pagination<JobQuickViewModel>();
pagination.PageIndex = pageIndex;
pagination.PageSize = pageTake;
pagination.TotalRecords = query.Count();
var categories = await _categoryRepository.GetAllAsync();
var recruiter = await _recruiterRepository.GetAllAsync();

var items = query.Skip((pageIndex - 1) * pageTake).Take(pageTake).Select(x => new JobQuickViewModel()
{
Id = x.Id,
CreateUpdate = x.CreateUpdate.GetValueOrDefault(),
Name = x.Name,
BussinisName = (recruiter.FirstOrDefault(y => y.Id == x.RecruiterId)).Name,
Address = (Constans.Address.FirstOrDefault(z => recruiter.FirstOrDefault(y => y.Id == x.RecruiterId).Address.GetValueOrDefault() == z.Id)) != null ? (Constans.Address.FirstOrDefault(z => recruiter.FirstOrDefault(y => y.Id == x.RecruiterId).Address.GetValueOrDefault() == z.Id)).Name : null,
Priority = x.Priority,
IsDelete = x.IsDeleted,
ImageUrl = (recruiter.FirstOrDefault(y => y.Id == x.RecruiterId)).ImageUrl,
CategoryName = categories.FirstOrDefault(y => y.Id == x.CategoryId).Name,
CategoryId = x.CategoryId,
AppliesNumber = applies.Where(y => y.JobId == x.Id).Count(),
RecruiterId = x.RecruiterId,
StartDeal = x.StartDeal,
EndDeal = x.EndDeal,
}).ToList();
pagination.Items = items;

return pagination;
}

public async Task<List<JobQuickViewModel>> GetJobOfRecruiterNuotFilterAsync(int recruiterId)
{
var contracts = await _contractRepository.GetAllAsync();
var applies = (await _applyRepository.GetAllAsync()).Where(x => !x.IsDeleted);
var query = (await _jobRepository.GetAllAsync()).Where(x => !x.IsDeleted && x.StartDay < DateTime.Now && !contracts.Select(y => y.JobId).Contains(x.Id) && x.RecruiterId == recruiterId);

var categories = await _categoryRepository.GetAllAsync();
var recruiter = await _recruiterRepository.GetAllAsync();

var items = query.Select(x => new JobQuickViewModel()
{
Id = x.Id,
CreateUpdate = x.CreateUpdate.GetValueOrDefault(),
Name = x.Name,
BussinisName = (recruiter.FirstOrDefault(y => y.Id == x.RecruiterId)).Name,
Address = (Constans.Address.FirstOrDefault(z => recruiter.FirstOrDefault(y => y.Id == x.RecruiterId).Address.GetValueOrDefault() == z.Id)) != null ? (Constans.Address.FirstOrDefault(z => recruiter.FirstOrDefault(y => y.Id == x.RecruiterId).Address.GetValueOrDefault() == z.Id)).Name : null,
Priority = x.Priority,
IsDelete = x.IsDeleted,
ImageUrl = (recruiter.FirstOrDefault(y => y.Id == x.RecruiterId)).ImageUrl,
CategoryName = categories.FirstOrDefault(y => y.Id == x.CategoryId).Name,
CategoryId = x.CategoryId,
AppliesNumber = applies.Where(y => y.JobId == x.Id).Count(),
RecruiterId = x.RecruiterId,
StartDeal = x.StartDeal,
EndDeal = x.EndDeal,
}).ToList();

return items;
}

public async Task<ServiceResult> UpdateJobAsync(int id, JobUpdateRequest request)
{
try
{
Job entity = await _jobRepository.GetByIdAsync(id);
if (entity == null)
{
return new ServiceResult()
{
Message = "Không tìm thấy thônng tin",
Status = StatusResult.ClientError
};
}
entity.CategoryId = request.CategoryId;
entity.RecruiterId = request.RecruiterId;
entity.StartDay = request.StartDay;
entity.Description = request.Description;
entity.EndDeal = request.EndDeal;
entity.Name = request.Name;
entity.Requirement = request.Requirement;
entity.StartDeal = request.StartDeal;

entity.CreateUpdate = DateTime.Now;
_jobRepository.Update(entity);
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

public async Task<ServiceResult> UpdateHidenJobAsync(int id)
{
try
{
Job entity = await _jobRepository.GetByIdAsync(id);
if (entity == null)
{
return new ServiceResult()
{
Message = "Không tìm thấy thônng tin",
Status = StatusResult.ClientError
};
}
entity.IsHiden = !entity.IsHiden;

entity.CreateUpdate = DateTime.Now;
_jobRepository.Update(entity);
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

public async Task<bool> CheckHidenJobAsync(int id)
{
Job entity = await _jobRepository.GetByIdAsync(id);
return entity.IsHiden;

}*/
    }
}
