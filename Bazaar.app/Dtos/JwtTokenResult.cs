namespace Bazaar.app.Dtos
{
    public class JwtTokenResult
    {
        public string Token { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public string? Id { get; set; }
    }
}
