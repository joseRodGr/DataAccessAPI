using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    public class ItemRepositoryEF : IItemRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ItemRepositoryEF(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ServerResponse<ItemDto>> Create(CreateItemDto createItemDto)
        {
            var item = _mapper.Map<Item>(createItemDto);
            _context.Items.Add(item);

            if (await SaveAsync())
                return new ServerResponse<ItemDto>
                {
                    IsSuccessful = true,
                    Message = null,
                    Content = _mapper.Map<ItemDto>(item)
                };

            return new ServerResponse<ItemDto>
            {
                IsSuccessful = false,
                Message = "Failed to create a new Item",
                Content = null
            };
        }

        public async Task<ServerResponse<ItemDto>> Delete(Guid id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(x => x.ItemId == id);

            if (item == null)
                return new ServerResponse<ItemDto>
                {
                    IsSuccessful = false,
                    Message = "Item could not be found",
                    Content = null
                };

            _context.Items.Remove(item);

            if (await SaveAsync())
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
            return await _context.Items
                .ProjectTo<ItemDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<ServerResponse<ItemDto>> GetByIdAsync(Guid id)
        {
            var itemDto = await _context.Items
                .ProjectTo<ItemDto>(_mapper.ConfigurationProvider)
                .Where(x => x.ItemId == id).FirstOrDefaultAsync();

            if (itemDto != null)
                return new ServerResponse<ItemDto>
                {
                    IsSuccessful = true,
                    Message = null,
                    Content = itemDto
                };

            return new ServerResponse<ItemDto>
            {
                IsSuccessful = false,
                Message = "Item could not be found",
                Content = null
            };
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
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

            var item = await _context.Items.FirstOrDefaultAsync(x => x.ItemId == id);

            if (item == null)
                return new ServerResponse<ItemDto>
                {
                    IsSuccessful = false,
                    Message = "Item could not be found",
                    Content = null
                };

            _mapper.Map(updateItemDto, item);
            _context.Attach(item);
            _context.Entry(item).State = EntityState.Modified;

            if (await SaveAsync())
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
