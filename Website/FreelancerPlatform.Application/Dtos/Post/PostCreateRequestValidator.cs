using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Post
{
    public class PostCreateRequestValidator : AbstractValidator<PostCreateRequest>
    {
        public PostCreateRequestValidator()
        {
            RuleFor(x => x.Content).NotEmpty().WithMessage("Nội dung bài đăng không được rỗng");
            RuleFor(x => x.Title).NotEmpty().WithMessage("Mô tả công việc không được rỗng");
        }
    }
}
