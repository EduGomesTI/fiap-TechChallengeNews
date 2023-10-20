using Microsoft.IdentityModel.Tokens;
using News.Domain.Entities;
using News.Infrastructure.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace News.Security.JWT
{
    public class AccessManager
    {
        private readonly TokenConfigurations _tokenConfigurations;
        private readonly AppDbContext _context;

        public AccessManager(
            TokenConfigurations tokenConfigurations,
            AppDbContext context)
        {
            _tokenConfigurations = tokenConfigurations;
            _context = context;
        }

        public bool ValidateCredentials(User inputUser)
        {
            if (string.IsNullOrEmpty(inputUser.Username) || string.IsNullOrEmpty(inputUser.Password))
                return false;

            var user = _context.User.SingleOrDefault(x => x.Username == inputUser.Username);

            // check if username exists
            if (user == null)
                return false;

            // check if password is correct
            if (user.Password == inputUser.Password)
                return true;

            // authentication successful
            return false;
        }

        public Token GenerateToken(User user)
        {
            ClaimsIdentity identity = new(
                new GenericIdentity(user.Username!, "Login"),
                new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.Username!)
                }
            );

            DateTime dataCriacao = DateTime.Now;
            DateTime dataExpiracao = dataCriacao +
                TimeSpan.FromSeconds(_tokenConfigurations.Seconds);

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenConfigurations.Issuer,
                Audience = _tokenConfigurations.Audience,
                SigningCredentials = _tokenConfigurations.SigningCredentials,
                Subject = identity,
                NotBefore = dataCriacao,
                Expires = dataExpiracao
            });
            var token = handler.WriteToken(securityToken);

            return new()
            {
                Authenticated = true,
                Created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                Expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                AccessToken = token,
                Message = "OK"
            };
        }
    }
}