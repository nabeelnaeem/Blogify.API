using Blogify.API.Data;
using Blogify.API.Models.Domain;
using Blogify.API.Models.DTO;
using Blogify.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blogify.API.Controllers
{   // https://localhost:hhhh/api/categories
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }
        // 
        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDto request)
        {
            //Map Dto to Domain Model
            var category = new Category
            {
                Name = request.Name,
                UrlHandle = request.UrlHandle,
            };

            //Repository Call
            //We never implement DTO in repository
            await categoryRepository.CreateAsync(category);

            //Domain to Dto
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };
            return Ok(response);
        }
        // Get https://localhost:7293/api/Categories
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await categoryRepository.GetAllAsync();
            // Map domain to dto
            var categoriesDto = new List<CategoryDto>();
            foreach (var category in categories)
            {
                categoriesDto.Add(new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    UrlHandle = category.UrlHandle,
                });
            }
            return Ok(categoriesDto);
        }

        // Get https://localhost:7293/api/Categories/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
            var category = await categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            //Convert to DTO
            var response = new CategoryDto()
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };
            return Ok(response);
        }

        // Put https://localhost:7293/api/Categories/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]

        public async Task<IActionResult> EditCategory([FromRoute] Guid id, [FromBody] UpdateCategoryRequestDto request)
        {
            //Convert to Domain Model
            var category = new Category()
            {
                Id = id,
                Name = request.Name,
                UrlHandle = request.UrlHandle,
            };

            category = await categoryRepository.UpdateAsync(category);
            if (category == null)
            {
                return NotFound();
            }

            //Convert to DTO
            var response = new CategoryDto()
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle= category.UrlHandle,
            };
            return Ok(response);
        }

        // Delete https://localhost:7293/api/Categories/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]

        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var category = await categoryRepository.DeleteAsync(id);
            if (category is null)
            {
                return NotFound();
            }

            //Convert to DTO
            var response = new CategoryDto()
            {
                Id=category.Id,
                Name = category.Name,
                UrlHandle=category.UrlHandle
            };
            return Ok(response);
        }
    }
}
