namespace SimpleInventoryManagement.Model
{
    public class ProductModel
    {
        public string? Name { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }

        public ProductModel(string? name, int price, int stock)
        {
            Name = name;
            Price = price;
            Stock = stock;
        }
    }
}
