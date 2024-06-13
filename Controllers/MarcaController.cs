using backend2.DTO;
using backend2.Interfaces;
using backend2.Logger;
using backend2.Token;
using backend2.Utilidad;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Threading.Tasks;

namespace backend2.Controllers {
    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy = "ESUSUARIO")]
    public class MarcaController : ControllerBase {
        private readonly IMarcaService _marcaServicio;
        private readonly IEmpresaService _empresaServicio;
        private readonly IConfiguration _configuration;
        private readonly JwtSettings _jwtSettings;
        private readonly ILoggerManager _logger;

        public MarcaController(IMarcaService marcaServicio, IEmpresaService empresaServicio, IConfiguration configuration,  IOptions<JwtSettings> options, ILoggerManager logger) {
            _marcaServicio = marcaServicio;
            _empresaServicio = empresaServicio;
            _configuration = configuration;
            _jwtSettings = options.Value;
            _logger = logger;
        }

        [HttpGet("getMarca")]
        public async Task<ActionResult<MarcaDTO>> GetMarca(int id) {
            _logger.LogInfo($"Obteniendo marca con Id {id}");
            string conn = await _empresaServicio.GenerarCadenaDeConexionAsync((int)JwtUtil.ObtenerDatosEmpresaPeticion(Request.Headers, _configuration, _jwtSettings));

            var marca = await _marcaServicio.GetById(id, conn);

            if (marca == null) {
                _logger.LogError("Marca no encontrada");
                return NotFound("Marca no encontrada");
            }

            _logger.LogInfo($"Devolviendo marca con ID {id}");
            return Ok(marca);
        }

        [HttpGet("getMarcas")]
        public async Task<ActionResult<List<MarcaDTO>>> GetMarcas([FromQuery] MarcaParams parameters, int idEmpresa) {
            _logger.LogInfo($"Obteniendo todas las marcas de la empresa con ID {idEmpresa}");
            string conn = await _empresaServicio.GenerarCadenaDeConexionAsync((int)JwtUtil.ObtenerDatosEmpresaPeticion(Request.Headers, _configuration, _jwtSettings));
            (List<MarcaDTO> listadoMarcasDto, MetadataDto metadataDto) marcas = await _marcaServicio.GetMarcasPaginatedAsync(parameters, conn, idEmpresa);

            if (marcas.listadoMarcasDto == null || marcas.listadoMarcasDto.Count == 0) {
                _logger.LogError("No se encontraron marcas");
            }

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(marcas.metadataDto));
            _logger.LogInfo($"Devolviendo todas las marcas de la empresa con ID {idEmpresa}");
            return Ok(marcas.listadoMarcasDto);
        }

        [HttpPut("updateMarca")]
        [Authorize(Policy = "ESADMINISTRADOR")]
        public async Task<IActionResult> PutMarca(int id, MarcaDTO marca, int idEditor) {
            _logger.LogInfo($"Actualizando la marca con ID {id} por el usuario con ID {idEditor}");
            string conn = await _empresaServicio.GenerarCadenaDeConexionAsync((int)JwtUtil.ObtenerDatosEmpresaPeticion(Request.Headers, _configuration, _jwtSettings));

            if (id != marca.Id) {
                _logger.LogError("No se encuentra el id buscado");
                return BadRequest("No se encuentra el id buscado");
            }

            await _marcaServicio.UpdateMarcaAsync(marca, idEditor, conn);
            _logger.LogInfo($"Marca con ID {id} actualizada correctamente por el usuario con ID {idEditor}");
            return NoContent();
        }

        [HttpPost("postMarca")]
        [Authorize(Policy = "ESADMINISTRADOR")]
        public async Task<IActionResult> PostMarca(MarcaDTO marca, int idCreador) {
            _logger.LogInfo($"Insertando marca por usuario con ID {idCreador}");
            string conn = await _empresaServicio.GenerarCadenaDeConexionAsync((int)JwtUtil.ObtenerDatosEmpresaPeticion(Request.Headers, _configuration, _jwtSettings));

            await _marcaServicio.InsertMarcaAsync(marca, idCreador, conn);
            _logger.LogInfo($"Marca insertada por el usuario con ID {idCreador}");
            return CreatedAtAction(nameof(GetMarca), new { id = marca.Id }, marca);
        }

        [HttpDelete("deleteMarca")]
        [Authorize(Policy = "ESADMINISTRADOR")]
        public async Task<IActionResult> DeleteMarca(int id, int idBorrador) {
            _logger.LogInfo($"Borrando marca con ID {id} por el usuario con ID {idBorrador}");
            string conn = await _empresaServicio.GenerarCadenaDeConexionAsync((int)JwtUtil.ObtenerDatosEmpresaPeticion(Request.Headers, _configuration, _jwtSettings));

            var marca = await _marcaServicio.GetById(id, conn);
            if (marca == null) {
                _logger.LogError("No se ha encontrado la marca buscada");
                return NotFound("No se ha encontrado la marca buscada");
            }

            await _marcaServicio.DeleteById(id, conn, idBorrador);
            _logger.LogInfo($"Marca con ID {id} borrada correctamente por el usuario con ID {idBorrador}");
            return NoContent();
        }
    }
}
