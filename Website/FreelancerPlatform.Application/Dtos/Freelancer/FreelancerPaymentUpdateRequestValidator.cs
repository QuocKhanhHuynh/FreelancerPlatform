using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Freelancer
{
    public class FreelancerPaymentUpdateRequestValidator : AbstractValidator<FreelancerPaymentUpdateRequest>
    {
        public FreelancerPaymentUpdateRequestValidator()
        {
            RuleFor(x => x.BankName).NotEmpty().WithMessage("Tên ngân hàng không được rỗng");
            RuleFor(x => x.BankNumber).NotEmpty().WithMessage("Số tài khoản ngân hàng không được rỗng");
        }
    }
}
