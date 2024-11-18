using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Freelancer
{
	public class FreelancerIdentityUpdateRequestValidator : AbstractValidator<FreelancerIdentityUpdateRequest>
	{
		public FreelancerIdentityUpdateRequestValidator()
		{
			RuleFor(x => x.Identity).NotEmpty().WithMessage("Số căn cước công dân không được rỗng");
			RuleFor(x => x.BankNumber).NotEmpty().WithMessage("Số tài khoản ngân hàng không được rỗng");
			RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Số điện thoại không được rỗng");
			RuleFor(x => x.Email).NotEmpty().WithMessage("Email không được rỗng").Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").WithMessage("Không đúng định dạng email");
		}
	}
}
