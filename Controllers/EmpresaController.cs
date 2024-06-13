using backend2.DTO;
using backend2.Interfaces;
using backend2.Logger;
using backend2.Modelos;
using backend2.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace backend2.Controllers {
    [Route("[controller]")]
    [Authorize(Policy = "ESUSUARIO")]

    public class EmpresaController : ControllerBase {
        private readonly IEmpresaService _empresaServicio;
        private readonly IEmpresaActivaService _empresaActivaServicio;
        private readonly IEmpresasUsuariosService _empresasUsuarioServicio;
        private readonly ICambioEmpresaActivaService _cambioEmpresaActivaServicio;
        private readonly IConfiguration _configuration;
        private readonly JwtSettings _jwtSettings;
        private readonly ILoggerManager _logger;

        public EmpresaController(IEmpresaService empresaServicio, IEmpresaActivaService empresaActivaServicio, IEmpresasUsuariosService empresasUsuarioServicio, IConfiguration configuration, IOptions<JwtSettings> options, ICambioEmpresaActivaService cambioEmpresaActivaServicio, ILoggerManager logger) {
            _empresaServicio = empresaServicio;
            _empresaActivaServicio = empresaActivaServicio;
            _empresasUsuarioServicio = empresasUsuarioServicio;
            _configuration = configuration;
            _jwtSettings = options.Value;
            _cambioEmpresaActivaServicio = cambioEmpresaActivaServicio;
            _logger = logger;
        }

        [HttpGet("getEmpresa")]
        public async Task<ActionResult<EmpresaDTO>> GetEmpresa(int id) {
            _logger.LogInfo($"Obteniendo empresa con ID {id}");

            string cadenaConexion = await _empresaServicio.GenerarCadenaDeConexionAsync((int)JwtUtil.ObtenerDatosEmpresaPeticion(Request.Headers, _configuration, _jwtSettings));

            var empresa = await _empresaServicio.GetById(id);
            if (empresa == null) {
                _logger.LogError("No se encontró la empresa buscada");
                return StatusCode(404, "Error, empresa no encontrada");
            }
            _logger.LogInfo($"Devolviendo empresa con ID {id}");
            return Ok(empresa);

        }

        [HttpGet("getEmpresas")]
        [Authorize(Policy = "ESADMINISTRADOR")]
        public async Task<ActionResult<List<EmpresaDTO>>> GetEmpresas( ) {
            _logger.LogInfo($"Obteniendo todas las empresas existentes");
            string cadenaConexion = await _empresaServicio.GenerarCadenaDeConexionAsync((int)JwtUtil.ObtenerDatosEmpresaPeticion(Request.Headers, _configuration, _jwtSettings));

            List<EmpresaDTO> listaEmpresas = await _empresaServicio.GetEmpresas();
            if (listaEmpresas == null || listaEmpresas.Count == 0) {
                _logger.LogError("Empresas no encontradas");
                return StatusCode(404, "Empresas no encontradas");
            }
            _logger.LogInfo("Empresas encontradas, devolviendo todas las empresas existentes");
            return Ok(listaEmpresas);
        }

        [HttpGet("getConn")]
        public async Task<ActionResult<string>> getConn() {
            string conn = await _empresaServicio.GenerarCadenaDeConexionAsync((int)JwtUtil.ObtenerDatosEmpresaPeticion(Request.Headers, _configuration, _jwtSettings));
            _logger.LogInfo($"Devolviéndo cadena de conexión modificada...");
            return Ok(conn);
        }

        [HttpGet("getEmpresaActiva")]
        public async Task<ActionResult<EmpresaActivaDTO>> GetEmpresaActiva(int idUsuario) {
            _logger.LogInfo($"Obteniendo la empresa activa para el usuario con ID {idUsuario}");
            string conn = await _empresaServicio.GenerarCadenaDeConexionAsync((int)JwtUtil.ObtenerDatosEmpresaPeticion(Request.Headers, _configuration, _jwtSettings));

            var empresaActiva = await _empresaActivaServicio.CheckEmpresa(idUsuario);

            if (empresaActiva == null) {
                _logger.LogError("No se encontró la empresa");
                return StatusCode(404, "No se encontró la empresa solicitada");
            } else {
                var empresa = await _empresaServicio.CheckEmpresa(empresaActiva.IdEmpresa);
                if (empresa == null) {
                    _logger.LogError("No se encontró la empresa");
                    return StatusCode(404, "No se encontró la empresa solicitada");
                } else {
                    EmpresaActivaDTO empActiva = new EmpresaActivaDTO();
                    empActiva.Nombre = empresa.Nombre;
                    empActiva.IdEmpresa = empresa.Id;
                    empActiva.IdUsuario = idUsuario;

                    _logger.LogInfo($"Devolviendo empresa activa para el usuario con ID {idUsuario}");
                    return Ok(empActiva);
                }
            }
        }

        [HttpGet("getEmpresasPorUsuario")]
        public async Task<ActionResult<List<EmpresaDTO>>> GetEmpresasPorUsuario(int idUsuario) {
            _logger.LogInfo($"Obteniendo todas las empresas a las que el usuario con ID {idUsuario} pertenece");

            string conn = await _empresaServicio.GenerarCadenaDeConexionAsync((int)JwtUtil.ObtenerDatosEmpresaPeticion(Request.Headers, _configuration, _jwtSettings));

            List<EmpresasUsuario> listaEmpresas = await _empresasUsuarioServicio.GetEmpresasPorUsuario(idUsuario);
            if (listaEmpresas == null || listaEmpresas.Count == 0) {
                _logger.LogError("No se encontraron empresas asignadas a este usuario");
                return StatusCode(404, "No se encontraron empresas asignadas a este usuario");
            } else {
                List<EmpresaDTO> lista = new List<EmpresaDTO>();
                foreach (EmpresasUsuario empresa in listaEmpresas) {
                    EmpresaDTO empresaNombre = await _empresaServicio.GetById(empresa.IdEmpresa);

                    EmpresaDTO emp = new EmpresaDTO();
                    emp.Nombre = empresaNombre.Nombre;
                    emp.Id = empresa.IdEmpresa;

                    lista.Add(emp);
                }
                if (lista == null || lista.Count == 0) {
                    _logger.LogError("No se encontraron empresas asignadas a este usuario");
                    return StatusCode(404, "No se encontraron empresas asignadas a este usuario");
                }
                _logger.LogInfo($"Devolviendo todas las empresas a las que el usuario con ID {idUsuario} pertenece ");
                return Ok(lista);

            }

        }

        //UPDATE
        [HttpPut("updateEmpresa")]
        [Authorize(Policy = "ESADMINISTRADOR")]
        public async Task<IActionResult> PutEmpresa(int id, Empresa empresa, int idEditor) {
            _logger.LogInfo($"Actualizando la empresa con ID {id} por el usuario con ID {idEditor}");
            string conn = await _empresaServicio.GenerarCadenaDeConexionAsync((int)JwtUtil.ObtenerDatosEmpresaPeticion(Request.Headers, _configuration, _jwtSettings));

            if (id != empresa.Id) {
                _logger.LogError("No se encuentra el id buscado");
                return StatusCode(404, "No se encuentra el id buscado");
            }

            await _empresaServicio.UpdateEmpresa(empresa, idEditor);
            _logger.LogInfo($"Empresa con ID {id} actualizada correctamente por el usuario con ID {idEditor}");
            return StatusCode(204);
        }

        // POST
        [HttpPost("postEmpresa")]
        [Authorize(Policy = "ESADMINISTRADOR")]
        public async Task<IActionResult> PostEmpresa(EmpresaDTO empresa, int idCreador) {
            _logger.LogInfo($"Insertando empresa por el usuario con ID {idCreador}");
            string conn = await _empresaServicio.GenerarCadenaDeConexionAsync((int)JwtUtil.ObtenerDatosEmpresaPeticion(Request.Headers, _configuration, _jwtSettings));


            await _empresaServicio.InsertEmpresaAsync(empresa, idCreador);
            _logger.LogInfo($"Empresa insertada por el usuario con ID {idCreador}");
            return CreatedAtAction(nameof(GetEmpresa), new { id = empresa.Id }, empresa);

        }

        //DELETE
        [HttpDelete("deleteEmpresa")]
        [Authorize(Policy = "ESADMINISTRADOR")]
        public async Task<IActionResult> DeleteEmpresa(int id, int idBorrador) {
            _logger.LogInfo($"Borrando empresa con ID {id} por el usuario con ID {idBorrador}");
            string conn = await _empresaServicio.GenerarCadenaDeConexionAsync((int)JwtUtil.ObtenerDatosEmpresaPeticion(Request.Headers, _configuration, _jwtSettings));

            var empresa = await _empresaServicio.GetById(id);
            if (empresa == null) {
                _logger.LogError("Empresa no encontrada");
                return StatusCode(404, "Empresa no encontrada");
            }

            await _empresaServicio.DeleteById(id, idBorrador);
            _logger.LogInfo($"Empresa con ID {id} borrada correctamente por el usuario con ID {idBorrador}");
            return StatusCode(204);

        }

        [HttpPut("CambiarEmpresaActiva")]
        public async Task<ActionResult<UsuarioEmpresaDTO>> CambiarEmpresaActiva(int empAnterior, int empSiguiente, string nombre, string email, string guidUsuario, int userId, bool esAdministrador) {
            _logger.LogInfo($"Cambiando empresa activa del usuario con ID {userId} de la empresa {empAnterior} a la empresa {empSiguiente}");
            string conn = await _empresaServicio.GenerarCadenaDeConexionAsync((int)JwtUtil.ObtenerDatosEmpresaPeticion(Request.Headers, _configuration, _jwtSettings));

            Empresa empresaAnterior = await _cambioEmpresaActivaServicio.GetById(empAnterior);
            Empresa empresaSiguiente = await _cambioEmpresaActivaServicio.GetById(empSiguiente);

            if (empresaAnterior is null || empresaSiguiente is null) {
                _logger.LogError("Empresa o empresas no encontrada");
                return StatusCode(404, "Empresa o empresas no encontradas");
            }

            await _cambioEmpresaActivaServicio.CambioEmpresaActiva(empresaAnterior.Id, empresaSiguiente.Id);

            JwtSettings settings = new JwtSettings(
                keySecret: _configuration.GetSection("JwtSettings").GetSection("KeySecret").Value,
                audience: _configuration.GetSection("JwtSettings").GetSection("Audience").Value,
                issuer: _configuration.GetSection("JwtSettings").GetSection("Issuer").Value);

            (string token, DateTime fechaExpiracion) = JwtUtil.GenerateJwtEmpresaToken(email, guidUsuario, userId, empresaSiguiente.GuidRegistro, empSiguiente, settings, esAdministrador);

            UsuarioEmpresaDTO userDTO = new UsuarioEmpresaDTO(nombre, email, userId, empSiguiente, token, fechaExpiracion, guidUsuario, empresaSiguiente.GuidRegistro);
            _logger.LogInfo($"Empresa activa cambiada correctamente para el usuario con ID {userId} de la empresa {empAnterior} a la empresa {empSiguiente}");
            return Ok(userDTO);


        }

    }
}