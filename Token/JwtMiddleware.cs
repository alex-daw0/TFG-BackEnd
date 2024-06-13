using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace backend2.Token {
    public class JwtMiddleware {
        private readonly RequestDelegate _next;
        private readonly JwtSettings _jwtSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<JwtSettings> jwtSettings) {
            _next = next;
            _jwtSettings = jwtSettings.Value;
        }

        /// <summary>
        /// Función encargada de comprobar la vericidad del token actual
        /// </summary>
        /// <remarks> Esta función se llama con cada peticion que se realiza</remarks>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context) {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token is not null) {
                AttachUserToContext(context, token);
            }

            await _next(context);
        }

        private void AttachUserToContext(HttpContext context, string token) {
            try {
                JwtSecurityTokenHandler tokenHandler = new();
                tokenHandler.ValidateToken(token, new TokenValidationParameters {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidAudience = _jwtSettings.Issuer,
                    ValidAudiences = new[] { _jwtSettings.Issuer, _jwtSettings.Audience },
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.KeySecret)),
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                JwtSecurityToken jwtToken = validatedToken as JwtSecurityToken;
                string user = jwtToken.Claims.First(x => x.Type == "NombreUsuario").Value;
                context.Items["User"] = user;
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);

            }
        }
    }
}
