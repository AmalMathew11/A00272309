

using Xunit;
using ShoppingClassLibrary.Models; 

public class ShoppingKartTests
{
    [Fact]
    public void ShoppingKart_UserIsSetCorrectly()
    {
        
        var shoppingKart = new ShoppingKart { User = "testuser@example.com" };

        
        var user = shoppingKart.User;

        
        Assert.Equal("testuser@example.com", user);
    }

    
}
