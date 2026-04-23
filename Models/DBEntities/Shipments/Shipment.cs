using Amazon_eCommerce_API.Models.BaseEntities;
using Amazon_eCommerce_API.Models.DBEntities.Orders;

namespace Amazon_eCommerce_API.Models.DBEntities.Shipments
{
    public class Shipment : BaseModel
    {
        public int OrderId { get; set; }

        public Order Order { get; set; } = null!;
        public string Carrier { get; set; } = null!;
        public string TrackingNumber { get; set; } = null!;
        public DateTime ShippedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }

        public ShipmentStatus Status { get; set; }
       
        public string ShippingAddressSnapshot { get; set; } = null!;

    }

    public enum ShipmentStatus
    {
        Pending,
        LabelCreated,
        Shipped,
        InTransit,
        Delivered,
        Delayed,
        Cancelled
    }
}
