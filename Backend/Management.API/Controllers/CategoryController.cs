using Management.BL.DTOs;
using Management.BL.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace Management.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _distributedCache;
        private const string CategoriesCacheKey = "CategoriesCache";

        public CategoriesController(
            ICategoryService categoryService,
            IMemoryCache memoryCache,
            IDistributedCache distributedCache)
        {
            _categoryService = categoryService;
            _memoryCache = memoryCache;
            _distributedCache = distributedCache;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Page number and page size must be greater than 0.");

            IEnumerable<CategoryDto> categories;

            if (!_memoryCache.TryGetValue(CategoriesCacheKey, out categories))
            {
                var cachedCategories = await _distributedCache.GetStringAsync(CategoriesCacheKey);

                if (!string.IsNullOrEmpty(cachedCategories))
                {
                    categories = JsonSerializer.Deserialize<IEnumerable<CategoryDto>>(cachedCategories);
                }
                else
                {
                    categories = await _categoryService.GetAllAsync();

                    var options = new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                    };
                    await _distributedCache.SetStringAsync(CategoriesCacheKey, JsonSerializer.Serialize(categories), options);
                    _memoryCache.Set(CategoriesCacheKey, categories, TimeSpan.FromMinutes(5));
                }
            }

            int totalCount = categories.Count();

            var paginatedCategories = categories
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return Ok(new
            {
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Data = paginatedCategories
            });
        }


        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var cacheKey = $"Category_{id}";

            CategoryDto category;

            if (!_memoryCache.TryGetValue(cacheKey, out category!))
            {
                var cachedCategory = await _distributedCache.GetStringAsync(cacheKey);

                if (!string.IsNullOrEmpty(cachedCategory))
                {
                    category = JsonSerializer.Deserialize<CategoryDto>(cachedCategory);
                }
                else
                {
                    category = await _categoryService.GetByIdAsync(id);

                    if (category == null)
                        return NotFound();

                    var options = new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                    };
                    await _distributedCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(category), options);

                    _memoryCache.Set(cacheKey, category, TimeSpan.FromMinutes(10));
                }
            }

            return Ok(category);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _categoryService.AddAsync(categoryDto);

            _memoryCache.Remove(CategoriesCacheKey);
            await _distributedCache.RemoveAsync(CategoriesCacheKey);

            return CreatedAtAction(nameof(GetById), new { id = categoryDto.Id }, categoryDto);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CategoryDto categoryDto)
        {
            if (id != categoryDto.Id)
                return BadRequest("ID mismatch");

            await _categoryService.UpdateAsync(categoryDto);

            _memoryCache.Remove(CategoriesCacheKey);
            await _distributedCache.RemoveAsync(CategoriesCacheKey);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _categoryService.DeleteAsync(id);

            _memoryCache.Remove(CategoriesCacheKey);
            await _distributedCache.RemoveAsync(CategoriesCacheKey);

            return NoContent();
        }
    }
}
