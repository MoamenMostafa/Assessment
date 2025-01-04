using AutoMapper;
using Management.BL.DTOs;
using Management.BL.Services.Interface;
using Management.DL.Models;
using Management.DL.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Management.BL.Services
{
    namespace Management.BL.Services
    {
        public class ProductService : IProductService
        {
            private readonly IRepository<Product> _productRepository;
            private readonly IRepository<Category> _categoryRepository;
            private readonly IMapper _mapper;

            public ProductService(IRepository<Product> productRepository, IRepository<Category> categoryRepository, IMapper mapper)
            {
                _productRepository = productRepository;
                _categoryRepository = categoryRepository;
                _mapper = mapper;
            }


            public async Task<IEnumerable<ProductDto>> GetAllAsync()
            {
                var products = await _productRepository.GetAllAsync(query => query
                    .Include(p => p.Category) 
                    .OrderBy(p => p.Name)     
                ); return _mapper.Map<IEnumerable<ProductDto>>(products);
            }

            public async Task<ProductDto?> GetByIdAsync(Guid id)
            {
                var product = await _productRepository.GetByIdAsync(id, query => query.Include(p => p.Category)); // Include Category
                if (product == null)
                    return null;

                return _mapper.Map<ProductDto>(product);
            }


            public async Task AddAsync(ProductDto productDto)
            {
                var product = _mapper.Map<Product>(productDto);
                //product.Id = Guid.NewGuid();
                product.CreatedDate = DateTime.UtcNow;
                product.UpdatedDate = DateTime.UtcNow;

                await _productRepository.AddAsync(product);
            }

            public async Task UpdateAsync(ProductDto productDto)
            {
                Console.WriteLine($"Updating product with ID: {productDto.Id}");
                var product = await _productRepository.GetByIdAsync(productDto.Id);
                if (product == null)
                {
                    Console.WriteLine("Product not found.");
                    throw new KeyNotFoundException("Product not found");
                }
                var category = await _categoryRepository.GetByNameAsync(productDto.CategoryName);

                if (category == null)
                {
                    Console.WriteLine("The specified category does not exist.");
                    throw new InvalidOperationException("The specified category does not exist.");
                }

                product.Category = category;
                productDto.CategoryId = category.Id;
                Console.WriteLine($"Updated CategoryId to: {category.Id}");

                _mapper.Map(productDto, product);
                product.UpdatedDate = DateTime.UtcNow;

                await _productRepository.UpdateAsync(product);
            }

            public async Task DeleteAsync(Guid id)
            {
                await _productRepository.DeleteAsync(id);
            }
        }
    }

}
