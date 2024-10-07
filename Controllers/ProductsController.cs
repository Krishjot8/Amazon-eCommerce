using Amazon_eCommerce_API.Models;
using Amazon_eCommerce_API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Amazon_eCommerce_API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class ProductsController : ControllerBase



    {
        private readonly IGenericRepository<Product> _productsrepository;
       

        public ProductsController(IGenericRepository<Product> productsrepository
           )       //assigning repositories


        {
            _productsrepository = productsrepository;
           
        }



        [HttpGet]

        public async Task<ActionResult<List<Product>>> GetProducts()


        {

            var products = await _productsrepository.GetAllIncludingAsync(
            p => p.ProductBrand,
            p => p.ProductType,
            p => p.Category
        );

            return Ok(products);

        }



        [HttpGet("{id}")]

        public async Task<ActionResult<Product>> GetProduct(int id)
        {



            return await _productsrepository.GetByIdAsync(id);

        }

       
    

        [HttpPost]

        public async Task<ActionResult<Product>> AddProduct(Product product) {

            if (product == null) {

                return BadRequest("The Product doesn't exist");

               


            }

            if (product.StockQuantity < 0)
            {
                return BadRequest("Stock quantity cannot be negative.");
            }

            var createdProduct = await _productsrepository.AddAsync(product);

            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);



        }






        //Update

        [HttpPut("{id}")]

        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {

            if (id != product.Id)
            {

                return BadRequest("Product ID mismatch");


            }

             await _productsrepository.UpdateAsync(product);

            return NoContent();



        }

        [HttpDelete("{id}")]

        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _productsrepository.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            await _productsrepository.DeleteAsync(product.Id);

            return NoContent();






        }



    }
}
