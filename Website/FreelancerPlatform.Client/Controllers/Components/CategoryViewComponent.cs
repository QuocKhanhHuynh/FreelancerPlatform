using FreelancerPlatform.Application.Abstraction.Service;
using Microsoft.AspNetCore.Mvc;

namespace FreelancerPlatform.Client.Controllers.Components
{
    public class CategoryViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;
        public CategoryViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _categoryService.GetCategoryAsync();
            return View("Default", categories);
        }
    }
}
