using backend2.DTO;
using backend2.Interfaces;
using backend2.Logger;
using backend2.Modelos;
using backend2.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace backend2.Controllers {
    [Route("[controller]")]
    [AllowAnonymous]
    public class LoginController : ControllerBase {

        private readonly ISesionService _sesionServicio;
        private readonly IConfiguration _configuration;
        private readonly IEmpresaActivaService _empresaActivaServicio;
        private readonly IEmpresaService _empresaServicio;
        private readonly ILoggerManager _logger;

        public LoginController(ISesionService sesionServicio, IConfiguration configuration, IEmpresaActivaService empresaActivaServicio, IEmpresaService empresaServicio, ILoggerManager logger) {
            _sesionServicio = sesionServicio;
            _configuration = configuration;
            _empresaActivaServicio = empresaActivaServicio;
            _empresaServicio = empresaServicio;
            _logger = logger;
        }

        [HttpGet("login")]
        public async Task<ActionResult<UsuarioEmpresaDTO>> CheckUser(string email, string pass) {
            _logger.LogInfo($"Tratanto de iniciar sesión en el usuario con email {email}");
            Usuario user = await _sesionServicio.CheckUser(email, pass);
            if (user == null) {
                _logger.LogError("Error al iniciar sesión");
                return NotFound("Error al iniciar sesión");
            }
            _logger.LogInfo("Usuario encontrado, buscando su empresa activa");

            EmpresasActiva empresaActiva = await _empresaActivaServicio.CheckEmpresa(user.Id);
            if (empresaActiva == null) {
                _logger.LogError($"Empresa activa no encontrada para el usuario con email {email}");
                return NotFound($"Empresa activa no encontrada para el usuario con email {email}");
            }

            Empresa empresa = await _empresaServicio.CheckEmpresa(empresaActiva.IdEmpresa);
            if (empresa == null) {
                _logger.LogError($"Empresa activa no encontrada para el usuario con email {email}");
                return NotFound($"Empresa activa no encontrada para el usuario con email {email}");
            }

            bool esAdministrador = user.Email == "alejandrodelsalto@gmail.com";

            JwtSettings settings = new JwtSettings(
                keySecret: _configuration.GetSection("JwtSettings").GetValue<string>("KeySecret"),
                audience: _configuration.GetSection("JwtSettings").GetValue<string>("Audience"),
                issuer: _configuration.GetSection("JwtSettings").GetValue<string>("Issuer")
            );

            (string token, DateTime fechaExpiracion) = JwtUtil.GenerateJwtEmpresaToken(
                email, user.GuidRegistro, user.Id, empresa.GuidRegistro, empresaActiva.IdEmpresa, settings, esAdministrador
            );

            UsuarioEmpresaDTO userDTO = new UsuarioEmpresaDTO(
                user.Nombre, user.Email, empresaActiva.IdUsuario, empresaActiva.IdEmpresa, token, fechaExpiracion, user.GuidRegistro, empresa.GuidRegistro
            );

            _logger.LogInfo($"Inicio de sesión exitoso para el usuario con email {email}");
            return Ok(userDTO);
        }

        [HttpGet("RestoreToken")]
        public async Task<ActionResult<UsuarioEmpresaDTO>> RestoreToken(string nombre, string email, string guidUsuario, string guidEmpresa, int idEmpresa, int userId, bool? esAdministrador = false) {
            _logger.LogInfo($"Intento de renovación de token en la sesión del usuario con email {email}");
            JwtSettings settings = new JwtSettings(
                keySecret: _configuration.GetSection("JwtSettings").GetSection("KeySecret").Value,
                audience: _configuration.GetSection("JwtSettings").GetSection("Audience").Value,
                issuer: _configuration.GetSection("JwtSettings").GetSection("Issuer").Value);

            (string token, DateTime fechaExpiracion) = JwtUtil.GenerateJwtEmpresaToken(email, guidUsuario, userId, guidEmpresa, idEmpresa, settings, esAdministrador);

            UsuarioEmpresaDTO userDTO = new UsuarioEmpresaDTO(nombre, email, userId, idEmpresa, token, fechaExpiracion, guidUsuario, guidEmpresa);
            _logger.LogInfo($"Renovación de token exitosa para el usuario con email {email}");
            return userDTO;
        }
    }
}
