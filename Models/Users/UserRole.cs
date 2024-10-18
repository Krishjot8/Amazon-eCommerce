namespace Amazon_eCommerce_API.Models.Users
{
    public class UserRole : BaseModel
    {

        public string RoleName { get; set; }


        public ICollection<User> Users { get; set; }


    }
}
