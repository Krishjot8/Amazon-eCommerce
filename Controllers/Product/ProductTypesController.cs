using Amazon_eCommerce_API.Models.DBEntities.Products;
using Amazon_eCommerce_API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Amazon_eCommerce_API.Controllers.Product
{

    [Route("api/[controller]")]
    [ApiController]

    public class ProductTypesController : ControllerBase



    {
       
        private readonly IGenericRepository<ProductType> _productTypeRepository;

        public ProductTypesController( IGenericRepository<ProductType> productTypeRepository)       //assigning repositories
        
        {
            _productTypeRepository = productTypeRepository;
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

        //productTypebyID

        //

        [HttpGet("{id}")]

        public async Task<ActionResult<ProductType>> GetProductType(int id)
        {



            return await _productTypeRepository.GetByIdAsync(id);

        }


      



       


        [HttpPost]

        public async Task<ActionResult<ProductType>> AddProductType(ProductType producttype)
        {

            if (producttype == null)
            {

                return BadRequest("The product type doesn't exist");


            }

            var createdProducttype = await _productTypeRepository.AddAsync(producttype);

            return CreatedAtAction(nameof(GetProductType), new { id = createdProducttype.Id }, createdProducttype);



        }

       




        [HttpPut("{id}")]

        public async Task<ActionResult> UpdateProductType(int id, ProductType producttype)
        {

            if (id != producttype.Id)
            {

                return BadRequest("ProductType ID mismatch");


            }

            await _productTypeRepository.UpdateAsync(producttype);

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

            await _productTypeRepository.DeleteAsync(productType.Id);

            return NoContent();


           



        }
    }
}
