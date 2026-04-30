using Amazon_eCommerce_API.Models.DBEntities.Products;
using Amazon_eCommerce_API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Amazon_eCommerce_API.Controllers.Product
{

    [Route("api/[controller]")]
    [ApiController]

    public class ProductBrandsController : ControllerBase



    {

        private readonly IGenericRepository<ProductBrand> _productbBrandRepository;
     

        public ProductBrandsController( IGenericRepository<ProductBrand> ProductbBrandRepository)

        {
          
            _productbBrandRepository = ProductbBrandRepository;
         
        }



      




       
        [HttpGet]

        public async Task<ActionResult<List<ProductBrand>>> GetProductBrands()


        {

            var productBrands = await _productbBrandRepository.GetAllAsync();
            
            if (productBrands == null || !productBrands.Any())
            {
                return NotFound("No product brands found");

            }


            return Ok(productBrands);

        }

        //productbrandbyID


        //

        [HttpGet("{id}")]

        public async Task<ActionResult<ProductBrand>> GetProductBrand(int id)
        {


            return await _productbBrandRepository.GetByIdAsync(id);

        }







        [HttpPost]

        public async Task<ActionResult<ProductBrand>> AddProductBrand(ProductBrand productbrand)
        {

            if (productbrand == null)
            {

                return BadRequest("The product brand doesn't exist");


            }

            var createdProductbrand = await _productbBrandRepository.AddAsync(productbrand);

            return CreatedAtAction(nameof(GetProductBrand), new { id = createdProductbrand.Id }, createdProductbrand);



        }


        [HttpPut("{id}")]

        public async Task<ActionResult> UpdateProductBrand(int id, ProductBrand productbrand)
        {

            if (id != productbrand.Id)
            {

                return BadRequest("ProductBrand ID mismatch");


            }

            await _productbBrandRepository.UpdateAsync(productbrand);

            return NoContent();

        }



        [HttpDelete("{id}")]

        public async Task<ActionResult> DeleteProductBrand(int id)
        {
            var productbrand = await _productbBrandRepository.GetByIdAsync(id);

            if (productbrand == null)
            {
                return NotFound();
            }

            await _productbBrandRepository.DeleteAsync(productbrand.Id);

            return NoContent();


        }




    }
}
