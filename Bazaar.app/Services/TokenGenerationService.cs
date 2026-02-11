using Bazaar.app.Dtos;
using Bazaar.Entityframework.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Bazaar.app.Services
{
    public class TokenGenerationService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;

        public TokenGenerationService(IConfiguration config)
        {
            _configuration = config;
        }
        public JwtTokenResult GenerateToken(AppUser user, IList<string> roles)
        {
            List<Claim> claims = new();
            claims.Add(new Claim(ClaimTypes.Name, user.UserName!));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id!));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]!));
            var sc = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: claims,
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.Now.AddMonths(1),
                signingCredentials: sc
            );
            return new JwtTokenResult()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresAt = token.ValidTo,
                Id = user.Id
            };
        }
    }
}
