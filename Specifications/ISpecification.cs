using System.Linq.Expressions;

namespace Amazon_eCommerce_API.Specifications
{
    public interface ISpecification<T>
    {

        Expression<Func<T, bool>> Criteria { get; }

        List<Expression<Func<T, object>>> Includes{ get;}
    }
}
