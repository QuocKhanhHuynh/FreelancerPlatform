using FluentValidation;
using FreelancerPlatform.Application.Dtos.Job;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Report
{
    public class ReportCreateRequestValidator : AbstractValidator<ReportCreateRequest>
    {
        public ReportCreateRequestValidator()
        {
            RuleFor(x => x.Content).NotEmpty().WithMessage("Nội dung báo cáo không được rỗng");
        }
    }
}
