namespace Bazaar.app.Services.TesterServices
{
    public interface IBypassService
    {
        bool IsValidTester(string email, string otp);
        bool IsTester(string email);
    }
}
