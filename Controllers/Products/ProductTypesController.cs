using Amazon_eCommerce_API.Models.DBEntities.Products;
using Amazon_eCommerce_API.Models.DTO_s.Products.ProductType;
using Amazon_eCommerce_API.Repositories;
using Amazon_eCommerce_API.Services.Products;
using Microsoft.AspNetCore.Mvc;

namespace Amazon_eCommerce_API.Controllers.Products
{

    [Route("api/[controller]")]
    [ApiController]

    public class ProductTypesController : ControllerBase

    
    {
       
        private readonly IGenericRepository<ProductType> _productTypeRepository;
        private readonly ICatalogService _catalogService;

        public ProductTypesController( IGenericRepository<ProductType> productTypeRepository, ICatalogService catalogService)       

        {
            _productTypeRepository = productTypeRepository;
            _catalogService = catalogService;
        }
        

        [HttpGet]

        public async Task<ActionResult<List<ProductType>>> GetProductTypes()


        {

            var productTypes = await _productTypeRepository.GetAllAsync();
            
            if (productTypes == null || !productTypes.Any())
            {
                return NotFound("No product types found");

            }

            return Ok(productTypes);

        }
        
        [HttpGet("{id}")]

        public async Task<ActionResult<ProductType>> GetProductType(int id)
        {

            return await _productTypeRepository.GetByIdAsync(id);

        }
        


        [HttpPost]

        public async Task<ActionResult<ProductType>> AddProductType([FromForm] ProductTypeCreateDto productTypeCreateDto)
        {
            if (productTypeCreateDto == null)
            {
                return BadRequest("The product type doesn't exist");
            }

            var productType = new ProductType
            {
                Name = productTypeCreateDto.ProductType
            };

            var createdProductType = await _catalogService.CreateProductTypeAsync(productType);

            return CreatedAtAction(nameof(GetProductType), new { id = createdProductType.Id }, createdProductType);
        }
       
       




        [HttpPut("{id}")]

        public async Task<ActionResult> UpdateProductType(int id, ProductType productType)
        {
            
            if (productType == null)
            {
                return BadRequest("The product type doesn't exist");
            }

            if (id != productType.Id)
            {
                return BadRequest("ProductType ID mismatch");
            }

            await _catalogService.UpdateProductTypeAsync(id, productType);

            return NoContent();



        }

        [HttpDelete("{id}")]

        public async Task<ActionResult> DeleteProductType(int id)
        {
            var productType = await _productTypeRepository.GetByIdAsync(id);

            if (productType == null)
            {
                return NotFound();
            }

            await _catalogService.DeleteProductTypeAsync(productType.Id);

            return NoContent();


           



        }
    }
}
