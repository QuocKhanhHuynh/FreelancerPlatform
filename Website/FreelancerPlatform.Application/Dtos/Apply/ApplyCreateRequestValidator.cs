using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Apply
{
	public class ApplyCreateRequestValidator : AbstractValidator<ApplyCreateRequest>
	{
		public ApplyCreateRequestValidator()
		{
			RuleFor(x => x.ExecutionDay).NotEmpty().WithMessage("Số ngày thực hiện đưa ra không được rỗng").GreaterThanOrEqualTo(0).WithMessage("Ngày thực hiện không được nhỏ hơn 0");
		}
	}
}
