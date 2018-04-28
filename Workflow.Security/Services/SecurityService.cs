using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Workflow.Abstractions.Models;
using Workflow.Abstractions.Services;
using Workflow.Core.Security;
using Workflow.Security.Configuration;

namespace Workflow.Security.Services
{
    public class SecurityService : ISecurityService
    {
        private const string CLAIM_PROJECT_ID = "Id";

        private const string CLAIM_USERNAME = "Username";

        private readonly ISecurityConfiguration _securityConfiguration;


        public SecurityService(ISecurityConfiguration securityConfiguration)
        {
            _securityConfiguration = securityConfiguration;
        }

        public string GenerateProjectToken(IProject project)
        {
            var claims = new[]
            {
                new Claim(CLAIM_PROJECT_ID, Convert.ToString(project.Id)),
                new Claim(ClaimTypes.Role, Roles.Project)
            };

            return GenerateToken(claims);
        }


        public string GenerateProjectAccessKey()
        {
            using (var cryptoProvider = new RNGCryptoServiceProvider())
            {
                var bytes = new byte[32];

                cryptoProvider.GetBytes(bytes);

                return Convert.ToBase64String(bytes);
            }
        }

        public string GenerateUserToken(IUser user)
        {
            var claims = new[]
            {
                new Claim(CLAIM_USERNAME, Convert.ToString(user.Username)),
                new Claim(ClaimTypes.Role, Convert.ToString(user.Role))
            };

            return GenerateToken(claims);
        }

        private string GenerateToken(Claim[] claims)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securityConfiguration.SecretKey));

            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_securityConfiguration.Issuer, _securityConfiguration.Audience, signingCredentials: credentials, claims: claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
