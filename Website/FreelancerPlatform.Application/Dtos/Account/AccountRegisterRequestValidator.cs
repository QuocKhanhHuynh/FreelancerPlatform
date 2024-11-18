using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Account
{
    public class AccountRegisterRequestValidator : AbstractValidator<AccountRegisterRequest>
    {
        public AccountRegisterRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email không được rỗng").Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").WithMessage("Không đúng định dạng email");
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Tên không được rỗng");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Họ và tên đệm không được rỗng");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Mật khẩu không được rỗng").Matches(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$").WithMessage("Mật khẩu phải dài ít nhất 8 ký tự bao gồm ít nhất 1 số, 1 chữ cái viết hoa, 1 chữ cái thường và 1 ký tự đặc biệt");
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Mật khẩu xác nhận không được rỗng");
            RuleFor(x => x).Custom((request, context) =>
            {
                if (!request.ConfirmPassword.Equals(request.ConfirmPassword))
                    context.AddFailure("Mật khẩu xác nhận không khớp");
            });
        }
    }
}
