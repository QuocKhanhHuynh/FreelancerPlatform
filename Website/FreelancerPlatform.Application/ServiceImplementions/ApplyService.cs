using AutoMapper;
using FreelancerPlatform.Application.Abstraction.Repository;
using FreelancerPlatform.Application.Abstraction.Service;
using FreelancerPlatform.Application.Abstraction.UnitOfWork;
using FreelancerPlatform.Application.Dtos.Apply;
using FreelancerPlatform.Application.Dtos.Category;
using FreelancerPlatform.Application.Dtos.Common;
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
using System.Xml.Schema;

namespace FreelancerPlatform.Application.ServiceImplementions
{
    public class ApplyService : BaseService<Apply>, IApplyService
    {
        private readonly IApplyRepository _applyRepository;
        private readonly IJobRepository _jobRepository;
        private readonly ICategoryRepository _categoryRepository;
		private readonly IFreelancerRepository _freelancerRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly IJobSkillRepository _jobSkillRepository;
        private readonly IFavoriteJobRepository _favoriteJobRepository;
        private readonly IOfferRepository _offerRepository;
        private readonly IContractRepository _contractRepository;
		public ApplyService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<Apply> logger, IApplyRepository applyRepository, IJobRepository jobRepository,
            ICategoryRepository categoryRepository, IOfferRepository offerRepository,
            IFreelancerRepository freelancerRepository, ISkillRepository skillRepository, IJobSkillRepository jobSkillRepository, IFavoriteJobRepository favoriteJobRepository, IContractRepository contractRepository) : base(unitOfWork, mapper, logger)
        {
            _applyRepository = applyRepository;
            _jobRepository = jobRepository;
            _categoryRepository = categoryRepository;
            _freelancerRepository = freelancerRepository;
            _skillRepository = skillRepository;
            _jobSkillRepository = jobSkillRepository;
            _favoriteJobRepository = favoriteJobRepository;
            _offerRepository = offerRepository;
            _contractRepository = contractRepository;
        }

