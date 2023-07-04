using DatingApp.Entites;
using DAtingApp.interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace DAtingApp.Services
{
    public class TokenSerivce : ITokenService
    {
		private readonly UserManager<AppUser> _UserManager;
		private SymmetricSecurityKey _SecurityKey;

        public TokenSerivce(IConfiguration configuration,UserManager<AppUser> userManager)
        {
            _SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"]));
			_UserManager = userManager;
		}
        public async Task<string> CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
				  //new Claim(JwtRegisteredClaimNames.NameId, user.UserId.ToString()),
				  new Claim(JwtRegisteredClaimNames.NameId, user.UserName),
                  new Claim(JwtRegisteredClaimNames.UniqueName, user.Id.ToString())
			};

            var roles = await _UserManager.GetRolesAsync(user);

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role,role)));


            var creds = new SigningCredentials(_SecurityKey, SecurityAlgorithms.HmacSha512Signature);

            var tokenDesc = new SecurityTokenDescriptor
            {

                SigningCredentials = creds,
                Expires = DateTime.Now.AddDays(7),
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDesc);

            return tokenHandler.WriteToken(token);
        }
    }
}
