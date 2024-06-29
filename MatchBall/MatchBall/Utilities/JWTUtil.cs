using Azure;
using MatchBall.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MatchBall.Utilities
{
    public class JWTUtil: IJWTUtil
    {
        private readonly byte[] key;
        private readonly JwtSecurityTokenHandler TokenHandler;
        private string ValidatedJWTToken;
        public JWTUtil(IConfiguration Configuration)
        {
            string h = Configuration["JWT:key"];
            key = Convert.FromBase64String(Configuration["JWT:key"]);
            TokenHandler = new JwtSecurityTokenHandler();
        }

        public Status GenerateToken(string username, int Expiry)
        {
            if (username == null)
            {
                return new Status { Flag = false, message = "Invalid Data" };
            }
            try
            {
                var TokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity
                    (
                        new[] {
                        new Claim(ClaimTypes.Name, username)
                        }
                    ),
                    Expires = DateTime.UtcNow.AddMinutes(Expiry),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                };
                var token = TokenHandler.CreateToken(TokenDescriptor);

                return new Status { Flag = true, message = TokenHandler.WriteToken(token)};
            }
            catch (Exception ex) {
                return new Status { Flag = false, message = "Please Try again" };
            }
        }

        public bool ValidateToken(string token)
        {
            try
            {
                TokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true
                }, out SecurityToken ValidatedToken);
                ValidatedJWTToken = TokenHandler.WriteToken(ValidatedToken);
                return true;
            }
            catch
            {
                return false; 
            }
        }

        public Status FetchUsername()
        {
            if(ValidatedJWTToken == null)
            {
                return new Status { Flag = false, message="Validate Token" };
            }
            JwtSecurityToken readToken = TokenHandler.ReadToken(ValidatedJWTToken) as JwtSecurityToken;
            string Username = readToken.Claims.Where(data => data.Type == "unique_name").FirstOrDefault().Value;
            if (Username == null)
            {
                return new Status { Flag = false, message = "Payload is null" };
            }
            else
            {
                return new Status { Flag = true, message = Username };
            }
        }

        public bool ValidateRefreshToken(string JWTToken, string refreshToken)
        {
            try
            {
                var Refresh = TokenHandler.ReadToken(refreshToken) as JwtSecurityToken;
                var Jwt = FetchUsername().message;
                var dataInRefresh = Refresh.Claims.Where(data => data.Type == "unique_name").FirstOrDefault().Value;
                if (dataInRefresh == Jwt)
                {
                    TokenHandler.ValidateToken(refreshToken, new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateLifetime = true,
                    }, out SecurityToken ValidatedRefreshToken);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex) {
                return false;
            }
            
        }
    }
}
