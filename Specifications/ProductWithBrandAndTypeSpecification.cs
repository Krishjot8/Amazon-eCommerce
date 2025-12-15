using Amazon_eCommerce_API.Models;
using System.Linq.Expressions;
using Amazon_eCommerce_API.Models.DBEntities.Products;

namespace Amazon_eCommerce_API.Specifications
{
    public class ProductWithBrandAndTypeSpecification : BaseSpecification<Products>
    {
        public ProductWithBrandAndTypeSpecification() : base(null)
        {

            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
        }
    }
}
