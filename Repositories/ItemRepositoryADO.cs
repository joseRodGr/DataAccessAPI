using AutoMapper;
using DataAccessAPI.Dtos;
using DataAccessAPI.Helpers;
using DataAccessAPI.Interfaces;
using DataAccessAPI.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessAPI.Repositories
{
    public class ItemRepositoryADO : IItemRepository
    {
    
        private readonly SqlConnection _sqlConnection;
        private readonly IMapper _mapper;

        public ItemRepositoryADO(IConfiguration configuration, IMapper mapper)
        {
            _sqlConnection = new SqlConnection(configuration.GetConnectionString("DbConnection"));
            _mapper = mapper;
        }

        public async Task<ServerResponse<ItemDto>> Create(CreateItemDto createItemDto)
        {
            var queryString = "INSERT INTO dbo.Items(ItemId, ItemName, ItemPrice, ItemCategoryId) " +
                "VALUES (@ItemId, @ItemName, @ItemPrice, @ItemCategoryId)";

            var item = _mapper.Map<Item>(createItemDto);
            item.ItemId = Guid.NewGuid();
            
            SqlCommand command = new SqlCommand(queryString, _sqlConnection);
            command.Parameters.AddWithValue("@ItemId", item.ItemId);
            command.Parameters.AddWithValue("@ItemName", createItemDto.ItemName);
            command.Parameters.AddWithValue("@ItemPrice", createItemDto.ItemPrice);
            command.Parameters.AddWithValue("@ItemCategoryId", createItemDto.ItemCategoryId);

            _sqlConnection.Open();

            var rowsAffected = await command.ExecuteNonQueryAsync();

            _sqlConnection.Close();

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
                Message = "Failed to create the item",
                Content = null
            };


        }

        public async Task<ServerResponse<ItemDto>> Delete(Guid id)
        {
            var queryString = "DELETE FROM dbo.Items WHERE ItemId = @ItemId";

            SqlCommand command = new SqlCommand(queryString, _sqlConnection);
            command.Parameters.AddWithValue("@ItemId", id);

            _sqlConnection.Open();

            int rowsAffected = await command.ExecuteNonQueryAsync();

            _sqlConnection.Close();

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
            var queryString = "SELECT A.ItemId, A.ItemName, A.ItemPrice, B.CategoryName FROM dbo.Items AS A " +
                "LEFT JOIN dbo.ItemCategories AS B " +
                "ON A.ItemCategoryId = B.ItemCategoryId";

            SqlCommand command = new SqlCommand(queryString, _sqlConnection);
            _sqlConnection.Open();
            SqlDataReader reader = await command.ExecuteReaderAsync();

            var items = new List<ItemDto>();

            while (reader.Read())
            {
                items.Add(new ItemDto
                {
                    ItemId = (Guid) reader[0],
                    ItemName = (string) reader[1],
                    ItemPrice = (decimal) reader[2],
                    CategoryName = (string) reader[3]
                });
            }

            reader.Close();
            _sqlConnection.Close();

            return items;
        }

        public async Task<ServerResponse<ItemDto>> GetByIdAsync(Guid id)
        {
            var sqlString = "SELECT A.ItemId, A.ItemName, A.ItemPrice, B.CategoryName FROM dbo.Items AS A " +
                "LEFT JOIN dbo.ItemCategories AS B " +
                "ON A.ItemCategoryId = B.ItemCategoryId " +
                "WHERE A.ItemId = @ItemId";

            SqlCommand command = new SqlCommand(sqlString, _sqlConnection);
            command.Parameters.AddWithValue("@ItemId", id);

            _sqlConnection.Open();
            SqlDataReader reader = await command.ExecuteReaderAsync();

            if (!reader.HasRows) //When there are no records
            {
                reader.Close();
                _sqlConnection.Close();
                return new ServerResponse<ItemDto>
                {
                    IsSuccessful = false,
                    Message = "Item could not be found",
                    Content = null
                };
            }
        
            await reader.ReadAsync();
            var item = new ItemDto
            {
                ItemId = (Guid)reader[0],
                ItemName = (string)reader[1],
                ItemPrice = (decimal)reader[2],
                CategoryName = (string)reader[3]
            };

            reader.Close();
            _sqlConnection.Close();

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

            var queryString = "UPDATE dbo.Items SET ItemName = @ItemName, ItemPrice = @ItemPrice, ItemCategoryId = @ItemCategoryId " +
                "WHERE ItemId = @ItemId";

            SqlCommand command = new SqlCommand(queryString, _sqlConnection);
            command.Parameters.AddWithValue("@ItemName", updateItemDto.ItemName);
            command.Parameters.AddWithValue("@ItemPrice", updateItemDto.ItemPrice);
            command.Parameters.AddWithValue("@ItemCategoryId", updateItemDto.ItemCategoryId);
            command.Parameters.AddWithValue("@ItemId", updateItemDto.ItemId);

            _sqlConnection.Open();

            int rowsAffected = await command.ExecuteNonQueryAsync();

            _sqlConnection.Close();

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
