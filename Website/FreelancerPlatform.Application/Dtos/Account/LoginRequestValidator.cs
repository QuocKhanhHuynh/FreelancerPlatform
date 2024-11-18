using FluentValidation;
using FluentValidation.Validators;
using FreelancerPlatform.Application.Dtos.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Account
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email không được rỗng").Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").WithMessage("Không đúng định dạng email");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Mật khẩu không được rỗng").Matches(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$").WithMessage("Mật khẩu phải dài ít nhất 8 ký tự bao gồm ít nhất 1 số, 1 chữ cái viết hoa, 1 chữ cái thường và 1 ký tự đặc biệt");
        }
    }
}
