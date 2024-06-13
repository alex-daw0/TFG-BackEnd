using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace backend2.Token {
    public static class JwtUtil {

        public static JwtSecurityToken? CheckToken(string token, IConfiguration configuration, JwtSettings jwtSettings, bool validarToken = true) {
            JwtSecurityToken? jwtToken = null;
            try {
                JwtSecurityTokenHandler tokenHandler = new();
                tokenHandler.ValidateToken(token, new TokenValidationParameters {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = validarToken,
                    ValidateIssuerSigningKey = true,
                    ValidIssuers = new[] { configuration["JwtSettings:Issuer"], configuration["JwtSettingsGeneral:Issuer"] },
                    ValidAudiences = new[] { configuration["JwtSettings:Audience"], configuration["JwtSettingsGeneral:Audience"] },
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.KeySecret)),
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                jwtToken = (JwtSecurityToken)validatedToken;
            } catch (Exception ex) {
                throw ex;

            }
            return jwtToken;
        }

        public static int? ObtenerDatosEmpresaPeticion(IHeaderDictionary headers, IConfiguration configuration, JwtSettings jwtSettings, bool validarToken = true) {
            int? idEmpresa = null;
            try {
                if (headers.ContainsKey("Authorization")) {
                    JwtSecurityToken? validatedToken = CheckToken(headers["Authorization"].ToString()[7..], configuration, jwtSettings, validarToken);
                    if (validatedToken != null) {
                        idEmpresa = int.Parse(validatedToken.Claims.FirstOrDefault(x => x.Type == Constantes.ID_EMPRESA)?.Value ?? "-1");
                    }
                }
            } catch (Exception ex) {
                return null;
            }
            return idEmpresa;
        }

        public static (string, DateTime) GenerateJwtEmpresaToken(string email, string guidUsuario, int idUsuario, string guidEmpresa, int idEmpresa,
            JwtSettings jwtSettings, bool? esAdministador = false) {


            List<Claim> claims = new() {
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(Constantes.GUID_USUARIO, guidUsuario),
                new Claim(Constantes.ID_USUARIO, idUsuario.ToString()),
                new Claim(Constantes.GUID_EMPRESA, guidEmpresa),
                new Claim(Constantes.ID_EMPRESA, idEmpresa.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            if (esAdministador == true) {
                claims.Add(new Claim(Constantes.ESADMINISTRADOR, "ESADMINISTRADOR"));
            } 
                claims.Add(new Claim(Constantes.ESUSUARIO, "ESUSUARIO"));
            


            SymmetricSecurityKey keySecret = new(Encoding.UTF8.GetBytes(jwtSettings.KeySecret));
            SigningCredentials credentials = new(keySecret, SecurityAlgorithms.HmacSha256);

            /*Tiempo durante el cual nuestro token va a ser válido para nuestras peticiones, al caducar, nuestras peticiones serán rechazadas con un 401 - Unauthorized, 
              por lo que desde nuestro FrontEnd con el interceptor de Axios, necesitaremos captar todas las peticiones rechazadas con un 401 para intentar renovar el token
              siempre y cuando se cumpla con las condiciones necesarias (que el token no haya caducado hace más de 5 minutos)
            Eso*/
            DateTime expiration = DateTime.Now.AddSeconds(5);

            JwtSecurityToken token = new(
               issuer: jwtSettings.Issuer,
               audience: jwtSettings.Audience,
               claims: claims,
               expires: expiration,
               signingCredentials: credentials);

            return (new JwtSecurityTokenHandler().WriteToken(token), expiration);
        }

    }
}

