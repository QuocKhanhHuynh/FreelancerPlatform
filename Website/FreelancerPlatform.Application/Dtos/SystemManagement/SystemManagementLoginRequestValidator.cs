using FluentValidation;
using FreelancerPlatform.Application.Dtos.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.SystemManagement
{
    public class SystemManagementLoginRequestValidator : AbstractValidator<SystemManagementLoginRequest>
    {
        public SystemManagementLoginRequestValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Tên đăng nhập không được rỗng");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Mật khẩu không được rỗng");
        }
    }
}
