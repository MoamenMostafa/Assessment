using Management.BL.DTOs;
using Management.BL.Services;
using Management.BL.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace Management.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _distributedCache;
        private const string ProductCacheKey = "ProductsCache";

        public ProductsController(IProductService productService, IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            _productService = productService;
            _memoryCache = memoryCache;
            _distributedCache = distributedCache;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Page number and page size must be greater than 0.");

            IEnumerable<ProductDto> products;

            if (!_memoryCache.TryGetValue(ProductCacheKey, out products))
            {
                var cachedProducts = await _distributedCache.GetStringAsync(ProductCacheKey);

                if (!string.IsNullOrEmpty(cachedProducts))
                {
                    products = JsonSerializer.Deserialize<IEnumerable<ProductDto>>(cachedProducts);
                }
                else
                {
                    products = await _productService.GetAllAsync();

                    var options = new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                    };
                    await _distributedCache.SetStringAsync(ProductCacheKey, JsonSerializer.Serialize(products), options);
                    _memoryCache.Set(ProductCacheKey, products, TimeSpan.FromMinutes(5));
                }
            }

            int totalCount = products.Count();

            var paginatedProducts = products
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return Ok(new
            {
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Data = paginatedProducts
            });
        }


        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var cacheKey = $"Category_{id}";

            ProductDto product;

            if (!_memoryCache.TryGetValue(cacheKey, out product!))
            {
                var cachedProduct = await _distributedCache.GetStringAsync(cacheKey);

                if (!string.IsNullOrEmpty(cachedProduct))
                {
                    product = JsonSerializer.Deserialize<ProductDto>(cachedProduct);
                }
                else
                {
                    product = await _productService.GetByIdAsync(id);

                    if (product == null)
                        return NotFound();

                    var options = new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                    };
                    await _distributedCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(product), options);

                    _memoryCache.Set(cacheKey, product, TimeSpan.FromMinutes(10));
                }
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductDto productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _productService.AddAsync(productDto);

            _memoryCache.Remove(ProductCacheKey);
            await _distributedCache.RemoveAsync(ProductCacheKey);

            return CreatedAtAction(nameof(GetById), new { id = productDto.Id }, productDto);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ProductDto productDto)
        {
            if (id != productDto.Id)
                return BadRequest("ID mismatch");

            await _productService.UpdateAsync(productDto);

            _memoryCache.Remove(ProductCacheKey);
            await _distributedCache.RemoveAsync(ProductCacheKey);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _productService.DeleteAsync(id);

            _memoryCache.Remove(ProductCacheKey);
            await _distributedCache.RemoveAsync(ProductCacheKey);

            return NoContent();
        }
    }
}
