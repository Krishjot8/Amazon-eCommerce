using Amazon_eCommerce_API.Models.BaseEntities;

namespace Amazon_eCommerce_API.Models.DBEntities.Orders
{
    public class OrderItem : BaseModel
    {
        
        public int OrderId { get; set; }
        
        public Order Order { get; set; }
        
        public int ProductId { get; set; }
        
        public string ProductName { get; set; } = null!;
        
        public int Quantity { get; set; }
        
        public decimal UnitPrice { get; set; }
        
        public decimal TotalPrice { get; set; }
        
    }
}