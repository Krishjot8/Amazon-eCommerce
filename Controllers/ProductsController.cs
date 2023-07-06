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
        private readonly IGenericRepository<ProductBrand> _productbrandrepository;
        private readonly IGenericRepository<ProductType> _producttyperepository;

        public ProductsController(IGenericRepository<Product> productsrepository,
            IGenericRepository<ProductBrand> productbrandrepository,
            IGenericRepository<ProductType> producttyperepository)       //assigning repositories


        {
            _productsrepository = productsrepository;
            _productbrandrepository = productbrandrepository;
            _producttyperepository = producttyperepository;
        }

        
        [HttpGet]

        public async Task<ActionResult<List<Product>>> GetProducts()
        {

            var products = await _productsrepository.GetAllAsync();

            return Ok(products);

        }

        [HttpGet("{id}")]
        
        public async Task <ActionResult<Product>> GetProduct(int id)
        {


            
            return await _productsrepository.GetByIdAsync(id);

        }
        


    }
}
