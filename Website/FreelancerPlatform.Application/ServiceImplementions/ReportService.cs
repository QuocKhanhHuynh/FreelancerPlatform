using AutoMapper;
using FreelancerPlatform.Application.Abstraction.Repository;
using FreelancerPlatform.Application.Abstraction.Service;
using FreelancerPlatform.Application.Abstraction.UnitOfWork;
using FreelancerPlatform.Application.Dtos.Apply;
using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.Report;
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
    public class ReportService : BaseService<Report>, IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IFreelancerRepository _freelancerRepository;
        public ReportService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<Report> logge, IReportRepository reportRepository, IFreelancerRepository freelancerRepository) : base(unitOfWork, mapper, logge)
        {
            _reportRepository = reportRepository;
            _freelancerRepository = freelancerRepository;
        }
        public async Task<ServiceResult> CreateReportAsync(ReportCreateRequest request)
        {
            try
            {
               var reportExist = (await _reportRepository.GetAllAsync()).Where(x => x.FreelancerId == request.FreelancerId && x.UserReport == request.UserReport);
                if (reportExist.Count() > 0)
                {
                    return new ServiceResult()
                    {
                        Message = "Bạn đã thực hiện báo cáo người dùng này",
                        Status = StatusResult.ClientError
                    };
                }
                var report = new Report()
                {
                    Content = request.Content,
                    FreelancerId = request.FreelancerId,
                    UserReport = request.UserReport
                };
                await _reportRepository.CreateAsync(report);
                await _unitOfWork.SaveChangeAsync();

                return new ServiceResult()
                {
                    Message = "Tạo báo cáo thành công",
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

        public async Task<List<ReportQuickViewModel>> GetReportAllAsync()
        {
            var reports = await _reportRepository.GetAllAsync();
            var freelancers = await _freelancerRepository.GetAllAsync();
            var query = from r in reports
                        join f in freelancers on r.FreelancerId equals f.Id
                        where r.IsReview == false
                        select new { r, f };
            return query.Select(x => new ReportQuickViewModel()
            {
                Id = x.f.Id,
                DateCreate = x.f.CreateDay,
                FirstName = x.f.FirstName,
                LastName = x.f.LastName,
                FreelancerId = x.f.Id,
                NumberReport = reports.Where(y => y.FreelancerId == x.f.Id).Count(),
            }).ToList();
        }

        public async Task<ReportViewModel> GetReportAsync(int id)
        {
            var report = await _reportRepository.GetByIdAsync(id);
            var freelancer = await _freelancerRepository.GetByIdAsync(report.FreelancerId);
            return new ReportViewModel()
            {
                Id = id,
                FreelancerId = freelancer.Id,
                Content = report.Content,
                DateCreate = report.CreateDay,
                FirstName = freelancer.FirstName,
                LastName = freelancer.LastName,
            };
        }

        public async Task<ServiceResult> ReviewReportAsync(int id)
        {
            try
            {
                var report = await _reportRepository.GetByIdAsync(id);
                report.IsReview = true;
                _reportRepository.Update(report);
                await _unitOfWork.SaveChangeAsync();

                return new ServiceResult()
                {
                    Message = "Cập nhật báo cáo thành công",
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

        public async Task<ServiceResult> UnReviewReportAsync(int id)
        {
            try
            {
                var report = await _reportRepository.GetByIdAsync(id);
                report.IsReview = false;
                _reportRepository.Update(report);
                await _unitOfWork.SaveChangeAsync();

                return new ServiceResult()
                {
                    Message = "Cập nhật báo cáo thành công",
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
