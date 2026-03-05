using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace App_Bets.Infrastructure.Services.AuthService.TokenGeracao
{
    public interface ITokenService
    {
        JwtSecurityToken GenerationToken(IEnumerable<Claim> claims, IConfiguration _config);
    }
}
