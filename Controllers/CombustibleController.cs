using backend2.DTO;
using backend2.Interfaces;
using backend2.Logger;
using backend2.Token;
using backend2.Utilidad;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace backend2.Controllers {
    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy = "ESUSUARIO")]
    public class CombustibleController : ControllerBase {

        private readonly IEmpresaService _empresaServicio;
        private readonly ICombustibleService _combustibleServicio;

        private readonly IConfiguration _configuration;
        private readonly JwtSettings _jwtSettings;
        private readonly ILoggerManager _logger;


        public CombustibleController(ICombustibleService combustibleServicio, IConfiguration configuration, IOptions<JwtSettings> options, IEmpresaService empresaServicio, ILoggerManager logger
) {
            _combustibleServicio = combustibleServicio;
            _configuration = configuration;
            _jwtSettings = options.Value;
            _empresaServicio = empresaServicio;
            _logger = logger;
        }

        [HttpGet("getCombustibles")]
        public async Task<ActionResult<List<CombustibleDTO>>> GetCombustible([FromQuery] CombustibleParams parameters, int idEmpresa) {
            _logger.LogInfo($"Obteniendo todos los combustibles de la empresa con ID {idEmpresa}");

            string conn = await _empresaServicio.GenerarCadenaDeConexionAsync((int)JwtUtil.ObtenerDatosEmpresaPeticion(Request.Headers, _configuration, _jwtSettings));

            (List<CombustibleDTO> listadoCombustiblesDto, MetadataDto metadataDto) combustibles = await _combustibleServicio.GetCombustiblesPaginated(parameters, conn, idEmpresa);

            if (combustibles.listadoCombustiblesDto == null || combustibles.listadoCombustiblesDto.Count == 0) {
                _logger.LogError("No se encontraron combustibles");
            }
            _logger.LogInfo($"Devolviendo todos los combustible de la empresa con ID {idEmpresa}");
            return StatusCode(200, combustibles.listadoCombustiblesDto);
        }


        [HttpGet("getCombustible")]
        public async Task<ActionResult<CombustibleDTO>> GetCombustible(int id) {
            _logger.LogInfo($"Obteniendo el combustible con ID {id}");
            string conn = await _empresaServicio.GenerarCadenaDeConexionAsync((int)JwtUtil.ObtenerDatosEmpresaPeticion(Request.Headers, _configuration, _jwtSettings));
            CombustibleDTO combustible = await _combustibleServicio.GetById(id, conn);

            if (combustible == null) {
                _logger.LogError("No se encontró el combustible buscado");
                return StatusCode(404, "No se encuentra el combustible buscado");
            }
            _logger.LogInfo($"Devolviendo el combustible con ID {id}");
            return StatusCode(200, combustible);
        }
        //UPDATE
        [HttpPut("updateCombustible")]
        [Authorize(Policy = "ESADMINISTRADOR")]
        public async Task<IActionResult> PutCombustible(int id, CombustibleDTO combustible, int idEditor) {
            _logger.LogInfo($"Actualizando el combustible con ID {id} por el usuario con ID {idEditor}");
            string conn = await _empresaServicio.GenerarCadenaDeConexionAsync((int)JwtUtil.ObtenerDatosEmpresaPeticion(Request.Headers, _configuration, _jwtSettings));

            if (id != combustible.Id) {
                _logger.LogError("No se encuentra el combustible seleccionado");
                return StatusCode(400, "No se encuentra el id buscado");
            }
            await _combustibleServicio.UpdateCombustible(combustible, idEditor, conn);
            _logger.LogInfo($"Combustible con ID {id} actualizado correctamente por el usuario con ID {idEditor}");
            return StatusCode(204);

        }

        // POST
        [HttpPost("postCombustible")]
        [Authorize(Policy = "ESADMINISTRADOR")]
        public async Task<IActionResult> PostCombustible(CombustibleDTO combustible, int idCreador) {
            _logger.LogInfo($"Insertando combustible por el usuario con ID {idCreador}");
            string conn = await _empresaServicio.GenerarCadenaDeConexionAsync((int)JwtUtil.ObtenerDatosEmpresaPeticion(Request.Headers, _configuration, _jwtSettings));

            await _combustibleServicio.InsertCombustibleAsync(combustible, idCreador, conn);
            _logger.LogInfo($"Combustible insertado por el usuario con ID {idCreador}");
            return CreatedAtAction(nameof(GetCombustible), new { id = combustible.Id }, combustible);

        }

        //DELETE
        [HttpDelete("deleteCombustible")]
        [Authorize(Policy = "ESADMINISTRADOR")]
        public async Task<IActionResult> DeleteCombustible(int id, int idBorrador) {
            _logger.LogInfo($"Borrando combustible con ID {id} por el usuario con ID {idBorrador}");
            string conn = await _empresaServicio.GenerarCadenaDeConexionAsync((int)JwtUtil.ObtenerDatosEmpresaPeticion(Request.Headers, _configuration, _jwtSettings));


            var combustible = await _combustibleServicio.GetById(id, conn);
            if (combustible == null) {
                _logger.LogError("No se encuentra el combustible indicado");
                return StatusCode(404, "No se encuentra el combustible buscado");
            }
            await _combustibleServicio.DeleteById(id, conn, idBorrador);
            _logger.LogInfo($"Combustible con ID {id} borrado correctamente por el usuario con ID {idBorrador}");
            return StatusCode(204);

        }
    }
}