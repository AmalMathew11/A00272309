// YourApiProject/Controllers/ShoppingCartController.cs

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using YourClassLibrary;

[Authorize]
[ApiController]
[Route("api/[controller]s")]  // Adjusted route to maintain consistency
public class ShoppingCartController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public ShoppingCartController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Get all products in the user's shopping cart
    [HttpGet]
    public IActionResult Get()
    {
        // Get the current user's email (assuming you have user authentication set up)
        var userEmail = User.Identity.Name;

        // Retrieve the shopping cart for the user from the database
        var shoppingCart = _dbContext.ShoppingCarts
            .Include(sc => sc.Products) // Include products to avoid lazy loading
            .FirstOrDefault(sc => sc.User == userEmail);

        if (shoppingCart == null)
        {
            return NotFound("Shopping cart not found for the current user.");
        }

        // Return the products in the shopping cart
        return Ok(shoppingCart.Products);
    }

    // Add a Post endpoint that takes a single ID and removes the item from the shopping cart
    [HttpPost("remove")]
    public IActionResult RemoveFromCart(int productId)
    {
        // Get the current user's email (assuming you have user authentication set up)
        var userEmail = User.Identity.Name;

        // Retrieve the shopping cart for the user from the database
        var shoppingCart = _dbContext.ShoppingCarts
            .Include(sc => sc.Products) // Include products to avoid lazy loading
            .FirstOrDefault(sc => sc.User == userEmail);

        if (shoppingCart == null)
        {
            return NotFound("Shopping cart not found for the current user.");
        }

        // Find the product in the shopping cart with the specified ID
        var productToRemove = shoppingCart.Products.FirstOrDefault(p => p.Id == productId);

        if (productToRemove == null)
        {
            return NotFound($"Product with ID {productId} not found in the shopping cart.");
        }

        // Remove the product from the shopping cart
        shoppingCart.Products.Remove(productToRemove);

        // Save changes to the database
        _dbContext.SaveChanges();

        return Ok($"Product with ID {productId} removed from the shopping cart.");
    }

    // Add a Post endpoint that takes a single ID and adds the item to the shopping cart
    [HttpPost("add")]
    public IActionResult AddToCart(int productId)
    {
        // Get the current user's email (assuming you have user authentication set up)
        var userEmail = User.Identity.Name;

        // Retrieve the shopping cart for the user from the database
        var shoppingCart = _dbContext.ShoppingCarts
            .Include(sc => sc.Products) // Include products to avoid lazy loading
            .FirstOrDefault(sc => sc.User == userEmail);

        // If the shopping cart doesn't exist, create a new one
        if (shoppingCart == null)
        {
            shoppingCart = new ShoppingCart
            {
                User = userEmail
            };

            _dbContext.ShoppingCarts.Add(shoppingCart);
        }

        // Find the product to add by ID
        var productToAdd = _dbContext.Products.FirstOrDefault(p => p.Id == productId);

        if (productToAdd == null)
        {
            return NotFound($"Product with ID {productId} not found.");
        }

        // Add the product to the shopping cart
        shoppingCart.Products.Add(productToAdd);

        // Save changes to the database
        _dbContext.SaveChanges();

        return Ok($"Product with ID {productId} added to the shopping cart.");
    }
}
