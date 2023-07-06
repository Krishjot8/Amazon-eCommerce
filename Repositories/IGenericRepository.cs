using Amazon_eCommerce_API.Models;

namespace Amazon_eCommerce_API.Repositories
{
    public interface IGenericRepository<T> where T : BaseModel

    {
        Task<T> GetByIdAsync(int id);
       Task<List<T>> GetAllAsync();


    }
}
