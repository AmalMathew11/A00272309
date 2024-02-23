

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

    

}
