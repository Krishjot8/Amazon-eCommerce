using Amazon_eCommerce_API.Data;
using Amazon_eCommerce_API.Models;
using Amazon_eCommerce_API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Amazon_eCommerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly StoreContext _storeContext;

        public CategoriesController(IGenericRepository<Category> categoryRepository, StoreContext storeContext)
        {
            _categoryRepository = categoryRepository;
            _storeContext = storeContext;
        }




        [HttpGet]


        public async Task<ActionResult<List<Category>>> GetCategories()
        {

            var categories = await _categoryRepository.GetAllAsync();

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

        public async Task<ActionResult<Category>> AddCategory(Category category)
        {



            var createdcategory = await _categoryRepository.AddAsync(category);

            return CreatedAtAction(nameof(GetCategory), new { id = createdcategory.Id }, createdcategory);



        }


        [HttpPut("{id}")]

        public async Task<ActionResult> UpdateCategory(int id, Category category)
        {



            if (id != category.Id)
            {


                return BadRequest("Category ID Mismatch");



            }
            await _categoryRepository.UpdateAsync(category);

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
                    await _categoryRepository.DeleteAsync(category.Id);
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

                await _categoryRepository.DeleteAsync(category.Id);

                return NoContent();


            }




        }


    }
}
