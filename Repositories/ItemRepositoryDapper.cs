using AutoMapper;
using Dapper;
using DataAccessAPI.Dtos;
using DataAccessAPI.Helpers;
using DataAccessAPI.Interfaces;
using DataAccessAPI.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DataAccessAPI.Repositories
{
    public class ItemRepositoryDapper : IItemRepository
    {
        private readonly IDbConnection _dbConnection;
        private readonly IMapper _mapper;

        public ItemRepositoryDapper(IConfiguration configuration, IMapper mapper)
        {
            _dbConnection = new SqlConnection(configuration.GetConnectionString("DbConnection"));
            _mapper = mapper;
        }
        public async Task<ServerResponse<ItemDto>> Create(CreateItemDto createItemDto)
        {
            var sqlQuery = "INSERT INTO dbo.Items(ItemId, ItemName, ItemPrice, ItemCategoryId) " +
                "VALUES (@ItemId, @ItemName, @ItemPrice, @ItemCategoryId)";

            var item = _mapper.Map<Item>(createItemDto);
            item.ItemId = Guid.NewGuid();

            var rowsAffected = await _dbConnection.ExecuteAsync(sqlQuery, item);

            if (rowsAffected > 0)
                return new ServerResponse<ItemDto>
                {
                    IsSuccessful = true,
                    Message = null,
                    Content = _mapper.Map<ItemDto>(item) 
                };

            return new ServerResponse<ItemDto>
            {
                IsSuccessful = false,
                Message = "Failed to create new item",
                Content = null
            };

        }

        public async Task<ServerResponse<ItemDto>> Delete(Guid id)
        {
            var sqlQuery = "DELETE FROM dbo.Items WHERE ItemId = @ItemId";

            var rowsAffected = await _dbConnection.ExecuteAsync(sqlQuery, new { ItemId = id });

            if (rowsAffected > 0)
                return new ServerResponse<ItemDto>
                {
                    IsSuccessful = true,
                    Message = null,
                    Content = null
                };

            return new ServerResponse<ItemDto>
            {
                IsSuccessful = false,
                Message = "Failed to delete the item",
                Content = null
            };
        }

        public async Task<IEnumerable<ItemDto>> GetAllAsync()
        {
            var sqlQuery = "SELECT A.ItemId, A.ItemName, A.ItemPrice, B.CategoryName FROM dbo.Items AS A " +
                "LEFT JOIN dbo.ItemCategories AS B " +
                "ON A.ItemCategoryId = B.ItemCategoryId";

            return await _dbConnection.QueryAsync<ItemDto>(sqlQuery);
       
        }

        public async Task<ServerResponse<ItemDto>> GetByIdAsync(Guid id)
        {
            var sqlQuery = "SELECT A.ItemId, A.ItemName, A.ItemPrice, B.CategoryName FROM dbo.Items AS A " +
                "LEFT JOIN dbo.ItemCategories AS B " +
                "ON A.ItemCategoryId = B.ItemCategoryId " +
                "WHERE A.ItemId = @ItemId";

            var item = await _dbConnection.QueryFirstOrDefaultAsync<ItemDto>(sqlQuery, new { ItemId = id });

            if (item == null)
                return new ServerResponse<ItemDto>
                {
                    IsSuccessful = false,
                    Message = "Item could not be found",
                    Content = null
                };

            return new ServerResponse<ItemDto>
            {
                IsSuccessful = true,
                Message = null,
                Content = item
            };
          
        }

        public async Task<ServerResponse<ItemDto>> Update(Guid id, UpdateItemDto updateItemDto)
        {
            if (id != updateItemDto.ItemId)
                return new ServerResponse<ItemDto>
                {
                    IsSuccessful = false,
                    Message = "Item could not be found",
                    Content = null
                };

            var sqlQuery = "UPDATE dbo.Items SET ItemName = @ItemName, ItemPrice = @ItemPrice, ItemCategoryId = @ItemCategoryId " +
                "WHERE ItemId = @ItemId";

            var rowsAffected = await _dbConnection.ExecuteAsync(sqlQuery, updateItemDto);

            if (rowsAffected > 0)
                return new ServerResponse<ItemDto>
                {
                    IsSuccessful = true,
                    Message = null,
                    Content = null
                };

            return new ServerResponse<ItemDto>
            {
                IsSuccessful = false,
                Message = "Failed to update the item",
                Content = null
            };

        }
    }
}
