using DatingApp.Entites;
using DAtingApp.interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DAtingApp.Services
{
    public class TokenSerivce : ITokenService
    {
        private SymmetricSecurityKey _SecurityKey;

        public TokenSerivce(IConfiguration configuration)
        {
            _SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"]));

        }
        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
				  //new Claim(JwtRegisteredClaimNames.NameId, user.UserId.ToString()),
				  new Claim(JwtRegisteredClaimNames.NameId, user.UserName),
			};

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
