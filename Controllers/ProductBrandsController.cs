using Amazon_eCommerce_API.Models;
using Amazon_eCommerce_API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Amazon_eCommerce_API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class ProductBrandsController : ControllerBase



    {

        private readonly IGenericRepository<ProductBrand> _productbrandrepository;
     

        public ProductBrandsController( IGenericRepository<ProductBrand> productbrandrepository)

        {
          
            _productbrandrepository = productbrandrepository;
         
        }



      




       
        [HttpGet]

        public async Task<ActionResult<List<ProductBrand>>> GetProductBrands()


        {

            var productbrands = await _productbrandrepository.GetAllAsync();

            return Ok(productbrands);

        }

        //productbrandbyID


        //

        [HttpGet("{id}")]

        public async Task<ActionResult<ProductBrand>> GetProductBrand(int id)
        {



            return await _productbrandrepository.GetByIdAsync(id);

        }







        [HttpPost]

        public async Task<ActionResult<ProductBrand>> AddProductBrand(ProductBrand productbrand)
        {

            if (productbrand == null)
            {

                return BadRequest("The product brand doesn't exist");


            }

            var createdProductbrand = await _productbrandrepository.AddAsync(productbrand);

            return CreatedAtAction(nameof(GetProductBrand), new { id = createdProductbrand.Id }, createdProductbrand);



        }


        [HttpPut("{id}")]

        public async Task<ActionResult> UpdateProductBrand(int id, ProductBrand productbrand)
        {

            if (id != productbrand.Id)
            {

                return BadRequest("ProductBrand ID mismatch");


            }

            await _productbrandrepository.UpdateAsync(productbrand);

            return NoContent();



        }


        [HttpDelete("{id}")]

        public async Task<ActionResult> DeleteProductBrand(int id)
        {
            var productbrand = await _productbrandrepository.GetByIdAsync(id);

            if (productbrand == null)
            {
                return NotFound();
            }

            await _productbrandrepository.DeleteAsync(productbrand.Id);

            return NoContent();






        }




    }
}
