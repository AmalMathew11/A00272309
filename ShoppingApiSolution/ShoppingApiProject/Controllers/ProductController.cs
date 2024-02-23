// YourApiProject/Controllers/ProductController.cs

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ShoppingClassLibrary;

[Authorize]
[ApiController]
[Route("api/[controller]s")]  // Adjusted route to maintain consistency
public class ProductController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public ProductController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Get all products
    [HttpGet]
    public IActionResult Get()
    {
        var products = _dbContext.Products.ToList();
        return Ok(products);
    }

    // Get products by category
    [HttpGet("byCategory/{categoryId}")]
    public IActionResult GetByCategory(int categoryId)
    {
        var products = _dbContext.Products
            .Where(p => p.CategoryId == categoryId)
            .ToList();

        return Ok(products);
    }

    // Add a new product
    [HttpPost]
    public IActionResult AddProduct([FromBody] Product product)
    {
        if (product == null)
        {
            return BadRequest("Invalid product data.");
        }

        _dbContext.Products.Add(product);
        _dbContext.SaveChanges();

        return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
    }
}
