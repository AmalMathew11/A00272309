

using System.Collections.Generic;

public class ShoppingKart
{
    public int Id { get; set; }
    public string User { get; set; }
    public List<Product> Products { get; set; } = new List<Product>();
}
