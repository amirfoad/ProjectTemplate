using System.IdentityModel.Tokens.Jwt;

namespace Application.Dtos.Jwt
{
    public class AccessToken
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public int ActorId { get; set; }
        public string Roles { get; set; }

        public AccessToken(JwtSecurityToken securityToken, int actorId, string roles, string refreshToken = "")
        {
            access_token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            token_type = "Bearer";
            expires_in = (int)(securityToken.ValidTo - DateTime.UtcNow).TotalSeconds;
            refresh_token = refreshToken;
            ActorId = actorId;
            Roles = roles;
        }
    }
}