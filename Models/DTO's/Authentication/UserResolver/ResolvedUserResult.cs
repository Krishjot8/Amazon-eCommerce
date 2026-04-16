namespace Amazon_eCommerce_API.Models.DTO_s.Authentication.UserResolver
{
    public class ResolvedUserResult
    
    {
        public object User { get; set; }
        
        public bool IsPasswordValid { get; set; }
    }
}