using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Context;
using ReactApp1.Server.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ReactApp1.Server.Dto;
using ReactApp1.Server.services;

namespace ReactApp1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductController> _logger;
        private readonly FirebaseStorageService _firebaseStorageService;
        private readonly SupabaseService _supabaseService;    

        public ProductController(ApplicationDbContext context, ILogger<ProductController> logger, FirebaseStorageService firebaseStorageService, SupabaseService supabaseService    )
        {
            _context = context;
            _logger = logger;
            _firebaseStorageService = firebaseStorageService;
            _supabaseService = supabaseService;
        }
        [HttpGet("Price")]
        public async Task<IActionResult> GetProductByPrice(decimal? minprice,decimal? maxprice)
        {
            if((minprice.HasValue && minprice.Value<0) || (maxprice.HasValue && maxprice.Value < 0)){
                return BadRequest("Gia khong chinh xac");
            }
            var query= _context.Products.AsQueryable();
            if (minprice.HasValue) { 
            query= query.Where(c=>c.Price > minprice.Value);
            }
            if (maxprice.HasValue) { 
            query = query.Where(c=>c.Price < maxprice.Value);
            }
            var product= await query.ToListAsync();
            return Ok(product);
        }
        // GET: api/Product/all
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var canConnect = await _context.Database.CanConnectAsync();
            _logger.LogInformation("Kết nối database thành công: {canConnect}", canConnect);

            try
            {
                var products = await _context.Products.ToListAsync();

                if (products == null || products.Count == 0)
                {
                    _logger.LogWarning("Không tìm thấy sản phẩm nào.");
                    return NotFound("Không tìm thấy sản phẩm nào.");
                }

                _logger.LogInformation("Lấy danh sách sản phẩm thành công. Số lượng: {Count}", products.Count);
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách sản phẩm.");
                return StatusCode(500, "Lỗi máy chủ nội bộ");
            }
        }

        // GET: api/Product/{ProductID}
        [HttpGet("{ProductID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Product>> GetProductById(int ProductID)
        {
            if (ProductID <= 0)
            {
                _logger.LogWarning("ProductID không hợp lệ: {ProductID}", ProductID);
                return BadRequest("ProductID phải lớn hơn 0.");
            }

            try
            {
                var product = await _context.Products.FindAsync(ProductID);

                if (product == null)
                {
                    _logger.LogWarning("Không tìm thấy sản phẩm với ID: {ProductID}", ProductID);
                    return NotFound($"Không tìm thấy sản phẩm với ID: {ProductID}");
                }

                _logger.LogInformation("Lấy sản phẩm với ID {ProductID} thành công.", ProductID);
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy sản phẩm với ID: {ProductID}", ProductID);
                return StatusCode(500, "Lỗi máy chủ nội bộ");
            }


        }


        [HttpPost("upload/{productId}")]
        public async Task<IActionResult> UploadProductImage(int productId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Chưa chọn file");

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                return NotFound("Không tìm thấy sản phẩm");

            try
            {
                var imageUrl = await _supabaseService.UploadFileAsync(file);
                if (string.IsNullOrEmpty(imageUrl))
                    return StatusCode(500, "Lỗi khi upload ảnh lên Supabase");

                product.ImageUrl = imageUrl;
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Upload thành công",
                    imageUrl
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi upload ảnh cho sản phẩm {ProductId}", productId);
                return StatusCode(500, $"Lỗi khi upload ảnh: {ex.Message}");
            }
        }




        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Product>> AddProduct([FromBody] ProductCreateDto productDto)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            if (string.IsNullOrWhiteSpace(productDto.ProductName))
            {
                return BadRequest("Cần có tên sản phẩm.");
            }

            var product = new Product
            {
                ProductName = productDto.ProductName,
                Description = productDto.Description,
                Price = productDto.Price,
                ImageUrl = productDto.ImageUrl,
                CategoryID = productDto.CategoryID,
                IsVegetarian = productDto.IsVegetarian,
                IsBestseller = productDto.IsBestseller,
                CreatedAt = DateTime.Now
            };

            try
            {
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetProductById), new { ProductID = product.ProductID }, product);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi thêm sản phẩm.");
                return StatusCode(500, "Đã xảy ra lỗi khi thêm sản phẩm.");
            }
        }

        [HttpDelete("{producid}")]
        [Authorize]
        public async Task<IActionResult> RemoveProduct(int producid)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized("Bạn cần đăng nhập.");

            if (producid <= 0)
                return BadRequest("ID không hợp lệ.");
            var product = await _context.Products.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.ProductID == producid);

            if (product == null)
                return NotFound("Không tìm thấy sản phẩm với ID đã cho.");


            try
            {
                // Xóa các bản ghi liên quan trước (nếu bạn cho phép)
                var cartItems = _context.CartItems.Where(c => c.ProductID == producid);
                _context.CartItems.RemoveRange(cartItems);

                var orderDetails = _context.OrderDetails.Where(o => o.ProductID == producid);
                _context.OrderDetails.RemoveRange(orderDetails);

                var productOptions = _context.ProductOptions.Where(po => po.ProductID == producid);
                _context.ProductOptions.RemoveRange(productOptions);

                var reviews = _context.Reviews.Where(r => r.ProductID == producid);
                _context.Reviews.RemoveRange(reviews);

                await _context.SaveChangesAsync();

                // Xóa sản phẩm
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return Ok("Xóa sản phẩm thành công.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa sản phẩm ID: {producid}", producid);
                return StatusCode(500, $"Lỗi khi xóa sản phẩm: {ex.Message}");
            }
        }

        [HttpPut("{producid}")]
        [Authorize]
        public async Task<IActionResult> UpdateProduct(int producid, [FromBody] UpdateProductDto product)
        {
            var userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userid == null) return Unauthorized();
            if (product == null) return BadRequest("Du lieu khong dung");
            try
            {
                var preProduct = await _context.Products.FirstOrDefaultAsync(c => c.ProductID == producid);
                if (preProduct == null) return NotFound("Khong co id nay");
                preProduct.ProductName = product.ProductName;
                preProduct.Description = product.Description;
                preProduct.Price = product.Price;
                preProduct.ImageUrl = product.ImageUrl;
                preProduct.CategoryID = product.CategoryID;
                preProduct.IsVegetarian = product.IsVegetarian;
                preProduct.IsBestseller = product.IsBestseller;
                await _context.SaveChangesAsync();
                return Ok("Sửa thành công ");
            }
            catch (Exception ex) { 
            Console.WriteLine(ex.ToString());
                return BadRequest(ex.ToString());
            }

        }

    }
}