        public async Task<ServiceResultInt> CreateApplyAsync(ApplyCreateRequest request)
        {
            try
            {
                //var entity = _mapper.Map<Apply>(request);
                var apply = (await _applyRepository.GetAllAsync()).FirstOrDefault(x => x.FreelancerId == request.FreelancerId && x.JobId == request.JobId);
                var isOffer = (await _offerRepository.GetAllAsync()).Where(x => x.JobId == request.JobId && x.FreelancerOfferId == request.FreelancerId).Any();
                if (apply != null)
                {
                    return new ServiceResultInt()
                    {
                        Message = "Đã thực hiện ứng tuyển",
                        Status = StatusResult.ClientError
                    };
                }
                var job = await _jobRepository.GetByIdAsync(request.JobId);
                if (job.MaxDeal  == null)
                {
                    if (request.Deal != 0 && request.Deal != job.MinDeal)
                    {
                        return new ServiceResultInt()
                        {
                            Message = "Mức lương đưa ra chưa phù hợp với công việc",
                            Status = StatusResult.ClientError
                        };
                    }
                }
                else
                {
                    if (request.Deal < job.MinDeal || request.Deal > job.MaxDeal || request.Deal == null)
                    {
                        return new ServiceResultInt()
                        {
                            Message = "Mức lương đưa ra chưa phù hợp với công việc",
                            Status = StatusResult.ClientError
                        };
                    }
                }
                var newApply = new Apply()
                {
                    FreelancerId = request.FreelancerId,
                    JobId = request.JobId,
                    Introduction = request.Introduction,
                    ExecutionTime = request.ExecutionDay,
                    Deal = request.Deal.Value,
                };
                if (isOffer)
                {
                    newApply.IsOffer = true;
                }
                await _applyRepository.CreateAsync(newApply);
                await _unitOfWork.SaveChangeAsync();

                return new ServiceResultInt()
                {
                    Message = "Tạo thông tin thành công",
                    Status = StatusResult.Success,
                    Result = newApply.Id
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

        public async Task<ServiceResult> DeleteApplyAsync(int id)
        {
            try
            {
                //var entity = _mapper.Map<Apply>(request);
                var apply =await _applyRepository.GetByIdAsync(id);

                if (apply == null)
                {
                    return new ServiceResultInt()
                    {
                        Message = "Không tìm thấy ứng tuyển",
                        Status = StatusResult.ClientError
                    };
                }
                _applyRepository.Delete(apply);
                await _unitOfWork.SaveChangeAsync();

                return new ServiceResult()
                {
                    Message = "Xóa thông tin thành công",
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

        public async Task<ApplyOfJobQuickViewModel> GetApplyAsync(int id)
        {
            var applyById = await _applyRepository.GetByIdAsync(id);
            var freelancer = await _freelancerRepository.GetByIdAsync(applyById.FreelancerId);
            return new ApplyOfJobQuickViewModel
            {
                CreateDay = applyById.CreateDay,
                Deal = applyById.Deal.GetValueOrDefault(),
                ExecutionDay = applyById.ExecutionTime,
                FirstName = freelancer.FirstName,
                Id = applyById.Id,
                ImageUrl = freelancer.ImageUrl,
                Introduction = applyById.Introduction,
                LastName = freelancer.LastName
            };
        }

        public async Task<List<JobQuickViewModel>> GetApplyByFreelancerAsync(int id)
        {
            var applyOfFreelancer = (await _applyRepository.GetAllAsync()).Where(x => x.FreelancerId == id);
            var jobs = (await _jobRepository.GetAllAsync());
            var categories = (await _categoryRepository.GetAllAsync());
            var jobSkills = await _jobSkillRepository.GetAllAsync();
            var skills = await _skillRepository.GetAllAsync();
            var contracts = await _contractRepository.GetAllAsync();
            var jobContract = contracts.Select(x => x.ProjectId).ToList();

            var query = from j in jobs
                        join c in categories on j.CategoryId equals c.Id
                        join a in applyOfFreelancer on j.Id equals a.JobId

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

        public async Task<List<ApplyOfJobQuickViewModel>> GetApplyOfJobAsync(int jobId)
        {
            var query = from a in (await _applyRepository.GetAllAsync())
                        where a.JobId == jobId
                        join f in (await _freelancerRepository.GetAllAsync()) on a.FreelancerId equals f.Id
                        select new { a, f };
            query = query.OrderByDescending(x => x.a.CreateDay);    
            return query.Select(x => new ApplyOfJobQuickViewModel()
            {
                Deal = x.a.Deal.GetValueOrDefault(),
                ExecutionDay = x.a.ExecutionTime,
                Id = x.a.Id,
                Introduction = x.a.Introduction,
                CreateDay = x.a.CreateDay,
                ImageUrl = x.f.ImageUrl,
                LastName = x.f.LastName,
                FirstName = x.f.FirstName,
                FreelancerId = x.a.FreelancerId,
                IsOffer = x.a.IsOffer,
            }).ToList();
        }

        /*
public async Task<ServiceResult> CreateApplyAsync(ApplyCreateRequest request)
{
try
{
//var entity = _mapper.Map<Apply>(request);
var job = await _jobRepository.GetByIdAsync(request.JobId);
if (request.Deal < job.MinDeal || request.Deal > job.MaxDeal)
{
return new ServiceResult()
{
Message = "Mức lương đưa ra chưa phù hợp với công việc",
Status = StatusResult.ClientError
};
}

await _applyRepository.CreateAsync(new Apply()
{
FreelancerId = request.FreelancerId,
JobId = request.JobId,
Introduction = request.Introduction,
ExecutionTime = request.ExecutionDay,
Deal = request.Deal
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

public async Task<ServiceResult> DeleteApplyAsync(int id)
{
try
{
Apply entity = (await _applyRepository.GetAllAsync()).FirstOrDefault(x => x.Id == id);
if (entity == null)
{
return new ServiceResult()
{
Message = "Không tìm thấy thônng tin",
Status = StatusResult.ClientError
};
}
if (entity.Status == 3)
{
return new ServiceResult()
{
Message = "Đã tạo hợp đồng không thể hủy",
Status = StatusResult.ClientError
};
}
entity.IsDeleted = true;
_applyRepository.Update(entity);
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

public async Task<Pagination<ApplyQuickViewModel>> GetAllApplyAsync(int pageIndex, int pageTake)
{
var query = (await _applyRepository.GetAllAsync()).Where(x => !x.IsDeleted);
var pagination = new Pagination<ApplyQuickViewModel>();
pagination.PageIndex = pageIndex;
pagination.PageSize = pageTake;
pagination.TotalRecords = query.Count();

var items = query.Skip((pageIndex - 1) * pageTake).Take(pageTake).Select(x => _mapper.Map<ApplyQuickViewModel>(x)).ToList();
pagination.Items = items;

return pagination;
}

public async Task<ApplyViewModel> GetApplyAsync(int id)
{
Apply apply = await _applyRepository.GetByIdAsync(id);
Job entity = await _jobRepository.GetByIdAsync(apply.JobId);
var category = await _categoryRepository.GetAllAsync();
var applies = (await _applyRepository.GetAllAsync()).Where(x => !x.IsDeleted);
var recruiters = await _recruiterRepository.GetAllAsync();
return new ApplyViewModel()
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
ApplyNumber = applies.Where(x => x.JobId == apply.Id).Count(),
RecruiterName = recruiters.FirstOrDefault(x => x.Id == entity.RecruiterId).Name,
Deal = apply.Deal,
ExecutionDay = apply.ExecutionDay,
Introduction = apply.Introduction,
StatusApply = apply.Status,
ApplyId = id
};
}

public async Task<Pagination<ApplyQuickViewModel>> GetApplyByFreelancerAsync(int freelancerId, int pageIndex, int pageTake)
{
var query = (await _applyRepository.GetAllAsync()).Where(x => !x.IsDeleted && x.FreelancerId == freelancerId);
var pagination = new Pagination<ApplyQuickViewModel>();
pagination.PageIndex = pageIndex;
pagination.PageSize = pageTake;
pagination.TotalRecords = query.Count();

var items = query.Skip((pageIndex - 1) * pageTake).Take(pageTake).Select(x => _mapper.Map<ApplyQuickViewModel>(x)).ToList();
pagination.Items = items;

return pagination;
}

public async Task<Pagination<ApplyQuickViewModel>> GetApplyOfRecruiterAsync(int recruiterId, int pageIndex, int pageTake)
{
var jobQuery = (await _jobRepository.GetAllAsync()).Where(x => x.RecruiterId == recruiterId).Select(x => x.Id);
var query = (await _applyRepository.GetAllAsync()).Where(x => !x.IsDeleted && jobQuery.Contains(x.JobId));
var pagination = new Pagination<ApplyQuickViewModel>();
pagination.PageIndex = pageIndex;
pagination.PageSize = pageTake;
pagination.TotalRecords = query.Count();

var items = query.Skip((pageIndex - 1) * pageTake).Take(pageTake).Select(x => _mapper.Map<ApplyQuickViewModel>(x)).ToList();
pagination.Items = items;

return pagination;
}

public async Task<ServiceResult> UpdateApplyAsync(int id, ApplyUpdateRequest request)
{
try
{
Apply entity = await _applyRepository.GetByIdAsync(id);
if (entity == null)
{
return new ServiceResult()
{
Message = "Không tìm thấy thônng tin",
Status = StatusResult.ClientError
};
}
var updateEntity = _mapper.Map<Apply>(request);
entity.CreateUpdate = DateTime.Now;
_applyRepository.Update(updateEntity);
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

public async Task<ServiceResult> UpdateStatusApplyAsync(int id, int status)
{
try
{
Apply entity = await _applyRepository.GetByIdAsync(id);
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
_applyRepository.Update(entity);
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


public async Task<Pagination<ApplyQuickViewModel>> GetJobApplyByFreelancerAsync(int pageIndex, int pageTake, int freelancerId, string? keyword, int? categoryId, int? addressId, int? startDeal, int? endDeal)
{

var contracts = await _contractRepository.GetAllAsync();
var applies = (await _applyRepository.GetAllAsync()).Where(x => !x.IsDeleted);
var jobApplyByFreelancer = applies.Where(x => x.FreelancerId == freelancerId).Select(x => x.JobId);
var query = (await _jobRepository.GetAllAsync()).Where(x => !x.IsDeleted && x.StartDay < DateTime.Now && !contracts.Select(y => y.JobId).Contains(x.Id) && jobApplyByFreelancer.Contains(x.Id));
var queries = from q in query
join a in applies on q.Id equals a.JobId
where a.IsDeleted == false
select new { a, q };

var jobs = await _jobRepository.GetAllAsync();
if (keyword != null)
{
var jobKeyword = jobs.Where(x => x.Name.Contains(keyword)).Select(x => x.Id);
queries = queries.Where(x => jobKeyword.Contains(x.a.JobId));

}
if (categoryId != null)
{
var jobCategoryId = jobs.Where(x => x.CategoryId == categoryId).Select(x => x.Id);
queries = queries.Where(x => jobCategoryId.Contains(x.a.JobId));
}
if (addressId != null)
{
var recruiterAddressId = (await _recruiterRepository.GetAllAsync()).Where(x => x.Address == addressId).Select(x => x.Id);
var jobAddressId = jobs.Where(x => recruiterAddressId.Contains(x.RecruiterId)).Select(x => x.Id);
queries = queries.Where(x => jobAddressId.Contains(x.a.JobId));
}
if (startDeal != null)
{
var jobStartDeal = jobs.Where(x => startDeal >= x.StartDeal).Select(x => x.Id);
queries = queries.Where(x => jobStartDeal.Contains(x.a.JobId));
}
if (endDeal != null)
{
var jobEndDeal = jobs.Where(x => endDeal <= x.EndDeal).Select(x => x.Id);
queries = queries.Where(x => jobEndDeal.Contains(x.a.JobId));
}

var pagination = new Pagination<ApplyQuickViewModel>();
pagination.PageIndex = pageIndex;
pagination.PageSize = pageTake;
pagination.TotalRecords = queries.Count();
var categories = await _categoryRepository.GetAllAsync();
var recruiter = await _recruiterRepository.GetAllAsync();

var items = queries.Skip((pageIndex - 1) * pageTake).Take(pageTake).Select(x => new ApplyQuickViewModel()
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
ApplyId = x.a.Id,
Deal  = x.a.Deal,
ExecutionDay = x.a.ExecutionDay,
JobId = x.a.JobId,
StartDay = x.q.StartDay,
Status = x.a.Status
}).ToList();
pagination.Items = items;

return pagination;
}

public async Task<List<ApplyQuickViewModel>> GetJobApplyByFreelancerNptFilterAsync(int freelancerId)
{
var contracts = await _contractRepository.GetAllAsync();
var applies = (await _applyRepository.GetAllAsync()).Where(x => !x.IsDeleted);
var jobApplyByFreelancer = applies.Where(x => x.FreelancerId == freelancerId).Select(x => x.JobId);
var query = (await _jobRepository.GetAllAsync()).Where(x => !x.IsDeleted && x.StartDay < DateTime.Now && !contracts.Select(y => y.JobId).Contains(x.Id) && jobApplyByFreelancer.Contains(x.Id));
var queries = from q in query
join a in applies on q.Id equals a.JobId
where a.IsDeleted == false
select new { a, q };

var jobs = await _jobRepository.GetAllAsync();

var categories = await _categoryRepository.GetAllAsync();
var recruiter = await _recruiterRepository.GetAllAsync();

var items = queries.Select(x => new ApplyQuickViewModel()
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
ApplyId = x.a.Id,
Deal = x.a.Deal,
ExecutionDay = x.a.ExecutionDay,
JobId = x.a.JobId,
StartDay = x.q.StartDay,
Status = x.a.Status
}).ToList();
return items;
}

public async Task<List<ApplyOfJobQuickViewModel>> GetApplyOfJobAsync(int jobId)
{
var query = from a in ((await _applyRepository.GetAllAsync()).Where(x => !x.IsDeleted))
where !a.IsDeleted && a.JobId == jobId
join f in (await _freelancerRepository.GetAllAsync()) on a.FreelancerId equals f.Id
select new { a, f };

return query.Select(x => new ApplyOfJobQuickViewModel()
{
Deal = x.a.Deal,
ExecutionDay = x.a.ExecutionDay,
Id = x.a.Id,
Introduction = x.a.Introduction,
CreateDay = x.a.CreateDay,
ImageUrl = x.f.ImageUrl,
UserName = x.f.UserName,
}).ToList();
}

public async Task<Pagination<ApplyApproveViewModel>> GetApplyApproveAsync(int recruiterId, int pageIndex, int pageTake, int? jobId)
{
var applies = await  _applyRepository.GetAllAsync();
var freelancers = await _freelancerRepository.GetAllAsync();
var jobs = await _jobRepository.GetAllAsync();
var query = from a in applies
join f in freelancers on a.FreelancerId equals f.Id
join j in jobs on a.JobId equals j.Id
where a.Status == 2 && j.RecruiterId == recruiterId && a.IsDeleted == false
select new {a, f, j};
if (jobId != null)
{
query = query .Where(x => x.a.JobId == jobId);
}
var pagination = new Pagination<ApplyApproveViewModel>();
pagination.PageIndex = pageIndex;
pagination.PageSize = pageTake;
pagination.TotalRecords = query.Count();
var categories = await _categoryRepository.GetAllAsync();
var recruiter = await _recruiterRepository.GetAllAsync();

var items = query.Skip((pageIndex - 1) * pageTake).Take(pageTake).Select(x => new ApplyApproveViewModel()
{
ApproveDay = x.a.CreateUpdate.GetValueOrDefault(),
FreelancerName = x.f.FullName,
ImageUrl = x.f.ImageUrl,
JobId = x.j.Id,
FreelancerId = x.f.Id
}).ToList();
pagination.Items = items;

return pagination;
}
*/
    }
}


/*



 public async Task<Pagination<ApplyQuickViewModel>> GetApplyByFreelancerAsync(int freelancerId, int pageIndex, int pageTake, string? keyword, int? categoryId, int? addressId, int? startDeal = null, int? endDeal = null)
        {
           
            var query = (await _applyRepository.GetAllAsync()).Where(x => !x.IsDeleted && x.FreelancerId == freelancerId);
            var jobs = await _jobRepository.GetAllAsync();
            if (keyword != null)
            {
                var jobKeyword = jobs.Where(x => x.Name.Contains(keyword)).Select(x => x.Id);
                query = query.Where(x => jobKeyword.Contains(x.JobId));

            }
            if (categoryId != null)
            {
                var jobCategoryId = jobs.Where(x => x.CategoryId == categoryId).Select(x => x.Id);
                query = query.Where(x => jobCategoryId.Contains(x.JobId));
            }
            if (addressId != null)
            {
                var recruiterAddressId = (await _recruiterRepository.GetAllAsync()).Where(x => x.Address == addressId).Select(x => x.Id);
                var jobAddressId = jobs.Where(x => recruiterAddressId.Contains(x.RecruiterId)).Select(x => x.Id);
                query = query.Where(x => jobAddressId.Contains(x.JobId));
            }
            if (startDeal != null)
            {
                var jobStartDeal = jobs.Where(x => x.StartDeal >= startDeal).Select(x => x.Id);
                query = query.Where(x => jobStartDeal.Contains(x.JobId));
            }
            if (endDeal != null)
            {
                var jobEndDeal = jobs.Where(x => x.EndDeal >= endDeal).Select(x => x.Id);
                query = query.Where(x => jobEndDeal.Contains(x.JobId));
            }
            var pagination = new Pagination<ApplyQuickViewModel>();
            pagination.PageIndex = pageIndex;
            pagination.PageSize = pageTake;
            pagination.TotalRecords = query.Count();

            var items = query.Skip((pageIndex - 1) * pageTake).Take(pageTake).Select(x => _mapper.Map<ApplyQuickViewModel>(x)).ToList();
            pagination.Items = items;

            return pagination;
        }



*/