namespace Amazon_eCommerce_API.Extensions
{
    public static class OneTimePasswordGenerator
    {

        public static string GenerateOtp()
        
        
        { 
        
        Random random = new Random();

            return random.Next(100000,999999).ToString();
        
        
        
        }
        

    }
}
