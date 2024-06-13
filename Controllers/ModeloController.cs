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
    public class ModeloController : ControllerBase {
        private readonly IModeloService _modeloServicio;
        private readonly IConfiguration _configuration;
        private readonly IEmpresaService _empresaServicio;
        private readonly JwtSettings _jwtSettings;
        private readonly ILoggerManager _logger;

        public ModeloController(IModeloService modeloServicio, IConfiguration configuration, IEmpresaService empresaServicio, IOptions<JwtSettings> options, ILoggerManager logger) {
            _modeloServicio = modeloServicio;
            _configuration = configuration;
            _empresaServicio = empresaServicio;
            _jwtSettings = options.Value;
            _logger = logger;
        }

        [HttpGet("getModelo")]
        public async Task<ActionResult<ModeloDTO>> GetModelo(int id) {
            _logger.LogInfo($"Obteniendo modelo con ID {id}");
            string conn = await _empresaServicio.GenerarCadenaDeConexionAsync((int)JwtUtil.ObtenerDatosEmpresaPeticion(Request.Headers, _configuration, _jwtSettings));
            var modelo = await _modeloServicio.GetById(id, conn);

            if (modelo == null) {
                _logger.LogError("No se encuentra el modelo buscado");
                return NotFound("No se encuentra el modelo buscado");
            }

            _logger.LogInfo($"Devolviendo modelo con ID {id}");
            return Ok(modelo);
        }

        [HttpGet("getModelos")]
        public async Task<ActionResult<List<ModeloDTO>>> GetModelos([FromQuery] ModeloParams parameters, int idEmpresa) {
            _logger.LogInfo($"Obteniendo todos los modelos de la empresa con ID {idEmpresa}");
            string conn = await _empresaServicio.GenerarCadenaDeConexionAsync((int)JwtUtil.ObtenerDatosEmpresaPeticion(Request.Headers, _configuration, _jwtSettings));
            (List<ModeloDTO> listadoModelosDto, MetadataDto metadataDto) modelos = await _modeloServicio.GetModelosPaginated(parameters, conn, idEmpresa);

            if (modelos.listadoModelosDto == null || modelos.listadoModelosDto.Count == 0) {
                _logger.LogError("No se encontraron modelos");
            }

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(modelos.metadataDto));
            _logger.LogInfo($"Devolviendo todos los modelos de la empresa con ID {idEmpresa}");
            return Ok(modelos.listadoModelosDto);
        }

        [HttpPut("updateModelo")]
        [Authorize(Policy = "ESADMINISTRADOR")]
        public async Task<IActionResult> PutModelo(int id, ModeloDTO modelo, int idEditor) {
            _logger.LogInfo($"Actualizando el modelo con ID {id} por el usuario con ID {idEditor}");
            string conn = await _empresaServicio.GenerarCadenaDeConexionAsync((int)JwtUtil.ObtenerDatosEmpresaPeticion(Request.Headers, _configuration, _jwtSettings));

            if (id != modelo.Id) {
                _logger.LogError("No se encuentra el id buscado");
                return BadRequest("No se encuentra el id buscado");
            }

            _logger.LogInfo($"Modelo con ID {id} actualizado correctamente por el usuario con ID {idEditor}");
            await _modeloServicio.UpdateModelo(modelo, idEditor, conn);
            return NoContent();
        }

        [HttpPost("postModelo")]
        [Authorize(Policy = "ESADMINISTRADOR")]
        public async Task<IActionResult> PostModelo(ModeloDTO modelo, int idCreador) {
            _logger.LogInfo($"Insertando modelo por el usuario con ID {idCreador}");
            string conn = await _empresaServicio.GenerarCadenaDeConexionAsync((int)JwtUtil.ObtenerDatosEmpresaPeticion(Request.Headers, _configuration, _jwtSettings));
            await _modeloServicio.InsertModeloAsync(modelo, idCreador, conn);
            _logger.LogInfo($"Modelo insertado por el usuario con ID {idCreador}");
            return CreatedAtAction(nameof(GetModelo), new { id = modelo.Id }, modelo);
        }

        [HttpDelete("deleteModelo")]
        [Authorize(Policy = "ESADMINISTRADOR")]
        public async Task<IActionResult> DeleteModelo(int id, int idBorrador) {
            _logger.LogInfo($"Borrando el modelo con ID {id} por el usuario con ID {idBorrador}");
            string conn = await _empresaServicio.GenerarCadenaDeConexionAsync((int)JwtUtil.ObtenerDatosEmpresaPeticion(Request.Headers, _configuration, _jwtSettings));

            var modelo = await _modeloServicio.GetById(id, conn);
            if (modelo == null) {
                _logger.LogError("No se encuentra el modelo buscado");
                return NotFound("No se encuentra el modelo buscado");
            }

            await _modeloServicio.DeleteById(id, conn, idBorrador);
            _logger.LogInfo($"Modelo con ID {id} borrado correctamente por el usuario con ID {idBorrador}");
            return NoContent();
        }
    }
}
