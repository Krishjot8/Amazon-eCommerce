using Amazon_eCommerce_API.Models.DBEntities.Products;
using Amazon_eCommerce_API.Models.DTO_s.Products;
using Amazon_eCommerce_API.Repositories;
using Amazon_eCommerce_API.Services.Products;
using Microsoft.AspNetCore.Mvc;

namespace Amazon_eCommerce_API.Controllers.Products
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Product> _productsRepository;
        private readonly IProductService _productService;

        public ProductsController(IGenericRepository<Product> productsRepository, IProductService productService)
        {
            _productsRepository = productsRepository;
            _productService = productService;
        }

        
        
        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await _productsRepository.GetAllIncludingAsync(
                p => p.ProductBrand,
                p => p.ProductType,
                p => p.Category
            );
            
            if (products == null || !products.Any())
            {
                return NotFound("No products found");

            }


            return Ok(products);
        }

     
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productsRepository.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

  
        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct([FromForm] ProductCreateDto createDto)
        {
            if (createDto == null)
                return BadRequest("The product data doesn't exist.");

            if (createDto.StockQuantity < 0)
                return BadRequest("Stock quantity cannot be negative.");

            var createdProduct = await _productService.CreateProductAsync(createDto);

            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
        }

       
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id,[FromForm] ProductUpdateDto updateDto)
        {
           if(updateDto == null)
                return BadRequest("The product data doesn't exist.");

           var updatedProduct = await _productService.UpdateProductAsync(id ,updateDto);
          
          if(updatedProduct == null)
            return NotFound($"Product with ID {id} not found.");
          
          return NoContent();
        }

        
       
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _productsRepository.GetByIdAsync(id);

            if (product == null)
                return NotFound();
            

            await _productService.DeleteProductAsync(id);
            return NoContent();
        }
    }
}