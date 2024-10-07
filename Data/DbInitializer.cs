using Amazon_eCommerce_API.Data;
using Amazon_eCommerce_API.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;



public class DbInitializer
{
    private readonly StoreContext _context;

    public DbInitializer(StoreContext context)
    {
        _context = context;
    }

    public async Task InitializeAsync()
    {
        // Ensure the database is created
        await _context.Database.EnsureCreatedAsync();

        // Seed data if the database is empty
        if (!_context.Products.Any())
        {
            await SeedDataFromJsonAsync();
        }
    }

    private async Task SeedDataFromJsonAsync()
    {
        // Example: Path to your JSON files
        var brandsJson = await File.ReadAllTextAsync("Data/SeedData/brands.json");
       var productsJson = await File.ReadAllTextAsync("Data/SeedData/products.json");
        var typesJson = await File.ReadAllTextAsync("Data/SeedData/types.json");

        var productBrands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsJson);
        var productTypes = JsonSerializer.Deserialize<List<ProductType>>(typesJson);
        var products = JsonSerializer.Deserialize<List<Product>>(productsJson);

           if (productBrands != null)
         {
             await _context.ProductBrands.AddRangeAsync(productBrands);
         }
         if (productTypes != null)
         {
             await _context.ProductTypes.AddRangeAsync(productTypes);
         }

        
        if (products != null)
                {
         await _context.Products.AddRangeAsync(products);

                }

        await _context.SaveChangesAsync();
    }
}
