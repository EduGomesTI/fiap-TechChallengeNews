using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace News.Security.JWT
{
    public class TokenConfigurations
    {
        private SecurityKey? _key;
        private SigningCredentials? _signingCredentials;

        public string? Audience { get; set; }
        public string? Issuer { get; set; }
        public int Seconds { get; set; }
        public string? SecretJwtKey { get; set; }
        public SigningCredentials SigningCredentials
        {
            get
            {
                if (_signingCredentials == null)
                {
                    _signingCredentials = new(Key, SecurityAlgorithms.HmacSha256Signature);
                }
                return _signingCredentials;
            }
            set => _signingCredentials = value;
        }
        public SecurityKey Key {
            get
            {
                if (_key == null)
                {
                    _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretJwtKey));
                }
                return _key;
            }
            set => _key = value; }

    }
}