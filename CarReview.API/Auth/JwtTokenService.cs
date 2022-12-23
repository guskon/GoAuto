using CarReview.API.Auth.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace CarReview.API.Auth
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly SymmetricSecurityKey _authSigningKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly UserManager<CarReviewUser> _userManager;

        public JwtTokenService(IConfiguration configuration, UserManager<CarReviewUser> userManager)
        {
            _authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
            _issuer = configuration["JWT:ValidIssuer"];
            _audience = configuration["JWT:ValidAudience"];
            _userManager = userManager;
        }

        public async Task<string> CreateAccessToken(string userName, string userId, IEnumerable<string> userRoles)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);
            var rolesToString = "";
            foreach (var role in roles)
                rolesToString += role;

            var authClaims = new List<Claim>
            {
                new(type: ClaimTypes.Name, value: userName),
                new(type: JwtRegisteredClaimNames.Jti, value: Guid.NewGuid().ToString()),
                new(type: JwtRegisteredClaimNames.Sub, value: userId),
                new Claim("userId", userId),
                new Claim("userName", userName),
                new Claim("roles", rolesToString)
            };

            authClaims.AddRange(collection: userRoles.Select(userRole => new Claim(type: ClaimTypes.Role, value: userRole)));

            var accessSecurityToken = new JwtSecurityToken
            (
                issuer: _issuer,
                audience: _audience,
                expires: DateTime.UtcNow.AddHours(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(_authSigningKey, algorithm: SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(accessSecurityToken);
        }
    }
}
