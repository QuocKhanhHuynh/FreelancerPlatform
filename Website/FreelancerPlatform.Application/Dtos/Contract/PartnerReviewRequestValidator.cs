using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Contract
{
    public class PartnerReviewRequestValidator : AbstractValidator<PartnerReviewRequest>
    {
        public PartnerReviewRequestValidator()
        {
            RuleFor(x => x.Point)
            .NotEmpty().WithMessage("Điểm đánh giá phải có");
            RuleFor(x => x.Review).NotEmpty().WithMessage("Nội dung đánh giá phải có");
        }
    }
}
