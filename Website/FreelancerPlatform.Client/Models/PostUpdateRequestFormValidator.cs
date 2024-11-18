using FluentValidation;

namespace FreelancerPlatform.Client.Models
{
    public class PostUpdateRequestFormValidator : AbstractValidator<PostUpdateRequestForm>
    {
        public PostUpdateRequestFormValidator()
        {
            RuleFor(x => x.Content).NotEmpty().WithMessage("Nội dung bài đăng không được rỗng");
            RuleFor(x => x.Title).NotEmpty().WithMessage("Tiêu đề bài đăng không được rỗng");
        }
    }
}
