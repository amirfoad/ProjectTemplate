using Application.Contracts.Authentication;
using Application.Contracts.Persistence;
using Application.Dtos.Jwt;
using Domain.Entities.Authentication;
using Identity.Identity;
using Identity.Identity.Dtos;
using Identity.Identity.Manager;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.Jwt
{
    public class JwtService : IJwtService
    {
        private readonly IdentitySettings _siteSetting;
        private readonly AppUserManager _userManager;
        private readonly IAppUserPrincipalFactory _claimsPrincipal;
        private readonly IUnitOfWork _unitOfWork;

        public JwtService(IOptions<IdentitySettings> siteSetting,
            AppUserManager userManager,
            IAppUserPrincipalFactory claimsPrincipal,
            IUnitOfWork unitOfWork)
        {
            _siteSetting = siteSetting.Value;
            _userManager = userManager;
            _claimsPrincipal = claimsPrincipal;
            _unitOfWork = unitOfWork;
        }

        public async Task<AccessToken> GenerateAsync(User user, int actorId = 0)
        {
            var admin = await _unitOfWork.UserRepository.TableNoTracking.SingleOrDefaultAsync(x =>
                x.Id == actorId);
            actorId = user.Id;
            if (admin != null)
                actorId = admin.Id;

            var secretKey = Encoding.UTF8.GetBytes(_siteSetting.SecretKey); // longer that 16 character
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);

            var encryptionkey = Encoding.UTF8.GetBytes(_siteSetting.Encryptkey); //must be 16 character
            var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionkey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);

            var claims = (await _getClaimsAsync(user, actorId)).ToList();
            var userRoles = claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToList();
            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _siteSetting.Issuer,
                Audience = _siteSetting.Audience,
                IssuedAt = DateTime.Now,
                NotBefore = DateTime.Now.AddMinutes(0),
                Expires = DateTime.Now.AddMinutes(_siteSetting.ExpirationMinutes),
                SigningCredentials = signingCredentials,
                EncryptingCredentials = encryptingCredentials,
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.CreateJwtSecurityToken(descriptor);

            var roles = string.Join(",", userRoles);
            var refreshToken = await _unitOfWork.UserRefreshTokenRepository.CreateToken(user.Id);
            await _unitOfWork.UserRefreshTokenRepository.RemoveUserOldTokens(user.Id);

            await _unitOfWork.CommitAsync();
            return new AccessToken(securityToken, actorId, roles, refreshToken.ToString());
        }

        public Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_siteSetting.SecretKey)),
                ValidateLifetime = false,
                TokenDecryptionKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_siteSetting.Encryptkey))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            //var jwtSecurityToken = securityToken as JwtSecurityToken;
            //if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            //    throw new SecurityTokenException("Invalid token");

            return Task.FromResult(principal);
        }

        public async Task<AccessToken> GenerateByPhoneNumberAsync(string phoneNumber)
        {
            var user = await _userManager.Users.AsNoTracking().FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
            var result = await this.GenerateAsync(user);
            return result;
        }

        public async Task<AccessToken> RefreshToken(string refreshTokenId, int requestingAdminId = 0)
        {
            var refreshToken = await _unitOfWork.UserRefreshTokenRepository.GetTokenWithInvalidation(Guid.Parse(refreshTokenId));

            if (refreshToken is null)
                return null;

            await _unitOfWork.CommitAsync();

            var user = await _unitOfWork.UserRefreshTokenRepository.GetUserByRefreshToken(Guid.Parse(refreshTokenId));
            if (user is null)
                return null;

            var result = await this.GenerateAsync(user, requestingAdminId);

            return result;
        }

        private async Task<IEnumerable<Claim>> _getClaimsAsync(User user, int actorId)
        {
            var result = await _claimsPrincipal.CreateAsync(user, actorId);
            return result.Claims;
        }
    }
}