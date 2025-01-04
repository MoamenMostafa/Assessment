using AutoMapper;
using Management.BL.DTOs;
using Management.BL.Services.Interface;
using Management.DL.Models;
using Management.DL.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Management.BL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(IRepository<Category> categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto?> GetByIdAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                return null;

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task AddAsync(CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            category.Id = Guid.NewGuid();
            category.CreatedDate = DateTime.UtcNow;
            category.UpdatedDate = DateTime.UtcNow;

            await _categoryRepository.AddAsync(category);
        }

        public async Task UpdateAsync(CategoryDto categoryDto)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryDto.Id);
            if (category == null)
                throw new KeyNotFoundException("Category not found");

            _mapper.Map(categoryDto, category);
            category.UpdatedDate = DateTime.UtcNow;

            await _categoryRepository.UpdateAsync(category);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _categoryRepository.DeleteAsync(id);
        }

    }
}
