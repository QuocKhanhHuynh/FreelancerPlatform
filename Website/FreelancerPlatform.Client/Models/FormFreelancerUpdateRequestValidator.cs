using FluentValidation;

namespace FreelancerPlatform.Client.Models
{
    public class FormFreelancerUpdateRequestValidator : AbstractValidator<FormFreelancerUpdateRequest>
    {
        public FormFreelancerUpdateRequestValidator()
        {
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Họ và tên đệm không được rỗng");
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Tên không được rỗng");
        }
    }
}
