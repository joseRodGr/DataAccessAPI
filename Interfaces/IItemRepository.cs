using DataAccessAPI.Dtos;
using DataAccessAPI.Helpers;
using DataAccessAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessAPI.Interfaces
{
    public interface IItemRepository
    {
        Task<IEnumerable<ItemDto>> GetAllAsync();
        Task<ServerResponse<ItemDto>> GetByIdAsync(Guid id);
        Task<ServerResponse<ItemDto>> Create(CreateItemDto createItemDto);
        Task<ServerResponse<ItemDto>> Update(Guid id, UpdateItemDto updateItemDto);
        Task<ServerResponse<ItemDto>> Delete(Guid id);
    }
}
