using FluentValidation;

namespace FreelancerPlatform._Admin.Models
{
    public class CategoryCreateRequestFormValidator : AbstractValidator<CategoryCreateRequestForm>
    {
        public CategoryCreateRequestFormValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên lĩnh vực không được rỗng");
            RuleFor(x => x.ImageUrl).NotEmpty().WithMessage("Ảnh lĩnh vực phải có");
        }
    }
}
