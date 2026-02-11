using Bazaar.app.Dtos;
using Bazaar.Entityframework.Models;

namespace Bazaar.app.Services
{
    public interface IJwtTokenService
    {
        JwtTokenResult GenerateToken(AppUser user, IList<string> roles);
    }
}
