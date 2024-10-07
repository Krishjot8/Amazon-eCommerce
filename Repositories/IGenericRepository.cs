using Amazon_eCommerce_API.Models;
using Amazon_eCommerce_API.Specifications;
using System.Linq.Expressions;

namespace Amazon_eCommerce_API.Repositories
{
    public interface IGenericRepository<T> where T : BaseModel

    {
        Task<T> GetByIdAsync(int id);

       Task<List<T>> GetAllAsync();

        Task<T> GetEntityWithSpec(ISpecification<T> spec);
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);

        Task<T> AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(int id);

        Task<List<T>> GetAllIncludingAsync(params Expression<Func<T, object>>[] includeProperties);
    }
}
