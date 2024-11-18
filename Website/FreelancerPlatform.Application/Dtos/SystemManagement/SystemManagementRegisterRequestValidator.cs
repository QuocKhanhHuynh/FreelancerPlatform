using FluentValidation;
using FreelancerPlatform.Application.Dtos.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.SystemManagement
{
    public class SystemManagementRegisterRequestValidator : AbstractValidator<SystemManagementRegisterRequest>
    {
        public SystemManagementRegisterRequestValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Tên đăng nhập không được rỗng");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Mật khẩu không được rỗng").Matches(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$").WithMessage("Mật khẩu phải dài ít nhất 8 ký tự bao gồm ít nhất 1 số, 1 chữ cái viết hoa, 1 chữ cái thường và 1 ký tự đặc biệt");
           
        }
    }
}
