using System.Linq.Expressions;

namespace Amazon_eCommerce_API.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public BaseSpecification(Expression<Func<T, bool>> criteria, List<Expression<Func<T, object>>> includes)
        {
            Criteria = criteria;
            Includes = includes;
        }

        public Expression<Func<T, bool>> Criteria { get; }

        public List<Expression<Func<T, object>>> Includes { get; }
        
    
    }

}
