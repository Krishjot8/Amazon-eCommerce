using Amazon_eCommerce_API.Models;
using Amazon_eCommerce_API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Amazon_eCommerce_API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class ProductTypesController : ControllerBase



    {
       
        private readonly IGenericRepository<ProductType> _producttyperepository;

        public ProductTypesController( IGenericRepository<ProductType> producttyperepository)       //assigning repositories


        {
          
        
            _producttyperepository = producttyperepository;
        }




        [HttpGet]

        public async Task<ActionResult<List<ProductType>>> GetProductTypes()


        {

            var producttypes = await _producttyperepository.GetAllAsync();

            return Ok(producttypes);

        }

        //productTypebyID

        //

        [HttpGet("{id}")]

        public async Task<ActionResult<ProductType>> GetProductType(int id)
        {



            return await _producttyperepository.GetByIdAsync(id);

        }


      



       


        [HttpPost]

        public async Task<ActionResult<ProductType>> AddProductType(ProductType producttype)
        {

            if (producttype == null)
            {

                return BadRequest("The product type doesn't exist");


            }

            var createdProducttype = await _producttyperepository.AddAsync(producttype);

            return CreatedAtAction(nameof(GetProductType), new { id = createdProducttype.Id }, createdProducttype);



        }

       




        [HttpPut("{id}")]

        public async Task<ActionResult> UpdateProductType(int id, ProductType producttype)
        {

            if (id != producttype.Id)
            {

                return BadRequest("ProductType ID mismatch");


            }

            await _producttyperepository.UpdateAsync(producttype);

            return NoContent();



        }

        [HttpDelete("{id}")]

        public async Task<ActionResult> DeleteProductType(int id)
        {
            var productType = await _producttyperepository.GetByIdAsync(id);

            if (productType == null)
            {
                return NotFound();
            }

            await _producttyperepository.DeleteAsync(productType.Id);

            return NoContent();


           



        }
    }
}
