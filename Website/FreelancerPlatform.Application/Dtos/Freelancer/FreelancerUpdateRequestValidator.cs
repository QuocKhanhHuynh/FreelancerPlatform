using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Freelancer
{
    public class FreelancerUpdateRequestValidator : AbstractValidator<FreelancerUpdateRequest>
    {
        public FreelancerUpdateRequestValidator()
        {
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Họ và tên đệm không được rỗng");
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Tên không được rỗng");
        }
    }
}
