using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TranslationFramework.Modelos;

namespace TranslationFramework.Servicos.Autorizacao
{
    public static class TokenServico
    {
        public static string GerarToken(User usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            
            var chave = Encoding.ASCII.GetBytes("fedaf7d8863b48e197b9287d492b708e");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, usuario.Username),
                    new Claim(ClaimTypes.Role, usuario.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(chave),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return tokenHandler.WriteToken(token);
        }
    }
}
