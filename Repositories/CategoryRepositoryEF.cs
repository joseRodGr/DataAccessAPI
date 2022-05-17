using AutoMapper;
using DataAccessAPI.Data;
using DataAccessAPI.Dtos;
using DataAccessAPI.Helpers;
using DataAccessAPI.Interfaces;
using DataAccessAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessAPI.Repositories
{
    public class CategoryRepositoryEF : ICategoryRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CategoryRepositoryEF(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServerResponse<ItemCategory>> Create(CreateCategoryDto createCategoryDto)
        {
            var category = _mapper.Map<ItemCategory>(createCategoryDto);
            _context.ItemCategories.Add(category);

            if (await SaveAsync())
                return new ServerResponse<ItemCategory>
                {
                    IsSuccessful = true,
                    Message = null,
                    Content = category
                };

            return new ServerResponse<ItemCategory>
            {
                IsSuccessful = false,
                Message = "Failed to create the category",
                Content = null
            };
            
        }

        public async Task<ServerResponse<ItemCategory>> Delete(int id)
        {
            var category = await _context.ItemCategories.FirstOrDefaultAsync(x => x.ItemCategoryId == id);

            if (category == null)
                return new ServerResponse<ItemCategory>
                {
                    IsSuccessful = false,
                    Message = "Category could not be found",
                    Content = null
                };

            _context.ItemCategories.Remove(category);

            if (await SaveAsync())
                return new ServerResponse<ItemCategory>
                {
                    IsSuccessful =  true,
                    Message = null,
                    Content = null
                };

            return new ServerResponse<ItemCategory>
            {
                IsSuccessful = false,
                Message = "Failed to delete the item",
                Content = null
            };
         
        }

        public async Task<IEnumerable<ItemCategory>> GetAllAsync()
        {
            return await _context.ItemCategories.ToListAsync();
        }

        public async Task<ServerResponse<ItemCategory>> GetByIdAsync(int id)
        {
            var category = await _context.ItemCategories
                .FirstOrDefaultAsync(x => x.ItemCategoryId == id);

            if (category == null)
                return new ServerResponse<ItemCategory>
                {
                    IsSuccessful = false,
                    Message = "Category could not be found",
                    Content = null
                };

            return new ServerResponse<ItemCategory>
            {
                IsSuccessful = true,
                Message = null,
                Content = category
            };
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<ServerResponse<ItemCategory>> Update(int id, UpdateCategoryDto updateCategoryDto)
        {

            if (id != updateCategoryDto.ItemCategoryId)
                return new ServerResponse<ItemCategory>
                {
                   IsSuccessful = false,
                   Message = "Category could not be found",
                   Content = null
                };

            var category = await _context.ItemCategories.FirstOrDefaultAsync(x => x.ItemCategoryId == id);

            if (category == null)
                return new ServerResponse<ItemCategory>
                {
                    IsSuccessful = false,
                    Message = "Category could not be found",
                    Content = null
                };

            _mapper.Map(updateCategoryDto, category);
            _context.Attach(category);
            _context.Entry(category).State = EntityState.Modified;

            if (await SaveAsync())
                return new ServerResponse<ItemCategory>
                {
                    IsSuccessful = true,
                    Message = "",
                    Content = null
                };

            return new ServerResponse<ItemCategory>
            {
                IsSuccessful = false,
                Message = "Failed to update the category",
                Content = null
            };
        }
    }
}
