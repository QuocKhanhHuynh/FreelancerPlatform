using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Contract
{
    public class ContractCreateRequestValidator : AbstractValidator<ContractCreateRequest>
    {
        public ContractCreateRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên hợp đồng không được rỗng");
            RuleFor(x => x.Content).NotEmpty().WithMessage("Nội dung hợp đồng không được rỗng");
            RuleFor(x => x.JobId).NotEmpty().WithMessage("Hợp đồng phải thuộc một công việc").GreaterThan(0).WithMessage("Hợp đồng phải thuộc một công việc"); ;
        }
    }
}
