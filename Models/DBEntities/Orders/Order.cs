using Amazon_eCommerce_API.Models.BaseEntities;
using Amazon_eCommerce_API.Models.DBEntities.Shipments;

namespace Amazon_eCommerce_API.Models.DBEntities.Orders
{
    public class Order : BaseModel
    {
        
        public int UserId { get; set; }

        public string OrderNumber { get; set; } = null!;
        
        public decimal TotalAmount { get; set; }
        
        public DateTime OrderDate { get; set; }
        
        public OrderStatus OrderStatus { get; set; }
        
        public OrderType OrderType { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new();

        public List<Shipment> Shipments { get; set; } = new();

    }

    public enum OrderStatus
    {
        
        Pending,
        Paid,
        Processing,
        Shipped,
        Delivered,
        Cancelled,
        Refunded
        
        
    }
    
    public enum OrderType
    {
        Customer,
        Business,
        Seller
    }
}