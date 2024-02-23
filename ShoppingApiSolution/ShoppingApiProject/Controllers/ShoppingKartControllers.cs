// YourApiProject/Controllers/ShoppingCartController.cs

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/[controller]s")]  
public class ShoppingCartController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public ShoppingCartController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    
    [HttpGet]
    public IActionResult Get()
    {
        
        var userEmail = User.Identity.Name;

        
        var ShoppingCart = _dbContext.ShoppingCarts
            .Include(sc => sc.Products) 
            .FirstOrDefault(sc => sc.User == userEmail);

        if (ShoppingCart == null)
        {
            return NotFound("Shopping cart not found for the current user.");
        }

        
        return Ok(ShoppingCart.Products);
    }

    
    [HttpPost("remove")]
    public IActionResult RemoveFromCart(int productId)
    {
        
        var userEmail = User.Identity.Name;

        
        var ShoppingCart = _dbContext.ShoppingCarts
            .Include(sc => sc.Products) 
            .FirstOrDefault(sc => sc.User == userEmail);

        if (ShoppingCart == null)
        {
            return NotFound("Shopping cart not found for the current user.");
        }

        // Find the product in the shopping cart with the specified ID
        var productToRemove = ShoppingCart.Products.FirstOrDefault(p => p.Id == productId);

        if (productToRemove == null)
        {
            return NotFound($"Product with ID {productId} not found in the shopping cart.");
        }

        
        ShoppingCart.Products.Remove(productToRemove);

        
        _dbContext.SaveChanges();

        return Ok($"Product with ID {productId} removed from the shopping cart.");
    }

    
    [HttpPost("add")]
    public IActionResult AddToCart(int productId)
    {
        
        var userEmail = User.Identity.Name;

        
        var ShoppingCart = _dbContext.ShoppingCarts
            .Include(sc => sc.Products) 
            .FirstOrDefault(sc => sc.User == userEmail);

        
        if (ShoppingCart == null)
        {
            ShoppingCart = new ShoppingCart
            {
                User = userEmail
            };

            _dbContext.ShoppingCarts.Add(shoppingCart);
        }

        
        var productToAdd = _dbContext.Products.FirstOrDefault(p => p.Id == productId);

        if (productToAdd == null)
        {
            return NotFound($"Product with ID {productId} not found.");
        }

        
        ShoppingCart.Products.Add(productToAdd);

        
        _dbContext.SaveChanges();

        return Ok($"Product with ID {productId} added to the shopping cart.");
    }
}
