using AutoMapper;
using Dapper;
using DataAccessAPI.Dtos;
using DataAccessAPI.Helpers;
using DataAccessAPI.Interfaces;
using DataAccessAPI.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DataAccessAPI.Repositories
{
    public class CategoryRepositoryDapper : ICategoryRepository
    {
        private readonly IDbConnection _dbConnection;
        private readonly IMapper _mapper;

        public CategoryRepositoryDapper(IConfiguration configuration, IMapper mapper)
        {
            _dbConnection = new SqlConnection(configuration.GetConnectionString("DbConnection"));
            _mapper = mapper;
        }
        public async Task<ServerResponse<ItemCategory>> Create(CreateCategoryDto createCategoryDto)
        {

            var sqlQuery = "INSERT INTO dbo.ItemCategories(CategoryName, Description) VALUES(@CategoryName, @Description)";

            var rowsAffected = await _dbConnection.ExecuteAsync(sqlQuery, createCategoryDto);

            if (rowsAffected > 0)
            {
                var category = _mapper.Map<ItemCategory>(createCategoryDto);
                category.ItemCategoryId = await _dbConnection.QuerySingleOrDefaultAsync<int>("SELECT IDENT_CURRENT('ItemCategories')");

                return new ServerResponse<ItemCategory>
                {
                    IsSuccessful = true,
                    Message = null,
                    Content = category
                };
            }

            return new ServerResponse<ItemCategory>
            {
                IsSuccessful = false,
                Message = "Failed to create the category",
                Content = null
            };
   
        }

        public async Task<ServerResponse<ItemCategory>> Delete(int id)
        {
            var sqlQuery = "DELETE FROM dbo.ItemCategories WHERE ItemCategoryId = @ItemCategoryId";

            var rowsAffected = await _dbConnection.ExecuteAsync(sqlQuery, new { ItemCategoryId = id });

            if (rowsAffected > 0)
                return new ServerResponse<ItemCategory>
                {
                    IsSuccessful = true,
                    Message = null,
                    Content = null
                };

            return new ServerResponse<ItemCategory>
            {
                IsSuccessful = false,
                Message = "Failed to delete the category",
                Content = null
            };
        }

        public async Task<IEnumerable<ItemCategory>> GetAllAsync()
        {
            var sqlQuery = "SELECT ItemCategoryId, CategoryName, Description FROM dbo.ItemCategories";
            return await _dbConnection.QueryAsync<ItemCategory>(sqlQuery);
        }

        public async Task<ServerResponse<ItemCategory>> GetByIdAsync(int id)
        {
            var sqlQuery = "SELECT ItemCategoryId, CategoryName, Description FROM dbo.ItemCategories " +
                "WHERE ItemCategoryId = @ItemCategoryId";

            var category = await _dbConnection.QueryFirstOrDefaultAsync<ItemCategory>(sqlQuery, new { ItemCategoryId = id });

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

        public async Task<ServerResponse<ItemCategory>> Update(int id, UpdateCategoryDto updateCategoryDto)
        {
            if (id != updateCategoryDto.ItemCategoryId)
                return new ServerResponse<ItemCategory>
                {
                    IsSuccessful = false,
                    Message = "Category could not be found",
                    Content = null
                };

            var sqlQuery = "UPDATE dbo.ItemCategories SET CategoryName = @CategoryName, Description = @Description " +
                "WHERE ItemCategoryId = @ItemCategoryId";

            var rowsAffected = await _dbConnection.ExecuteAsync(sqlQuery, updateCategoryDto);

            if (rowsAffected > 0)
                return new ServerResponse<ItemCategory>
                {
                    IsSuccessful = true,
                    Message = null,
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
