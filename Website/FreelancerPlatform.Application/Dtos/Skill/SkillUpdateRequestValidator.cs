using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Skill
{
    public class SkillUpdateRequestValidator : AbstractValidator<SkillUpdateRequest>
    {
        public SkillUpdateRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên kỹ năng không được rỗng");
        }
    }
}
