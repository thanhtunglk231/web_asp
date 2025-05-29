using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
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
                    ProductId= c.ProductID,
                    ProductName=c.Product.ProductName,
                    ImgUrl=c.Product.ImageUrl,
                    Price=c.Product.Price,
                    Quantity = c.Quantity
                }
                
                )
                .ToListAsync();
            return Ok(CartItem);
        
        }
        [HttpPost("AddCart")]
        public async Task<IActionResult> AddCart([FromBody] CartItem cartItem)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            Console.WriteLine("User ID từ token: " + userId);
            if (userId == null)
                return Unauthorized("User is not authenticated.");

            // Kiểm tra user có tồn tại trong DB không
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return Unauthorized("User does not exist in the system.");

            if (cartItem.Quantity <= 0)
                return BadRequest("Quantity must be greater than 0.");

            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.UserID == userId && c.ProductID == cartItem.ProductID);

            if (existingItem == null)
            {
                cartItem.UserID = userId;
                await _context.CartItems.AddAsync(cartItem);
            }
            else
            {
                existingItem.Quantity += cartItem.Quantity;
            }

            await _context.SaveChangesAsync();
            return Ok(existingItem ?? cartItem);
        }

        [HttpPut("UpdateCart")]
        public async Task<IActionResult> UpdateCart([FromBody] UpdateQuantityRequest item)
        {
            var userId = User.Claims.FirstOrDefault(c=>c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();
            var CartItem= await _context.CartItems.FirstOrDefaultAsync(c=>c.UserID == userId && c.ProductID==item.ProductId);
            if (CartItem == null) return NotFound("Không có sản phẩm");
            if (item.Quantity < 0) return BadRequest("Số lượng không nhỏ hơn 0 ");
            CartItem.Quantity=item.Quantity;
            await _context.SaveChangesAsync();
            return Ok(CartItem);
        }
        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteCartItem(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized("User is not authenticated.");

            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.UserID == userId && c.ProductID == productId);

            if (cartItem == null) return NotFound("Không tìm thấy sản phẩm trong giỏ hàng.");

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            return Ok("Xóa sản phẩm khỏi giỏ hàng thành công.");
        }

    }
}
