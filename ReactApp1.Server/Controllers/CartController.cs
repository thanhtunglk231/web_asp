using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Context;
using ReactApp1.Server.Models;
using System.Security.Claims;

namespace ReactApp1.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get() { 
        var userid=User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userid == null) return BadRequest("Khong co user");
            var CartItem = await _context.CartItems
                .Where(c => c.UserID == userid)
                .Include(c => c.Product)
                .Select(c => new
                {
                    ProductName=c.Product.ProductName,
                    ImgUrl=c.Product.ImageUrl,

                }
                
                )
                .ToListAsync();
            return Ok(CartItem);
        
        }
        [HttpPost("AddCart")]
        public async Task<IActionResult> AddCart([FromBody] CartItem cartItem)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userid == null) return Unauthorized();

            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.UserID == userid && c.ProductID == cartItem.ProductID && c.OptionID == cartItem.OptionID);

            if (existingItem == null)
            {
                cartItem.UserID = userid;
                await _context.CartItems.AddAsync(cartItem);
            }
            else
            {
                existingItem.Quantity += cartItem.Quantity;
            }

            await _context.SaveChangesAsync();
            return Ok(cartItem);
        }



    }
}
