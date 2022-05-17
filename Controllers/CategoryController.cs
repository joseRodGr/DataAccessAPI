using DataAccessAPI.Dtos;
using DataAccessAPI.Interfaces;
using DataAccessAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemCategory>>> GetAll()
        {
            return Ok(await _unitOfWork.CategoryRepository.GetAllAsync());
        }

        [HttpGet("{id}", Name = "GetCategory")]
        public async Task<ActionResult<ItemCategory>> GetById(int id)
        {
            var response = await _unitOfWork.CategoryRepository.GetByIdAsync(id);

            if (response.IsSuccessful) return Ok(response.Content);

            return NotFound(response.Message);
        }

        [HttpPost]
        public async Task<ActionResult<ItemCategory>> Create(CreateCategoryDto createCategoryDto)
        {
            var response = await _unitOfWork.CategoryRepository.Create(createCategoryDto);

            if (response.IsSuccessful) return CreatedAtRoute("GetCategory", new { Id = response.Content.ItemCategoryId }, response.Content);

            return BadRequest(response.Message);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateCategoryDto updateCategoryDto)
        {
            var response = await _unitOfWork.CategoryRepository.Update(id, updateCategoryDto);

            if (response.IsSuccessful) return NoContent();

            return NotFound(response.Message);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var response = await _unitOfWork.CategoryRepository.Delete(id);

            if (response.IsSuccessful) return NoContent();

            return BadRequest(response.Message);
        }
    }
}
