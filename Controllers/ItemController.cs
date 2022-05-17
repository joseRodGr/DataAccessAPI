using DataAccessAPI.Dtos;
using DataAccessAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ItemController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetAll()
        {
            return Ok(await _unitOfWork.ItemRepository.GetAllAsync());

        }

        [HttpGet("{id}", Name = "GetItem")]
        public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
        {
            var response =  await _unitOfWork.ItemRepository.GetByIdAsync(id);

            if (response.IsSuccessful) return Ok(response.Content);

            return NotFound(response.Message);
        
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> Create(CreateItemDto createItemDto)
        {
            var response = await _unitOfWork.ItemRepository.Create(createItemDto);

            if (response.IsSuccessful) return CreatedAtRoute("GetItem", new { id = response?.Content?.ItemId }, response?.Content);

            return BadRequest(response.Message);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, UpdateItemDto updateItemDto)
        {
            var response = await _unitOfWork.ItemRepository.Update(id, updateItemDto);

            if (response.IsSuccessful) return NoContent();

            return NotFound(response.Message);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var response = await _unitOfWork.ItemRepository.Delete(id);

            if (response.IsSuccessful) return NoContent();

            return BadRequest(response.Message);
        }
    }
}
