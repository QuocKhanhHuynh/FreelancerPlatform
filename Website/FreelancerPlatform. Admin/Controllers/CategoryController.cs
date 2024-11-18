using Azure.Core;
using FreelancerPlatform._Admin.Models;
using FreelancerPlatform._Admin.Services;
using FreelancerPlatform.Application.Abstraction.Repository;
using FreelancerPlatform.Application.Abstraction.Service;
using FreelancerPlatform.Application.Dtos.Category;
using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.Skill;
using FreelancerPlatform.Application.ServiceImplementions;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace FreelancerPlatform._Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ISkillService  _skillService;
        private readonly IStorageService _storageService;
        public CategoryController(ICategoryService categoryService, ISkillService skillService, IStorageService storageService)
        {
            _categoryService = categoryService;
            _skillService = skillService;
            _storageService = storageService;
        }
        public async Task<IActionResult> GetCategory()
        {
            var categories = await _categoryService.GetCategoryAsync();
            return View(categories);
        }
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateCategory([FromForm] CategoryCreateRequestForm request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new
                {
                    Field = x.Key,
                    Errors = x.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                })
                .ToList();

                return BadRequest(new { Errors = errors });
            }

            var applicationRequest = new CategoryCreateRequest()
            {
               Name = request.Name,
               
            };
            if (request.ImageUrl != null)
            {
                applicationRequest.ImageUrl = await SaveFileAsync(request.ImageUrl);
            }
            var response = await _categoryService.CreateCategoryAsync(applicationRequest);
            if (response.Status != StatusResult.Success)
            {
                return BadRequest();
            }
            return Ok(response.Message);
        }

        public async Task<IActionResult> UpdateCategory(int id)
        {
            var categories = (await _categoryService.GetCategoryAsync());
            var category = categories.FirstOrDefault(x => x.Id == id);
            var request = new CategoryQuickViewModel()
            {
                Id = category.Id,
                ImageUrl = category.ImageUrl,
                Name = category.Name
            };
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategory(CategoryUpdateRequestForm request)
        {
            var applicationRequest = new CategoryUpdateRequest()
            {
                Name = request.Name,
            };
            if (request.ImageUrl != null)
            {
                var categories = await _categoryService.GetCategoryAsync();
                var category = categories.FirstOrDefault(x => x.Id == request.Id);
                await _storageService.DeleteFileAsync(category.ImageUrl);
                var newImageUrl = await SaveFileAsync(request.ImageUrl);
                applicationRequest.ImageUrl = newImageUrl;
            }
            var response = await _categoryService.UpdateCategoryAsync(request.Id, applicationRequest);
            if (response.Status != StatusResult.Success)
            {
                return BadRequest();
            }
            return Ok(response.Message);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            if (result.Status != StatusResult.Success)
            {
                var errors = new List<object>
                {
                    new
                    {
                        Field = "ResultStatus",
                        Errors = new[] { result.Message }
                    }
                };

                return BadRequest(new { Errors = errors });
            }

            return Ok(result.Message);
        }

        public async Task<IActionResult> GetSkill()
        {
            var skills = await _skillService.GetSkillAsync();
            return View(skills);
        }

        public IActionResult CreateSkill()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateSkill(SkillCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new
                {
                    Field = x.Key,
                    Errors = x.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                })
                .ToList();

                return BadRequest(new { Errors = errors });
            }
            var result = await _skillService.CreateSkillAsync(request);
            if (result.Status != StatusResult.Success)
            {
                var errors = new List<object>
                {
                    new
                    {
                        Field = "ResultStatus",
                        Errors = new[] { result.Message }
                    }
                };

                return BadRequest(new { Errors = errors });
            }

            return Ok(result.Message);
        }

        public async Task<IActionResult> UpdateSkill(int id)
        {
            var skills = await _skillService.GetSkillAsync();
            var skill = skills.FirstOrDefault(x => x.Id == id);
            return View(skill);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSkill(SkillUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new
                {
                    Field = x.Key,
                    Errors = x.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                })
                .ToList();

                return BadRequest(new { Errors = errors });
            }
            var result = await _skillService.UpdateSkillAsync(request);
            if (result.Status != StatusResult.Success)
            {
                var errors = new List<object>
                {
                    new
                    {
                        Field = "ResultStatus",
                        Errors = new[] { result.Message }
                    }
                };

                return BadRequest(new { Errors = errors });
            }

            return Ok(result.Message);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSkill(int id)
        {
            var result = await _skillService.DeleteSkillAsync(id);
            if (result.Status != StatusResult.Success)
            {
                var errors = new List<object>
                {
                    new
                    {
                        Field = "ResultStatus",
                        Errors = new[] { result.Message }
                    }
                };

                return BadRequest(new { Errors = errors });
            }

            return Ok(result.Message);
        }
        private async Task<string> SaveFileAsync(IFormFile file)
        {
           // var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = file.FileName; //$"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            try
            {
                await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            }
            catch (Exception ex)
            {

            }
            return fileName;
        }
    }

}
