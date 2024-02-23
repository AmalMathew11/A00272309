

using Xunit;
using ShoppingClassLibrary.Models;

public class CategoryTests
{
    [Fact]
    public void Category_IdIsSetCorrectly()
    {
        
        var category = new Category { Id = 1, Description = "Test Category" };

       
        var categoryId = category.Id;

        
        Assert.Equal(1, categoryId);
    }

   
}
