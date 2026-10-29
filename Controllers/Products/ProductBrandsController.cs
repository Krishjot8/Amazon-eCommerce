using Amazon_eCommerce_API.Models.DBEntities.Products;
using Amazon_eCommerce_API.Models.DTO_s.Products;
using Amazon_eCommerce_API.Models.DTO_s.Products.ProductBrand;
using Amazon_eCommerce_API.Repositories;
using Amazon_eCommerce_API.Services.Products;
using Microsoft.AspNetCore.Mvc;

namespace Amazon_eCommerce_API.Controllers.Products
{

    [Route("api/[controller]")]
    [ApiController]

    public class ProductBrandsController : ControllerBase

    {

        private readonly IGenericRepository<ProductBrand> _productBrandRepository;
        private readonly ICatalogService _catalogService;
     

        public ProductBrandsController( IGenericRepository<ProductBrand> ProductBrandRepository, ICatalogService catalogService)

        {
            _productBrandRepository = ProductBrandRepository;
            _catalogService = catalogService;
        }



      




       
        [HttpGet]

        public async Task<ActionResult<List<ProductBrand>>> GetProductBrands()
        
        {
            var productBrands = await _productBrandRepository.GetAllAsync();
            
            if (productBrands == null || !productBrands.Any())
            {
                return NotFound("No product brands found");

            }


            return Ok(productBrands);

        }
        

        [HttpGet("{id}")]

        public async Task<ActionResult<ProductBrand>> GetProductBrand(int id)
        {

            return await _productBrandRepository.GetByIdAsync(id);

        }







        [HttpPost]

        public async Task<ActionResult<ProductBrand>> AddProductBrand([FromForm] ProductBrandCreateDto productBrandCreateDto)
        {

            if (productBrandCreateDto == null)
            {
                return BadRequest("The product brand you are looking for doesn't exist");

            }

            if (string.IsNullOrWhiteSpace(productBrandCreateDto.ProductBrand))
            {
                return BadRequest("The product brand name is required");
            }
            
            var productBrand = new ProductBrand
            {
                Name = productBrandCreateDto.ProductBrand,
                LogoUrl = productBrandCreateDto.LogoUrl,
                Description = productBrandCreateDto.Description
            };

            var createdProductBrand = await _catalogService.CreateProductBrandAsync(productBrand);

            if (createdProductBrand != null)
            {
                return CreatedAtAction(nameof(GetProductBrand), new { id = createdProductBrand.Id },
                    createdProductBrand);
            }
            
            return BadRequest("Failed to create the product brand");
        }


        [HttpPut("{id}")]

        public async Task<ActionResult> UpdateProductBrand( int id,[FromForm] ProductBrandUpdateDto productBrandUpdateDto)
        {
            if (productBrandUpdateDto == null)
            {
                return BadRequest("The product brand you are looking for doesn't exist");
            }

            var existingProductBrand = await _productBrandRepository.GetByIdAsync(id);

            if (existingProductBrand == null)
            {
                return NotFound($"Product brand with ID {id} not found.");
            }

            existingProductBrand.Name = productBrandUpdateDto.ProductBrand;
            existingProductBrand.LogoUrl = productBrandUpdateDto.LogoUrl;
            existingProductBrand.Description = productBrandUpdateDto.Description;

            await _catalogService.UpdateProductBrandAsync(id, existingProductBrand);

            return NoContent();
        }
       



        [HttpDelete("{id}")]

        public async Task<ActionResult> DeleteProductBrand(int id)
        {
            var productBrand = await _productBrandRepository.GetByIdAsync(id);

            if (productBrand == null)
            {
                return NotFound();
            }

            await _catalogService.DeleteProductBrandAsync(productBrand.Id);

            return NoContent();


        }




    }
}
