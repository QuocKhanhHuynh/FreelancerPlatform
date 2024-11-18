using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Account
{
	public class PasswordUpdateRequestValidator : AbstractValidator<PasswordUpdateRequest>
	{
		public PasswordUpdateRequestValidator()
		{
			RuleFor(x => x.CurrentPassword).NotEmpty().WithMessage("Mật khẩu hiện tại không được rỗng");
			RuleFor(x => x.NewPassword).NotEmpty().WithMessage("Mật khẩu mới không được rỗng").Matches(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$").WithMessage("Mật khẩu phải dài ít nhất 8 ký tự bao gồm ít nhất 1 số, 1 chữ cái viết hoa, 1 chữ cái thường và 1 ký tự đặc biệt");
			RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Mật khẩu xác nhận không được rỗng");
			RuleFor(x => x).Custom((request, context) =>
			{
				if (request.ConfirmPassword != request.NewPassword)
					context.AddFailure("Mật khẩu xác nhận không khớp");
			});
		}
	}
}
