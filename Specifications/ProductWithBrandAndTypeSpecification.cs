using Amazon_eCommerce_API.Models;
using System.Linq.Expressions;

namespace Amazon_eCommerce_API.Specifications
{
    public class ProductWithBrandAndTypeSpecification : BaseSpecification<Product>
    {
        public ProductWithBrandAndTypeSpecification() : base(null)
        {

            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
        }
    }
}
