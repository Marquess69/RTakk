using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace RTakk.Data.Service
{
    public class JWTService
    {
        private string SecureKey = "My top secret key that cannot be guessed";
        public string Generate(int id)
        {
            var sk = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecureKey));

            var creds = new SigningCredentials(sk, SecurityAlgorithms.HmacSha256Signature);

            var header = new JwtHeader(creds);

            //id, aucience, claims, , expire_date

            var pl = new JwtPayload(id.ToString(), null, null, null, DateTime.Now.AddHours(4));

            var Token = new JwtSecurityToken(header, pl);

            return new JwtSecurityTokenHandler().WriteToken(Token);
        }

        public JwtSecurityToken Verify(string jwt)
        {
            var TokenHndl = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(SecureKey);
            TokenHndl.ValidateToken(jwt, new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false
            }, 
            out SecurityToken validatedToken);


            return (JwtSecurityToken)validatedToken;
        }
    }
}
