using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Context;
using ReactApp1.Server.Dto;
using ReactApp1.Server.Models;
using System.Numerics;

namespace ReactApp1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatagoriController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CatagoriController(ApplicationDbContext applicationDbContext) {
            _context = applicationDbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<string>>> GetCategoryNames()
        {
            var names = await _context.Categories.Select(c => c.CategoryName).ToListAsync();
            return Ok(names);
        }
        [HttpGet("ProductName")]
        public async Task<ActionResult<List<string>>> GetByName(string cartagoriName) 
        {
            var products= await _context.Products.Where(c=>c.Category.CategoryName==cartagoriName).ToListAsync();
            return Ok(products);
        }
        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound("Không tìm thấy danh mục.");
            return category;
        }

      
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Category>> CreateCategory(CategoryCreateDto dto)
        {
            var category = new Category
            {
                CategoryName = dto.CategoryName,
                Description = dto.Description,
                IsActive = dto.IsActive
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategory), new { id = category.CategoryID }, category);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateCategory(int id, CategoryUpdateDto dto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound("Không tìm thấy danh mục.");

            category.CategoryName = dto.CategoryName;
            category.Description = dto.Description;
            category.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();
            return Ok("Cập nhật danh mục thành công.");
        }

        
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound("Không tìm thấy danh mục.");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return Ok("Xóa danh mục thành công.");
        }

    }
}
