

using Xunit;
using ShoppingClassLibrary.Models; 

public class ProductTests
{
    [Fact]
    public void Product_IdIsSetCorrectly()
    {
        
        var product = new Product { Id = 1, Name = "Test Product", Price = 10.99, Description = "Test Description" };

        
        var productId = product.Id;

        
        Assert.Equal(1, productId);
    }

    
}
