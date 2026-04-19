namespace Amazon_eCommerce_API.Models.BaseEntities
{
    public class BaseModel
    {
        public int Id { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; } 


    }
}
