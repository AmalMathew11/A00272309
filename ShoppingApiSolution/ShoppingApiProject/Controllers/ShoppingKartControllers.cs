// YourApiProject/Controllers/ShoppingCartController.cs

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourClassLibrary; 

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ShoppingKartController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public ShoppingKartController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    
    [HttpGet]
    public IActionResult Get()
    {
        
        var userEmail = User.Identity.Name;

        
        var shoppingKart = _dbContext.ShoppingKarts
            .Include(sc => sc.Products) 
            .FirstOrDefault(sc => sc.User == userEmail);

        if (shoppingKart == null)
        {
            return NotFound("Shopping kart not found for the current user.");
        }

        
        return Ok(shoppingKart.Products);
    }

    
    [HttpPost("remove")]
    public IActionResult RemoveFromCart(int productId)
    {
        
        var userEmail = User.Identity.Name;

        
        var shoppingKart = _dbContext.ShoppingKarts
            .Include(sc => sc.Products) 
            .FirstOrDefault(sc => sc.User == userEmail);

        if (shoppingKart == null)
        {
            return NotFound("Shopping cart not found for the current user.");
        }

        
        var productToRemove = shoppingKart.Products.FirstOrDefault(p => p.Id == productId);

        if (productToRemove == null)
        {
            return NotFound($"Product with ID {productId} not found in the shopping cart.");
        }

        
        shoppingKart.Products.Remove(productToRemove);

        
        _dbContext.SaveChanges();

        return Ok($"Product with ID {productId} removed from the shopping cart.");
    }

    
    [HttpPost("add")]
    public IActionResult AddToCart(int productId)
    {
        
        var userEmail = User.Identity.Name;

        
        var shoppingKart = _dbContext.ShoppingKarts
            .Include(sc => sc.Products) 
            .FirstOrDefault(sc => sc.User == userEmail);

        
        if (shoppingKart == null)
        {
            shoppingKart = new ShoppingKart
            {
                User = userEmail
            };

            _dbContext.ShoppingKarts.Add(shoppingKart);
        }

       
        var productToAdd = _dbContext.Products.FirstOrDefault(p => p.Id == productId);

        if (productToAdd == null)
        {
            return NotFound($"Product with ID {productId} not found.");
        }

        
        shoppingKart.Products.Add(productToAdd);

       
        _dbContext.SaveChanges();

        return Ok($"Product with ID {productId} added to the shopping kart.");
    }

    

}
