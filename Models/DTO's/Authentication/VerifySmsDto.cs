namespace Amazon_eCommerce_API.Models.DTO_s.Authentication

{
    public class VerifySmsDto
    {
        public string PhoneNumber { get; set; }

        public string SmsOtpCode { get; set; }

         //   public bool IsResendRequest { get; set; }
    
            public AccountType AccountType { get; set; }
    }

  
}
   public enum AccountType
     {
          Customer,
          Business
     }