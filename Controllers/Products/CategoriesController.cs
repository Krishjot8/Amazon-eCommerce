using Amazon_eCommerce_API.Data;
using Amazon_eCommerce_API.Models.DBEntities.Products;
using Amazon_eCommerce_API.Models.DTO_s.Products;
using Amazon_eCommerce_API.Models.DTO_s.Products.ProductCategory;
using Amazon_eCommerce_API.Repositories;
using Amazon_eCommerce_API.Services.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Amazon_eCommerce_API.Controllers.Products
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICatalogService _catalogService;
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly StoreContext _storeContext;

        public CategoriesController(IGenericRepository<Category> categoryRepository, StoreContext storeContext, ICatalogService catalogService)
        {
            _categoryRepository = categoryRepository;
            _storeContext = storeContext;
            _catalogService = catalogService;
        }


        
        [HttpGet]


        public async Task<ActionResult<List<Category>>> GetCategories()
        {

            var categories = await _categoryRepository.GetAllAsync();
            
            if (categories == null || !categories.Any())
            {
                return NotFound("No categories found");

            }


            return Ok(categories);


        }



        [HttpGet("{id}")]


        public async Task<ActionResult<List<Category>>> GetCategory(int id)
        {

            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            {


                return NotFound("The category you are looking for doesn't exist");
            }

            return Ok(category);


        }


        [HttpPost]

        public async Task<ActionResult<Category>> AddCategory([FromForm] CategoryCreateDto categoryCreateDto)
        {

            if(categoryCreateDto == null || string.IsNullOrWhiteSpace(categoryCreateDto.Name))
                return BadRequest("The category name is required");
            
            var category = new Category
            {
                Name = categoryCreateDto.Name,
                
            };
           
            var createdCategory = await _catalogService.CreateCategoryAsync(category);
            

            return CreatedAtAction(nameof(GetCategories), new { id = createdCategory.Id }, createdCategory);
        }


        


        [HttpPut("{id}")]

        public async Task<ActionResult> UpdateCategory(int id, Category category)
        {
            if (category == null)
            {
                return BadRequest("The category doesn't exist");
            }

            if (id != category.Id)
            {
                return BadRequest("Category ID Mismatch");

            }
            
            
            await _catalogService.UpdateCategoryAsync(id,category);
            

            return NoContent();
            
        }


        

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            {
                var category =await _categoryRepository.GetByIdAsync(id);

                if (category == null)
                {
                    return NotFound("The category you are looking for does not exist");
                }

                try
                {
                     await _catalogService.DeleteCategoryAsync(id);
                    await _storeContext.SaveChangesAsync();
                }

                catch (DbUpdateException ex)
                {
                    if (ex.InnerException?.Message.Contains("DELETE statement conflicted with the REFERENCE constraint") == true)
                    {
                        return BadRequest(new { message = "Category cannot be deleted because it is associated with one or more products." });
                    }
                    throw;
                }

                await _catalogService.DeleteCategoryAsync(category.Id);

                return NoContent();


            }




        }


    }
}
