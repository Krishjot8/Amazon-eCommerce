using Amazon_eCommerce_API.Models.DBEntities.Products;
using Amazon_eCommerce_API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Amazon_eCommerce_API.Controllers.Product
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Models.DBEntities.Products.Product> _productsRepository;

        public ProductsController(IGenericRepository<Models.DBEntities.Products.Product> productsRepository)
        {
            _productsRepository = productsRepository;
        }

        
        
        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<List<Models.DBEntities.Products.Product>>> GetProducts()
        {
            var products = await _productsRepository.GetAllIncludingAsync(
                p => p.ProductBrand,
                p => p.ProductType,
                p => p.Category
            );

            return Ok(products);
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.DBEntities.Products.Product>> GetProduct(int id)
        {
            var product = await _productsRepository.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<Models.DBEntities.Products.Product>> AddProduct(Models.DBEntities.Products.Product product)
        {
            if (product == null)
                return BadRequest("The product doesn't exist.");

            if (product.StockQuantity < 0)
                return BadRequest("Stock quantity cannot be negative.");

            var createdProduct = await _productsRepository.AddAsync(product);

            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
        }

        // PUT: api/products/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, Models.DBEntities.Products.Product product)
        {
            if (id != product.Id)
                return BadRequest("Product ID mismatch.");

            await _productsRepository.UpdateAsync(product);
            return NoContent();
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _productsRepository.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            await _productsRepository.DeleteAsync(product.Id);
            return NoContent();
        }
    }
}