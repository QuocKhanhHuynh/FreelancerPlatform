using FluentValidation;
using FreelancerPlatform.Application.Dtos.SystemManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Skill
{
    public class SkillCreateRequestValidator : AbstractValidator<SkillCreateRequest>
    {
        public SkillCreateRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên kỹ năng không được rỗng");
        }
    }
}
