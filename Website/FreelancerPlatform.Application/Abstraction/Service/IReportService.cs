using FreelancerPlatform.Application.Dtos.Apply;
using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Abstraction.Service
{
    public interface IReportService
    {
        Task<ServiceResult> CreateReportAsync(ReportCreateRequest request);
        Task<List<ReportQuickViewModel>> GetReportAllAsync();
        Task<ReportViewModel> GetReportAsync(int id);
        Task<ServiceResult> ReviewReportAsync(int id);
        Task<ServiceResult> UnReviewReportAsync(int id);
    }
}
