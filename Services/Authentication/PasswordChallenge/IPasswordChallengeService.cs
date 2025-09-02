namespace Amazon_eCommerce_API.Services.Authentication.PasswordChallenge
{
    public interface IPasswordChallengeService
    {

        bool VerifyPassword(string inputPassword, string storedHash);
        string HashPassword(string password);

        bool RequiresReChallenge(string userId);

    }
}
