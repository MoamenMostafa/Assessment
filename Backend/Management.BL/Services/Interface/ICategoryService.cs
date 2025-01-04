using Management.BL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Management.BL.Services.Interface
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto> GetByIdAsync(Guid id);
        Task AddAsync(CategoryDto productDto);
        Task UpdateAsync(CategoryDto productDto);
        Task DeleteAsync(Guid id);
    }
}
