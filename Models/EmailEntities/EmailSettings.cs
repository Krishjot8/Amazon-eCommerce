namespace Amazon_eCommerce_API.Models.EmailEntities
{
    public class EmailSettings
    {

        public string DefaultProvider { get; set; }


        public Dictionary<string, EmailProviderSettings> Providers { get; set; }
    }
}
