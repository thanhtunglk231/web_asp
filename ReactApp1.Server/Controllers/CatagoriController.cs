using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Context;
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

    }
}
