using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Job
{
    public class JobUpdateRequestValidator : AbstractValidator<JobUpdateRequest>
    {
        public JobUpdateRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên công việc không được rỗng");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Mô tả công việc không được rỗng");
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Nhóm công việc phải có");
            RuleFor(x => x.SalaryType).NotEmpty().WithMessage("Hình thức trả lương phải có");
            RuleFor(x => x.JobType).NotEmpty().WithMessage("Loại hình công việc phải có");
            RuleFor(x => x.WorkType).NotEmpty().WithMessage("Hình thức làm việc phải có");
            RuleFor(x => x.MinDeal).NotEmpty().WithMessage("Mức lương nhỏ nhất không được rỗng").GreaterThan(0).WithMessage("Mức lương nhỏ nhất phải lớn hơn 0");
            RuleFor(x => x.MaxDeal)
            .GreaterThan(0).WithMessage("Mức lương lớn nhất phải lớn hơn 0")
            .GreaterThan(x => x.MinDeal).WithMessage("Mức lương lớn nhất phải lớn hơn lương nhỏ nhất")
            .When(x => x.MaxDeal.HasValue)
            .WithMessage("Mức lương lớn nhất không được rỗng hoặc phải hợp lệ nếu có giá trị.");
            RuleFor(x => x.EstimatedCompletion).GreaterThan(0).When(x => x.EstimatedCompletion.HasValue).WithMessage("Số giờ ước lượng hoàn thành không được rỗng");
        }
    }
}
