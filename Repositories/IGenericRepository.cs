
using Amazon_eCommerce_API.Specifications;
using System.Linq.Expressions;
using Amazon_eCommerce_API.Models.BaseEntities;

namespace Amazon_eCommerce_API.Repositories
{
    public interface IGenericRepository<T> where T : BaseModel
    
   // generic repository used for straightforward lookup tables like categories, products, etc.
    {
        Task<T> GetByIdAsync(int id);

       Task<List<T>> GetAllAsync();

        Task<T> GetEntityWithSpec(ISpecification<T> spec);
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);

      
        Task<List<T>> GetAllIncludingAsync(params Expression<Func<T, object>>[] includeProperties);
    }
}
