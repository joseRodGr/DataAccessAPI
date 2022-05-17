using DataAccessAPI.Dtos;
using DataAccessAPI.Helpers;
using DataAccessAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessAPI.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<ItemCategory>> GetAllAsync();
        Task<ServerResponse<ItemCategory>> GetByIdAsync(int id);
        Task<ServerResponse<ItemCategory>> Create(CreateCategoryDto createCategoryDto);
        Task<ServerResponse<ItemCategory>> Update(int id, UpdateCategoryDto updateCategoryDto);
        Task<ServerResponse<ItemCategory>> Delete(int id);
    }
}
