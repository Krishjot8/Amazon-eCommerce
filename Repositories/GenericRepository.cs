using Amazon_eCommerce_API.Data;
using Amazon_eCommerce_API.Models;
using Amazon_eCommerce_API.Specifications;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Amazon_eCommerce_API.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseModel
    {
        private readonly StoreContext _context;

        public GenericRepository(StoreContext storeContext)
        {
            _context = storeContext;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
           var entity = await _context.Set<T>().FindAsync(id);

            if (entity != null)
            {

                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
            }
            else {
               throw new KeyNotFoundException("The Item you are trying to delete does not exist.");
            };
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<List<T>> GetAllIncludingAsync(params Expression<Func<T, object>>[] includeProperties)
        {
           IQueryable<T> query = _context.Set<T>();

            foreach(var includeProperty in includeProperties) 
            {
            
            query  = query.Include(includeProperty);
            }


            return await query.ToListAsync();

        }

        public async Task<T> GetByIdAsync(int id)

        {
            return await _context.Set<T>().FindAsync(id);
        }



        public async Task<T> GetEntityWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        { 

            return await ApplySpecification(spec).ToListAsync();


        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {

            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);

        }

    }
}
