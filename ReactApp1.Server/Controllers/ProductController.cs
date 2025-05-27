using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Context;
using ReactApp1.Server.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ReactApp1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductController> _logger;

        public ProductController(ApplicationDbContext context, ILogger<ProductController> logger)
        {
            _context = context;
            _logger = logger;
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
    }
}