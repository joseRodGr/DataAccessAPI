using AutoMapper;
using DataAccessAPI.Dtos;
using DataAccessAPI.Helpers;
using DataAccessAPI.Interfaces;
using DataAccessAPI.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessAPI.Repositories
{
    public class CategoryRepositoryADO : ICategoryRepository
    {
        private readonly SqlConnection _sqlConnection;
        private readonly IMapper _mapper;

        public CategoryRepositoryADO(IConfiguration configuration, IMapper mapper)
        {
            _sqlConnection = new SqlConnection(configuration.GetConnectionString("DbConnection"));
            _mapper = mapper;
        }
        public async Task<ServerResponse<ItemCategory>> Create(CreateCategoryDto createCategoryDto)
        {
            var queryString = "INSERT INTO dbo.ItemCategories(CategoryName, Description) VALUES(@CategoryName, @Description)";

            SqlCommand command = new SqlCommand(queryString, _sqlConnection);
            command.Parameters.AddWithValue("@CategoryName", createCategoryDto.CategoryName);
            command.Parameters.AddWithValue("@Description", createCategoryDto.Description);

            _sqlConnection.Open();

            var rowsAffected = await command.ExecuteNonQueryAsync();

            _sqlConnection.Close();

            if(rowsAffected > 0)
            {
                var category = _mapper.Map<ItemCategory>(createCategoryDto);

                SqlCommand command2 = new SqlCommand("SELECT IDENT_CURRENT('ItemCategories')", _sqlConnection);
                _sqlConnection.Open();

                SqlDataReader reader = await command2.ExecuteReaderAsync();
                
                await reader.ReadAsync();
                category.ItemCategoryId = reader.HasRows ? (int)(decimal)reader[0] : 0;

                reader.Close();
                _sqlConnection.Close();

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
                Message = "Failed to create a category",
                Content = null
            };

        }

        public async Task<ServerResponse<ItemCategory>> Delete(int id)
        {
            var queryString = "DELETE FROM dbo.ItemCategories WHERE ItemCategoryId = @ItemCategoryId";

            SqlCommand command = new SqlCommand(queryString, _sqlConnection);
            command.Parameters.AddWithValue("@ItemCategoryId", id);

            _sqlConnection.Open();

            var rowsAffected = await command.ExecuteNonQueryAsync();

            _sqlConnection.Close();

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
            var queryString = "SELECT ItemCategoryId, CategoryName, Description FROM dbo.ItemCategories";

            SqlCommand command = new SqlCommand(queryString, _sqlConnection);
            _sqlConnection.Open();
            SqlDataReader reader = await command.ExecuteReaderAsync();

            var categories = new List<ItemCategory>();

            while (reader.Read())
            {
                categories.Add(new ItemCategory
                {
                    ItemCategoryId = (int)reader[0],
                    CategoryName = (string)reader[1],
                    Description = (string)reader[2]
                });
            }

            reader.Close();
            _sqlConnection.Close();

            return categories;

        }

        public async Task<ServerResponse<ItemCategory>> GetByIdAsync(int id)
        {
            var queryString = "SELECT ItemCategoryId, CategoryName, Description FROM dbo.ItemCategories " +
                "WHERE ItemCategoryId = @ItemCategoryId";

            SqlCommand command = new SqlCommand(queryString, _sqlConnection);
            command.Parameters.AddWithValue("@ItemCategoryId", id);

            _sqlConnection.Open();

            SqlDataReader reader = await command.ExecuteReaderAsync();

            if (!reader.HasRows) //Where there are no records
            {
                reader.Close();
                _sqlConnection.Close();
                return new ServerResponse<ItemCategory>
                {
                    IsSuccessful = false,
                    Message = "Category could not be found",
                    Content = null
                };
            }

            await reader.ReadAsync();

            var category = new ItemCategory
            {
                ItemCategoryId = (int)reader[0],
                CategoryName = (string)reader[1],
                Description = (string)reader[2]
            };

            reader.Close();
            _sqlConnection.Close();

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

            var queryString = "UPDATE dbo.ItemCategories SET CategoryName = @CategoryName, Description = @Description " +
                "WHERE ItemCategoryId = @ItemCategoryId";

            SqlCommand command = new SqlCommand(queryString, _sqlConnection);
            command.Parameters.AddWithValue("@CategoryName", updateCategoryDto.CategoryName);
            command.Parameters.AddWithValue("@Description", updateCategoryDto.Description);
            command.Parameters.AddWithValue("@ItemCategoryId", updateCategoryDto.ItemCategoryId);

            _sqlConnection.Open();

            var rowsAffected = await command.ExecuteNonQueryAsync();

            _sqlConnection.Close();

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
