using backend2.DTO;
using backend2.Interfaces;
using backend2.Logger;
using backend2.Token;
using backend2.Utilidad;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace backend2.Controllers {
    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy = "ESUSUARIO")]
    public class VehiculoController : ControllerBase {

        private readonly IVehiculoService _vehiculoServicio;
        private readonly IEmpresaService _empresaServicio;
        private readonly ICombustibleService _combustibleServicio;
        private readonly IMarcaService _marcaServicio;
        private readonly IModeloService _modeloServicio;
        private readonly IConfiguration _configuration;
        private readonly JwtSettings _jwtSettings;
        private readonly ILoggerManager _logger;

        public VehiculoController(IVehiculoService vehiculoServicio, ICombustibleService combustibleServicio, IConfiguration configuration,IOptions<JwtSettings> options,
        IEmpresaService empresaServicio, IMarcaService marcaServicio, IModeloService modeloServicio, ILoggerManager logger) {
            _vehiculoServicio = vehiculoServicio;
            _combustibleServicio = combustibleServicio;
            _configuration = configuration;
            _jwtSettings = options.Value;
            _empresaServicio = empresaServicio;
            _marcaServicio = marcaServicio;
            _modeloServicio = modeloServicio;
            _logger = logger;
        }

        //GET
        [HttpGet("getVehiculo")]
        public async Task<ActionResult<VehiculoDTO>> GetVehicle(int id) {
            _logger.LogInfo("Obteniendo vehículo con ID: " + id);

            string conn = await _empresaServicio.GenerarCadenaDeConexionAsync((int)JwtUtil.ObtenerDatosEmpresaPeticion(Request.Headers, _configuration, _jwtSettings));
            VehiculoDTO vehiculo = await _vehiculoServicio.GetById(id, conn);

            if (vehiculo == null) {
                _logger.LogError("vehículo con ID: " + id + " no encontrado.");
                return NotFound("No se encuentra el vehículo buscado");
            } else {
                _logger.LogInfo($"Devolviendo vehículo con ID: {id}");
                return Ok(vehiculo);
            }
        }

        [HttpGet("getVehiculosPorEmpresa")]
        public async Task<ActionResult<List<VehiculoDTO>>> GetVehiculosPorEmpresa([FromQuery] int idEmpresa) {
            _logger.LogInfo("Obteniendo vehículos de la empresa con ID: " + idEmpresa);

            string conn = await _empresaServicio.GenerarCadenaDeConexionAsync((int)JwtUtil.ObtenerDatosEmpresaPeticion(Request.Headers, _configuration, _jwtSettings));
            List<VehiculoDTO> vehiculos = await _vehiculoServicio.GetVehiculos(idEmpresa, conn);

            if(vehiculos == null || vehiculos.Count == 0) {
                _logger.LogError("No se han encontrado vehículos");
            }

            _logger.LogInfo($"Devolviendo {vehiculos.Count} vehículos de la empresa con ID: {idEmpresa}");
            return Ok(vehiculos);
        }

        [HttpGet("getAllVehiculos")]
        public async Task<ActionResult<List<VehiculoDTO>>> GetAllVehiculos([FromQuery] VehiculoParams parameters, int idEmpresa) {
            _logger.LogInfo("Obteniendo todos los vehículos de la empresa con ID: " + idEmpresa);

            string conn = await _empresaServicio.GenerarCadenaDeConexionAsync((int)JwtUtil.ObtenerDatosEmpresaPeticion(Request.Headers, _configuration, _jwtSettings));
            var vehiculos = await _vehiculoServicio.GetVehiculosPaginated(parameters, conn, idEmpresa);

            if(vehiculos.listadoVehiculosDto == null ||vehiculos.listadoVehiculosDto.Count == 0) {
                _logger.LogError("No se han encontrado vehículos");
            }

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(vehiculos.metadataDto));
            _logger.LogInfo($"Devolviendo {vehiculos.listadoVehiculosDto.Count} vehículos de la empresa con ID " + idEmpresa);
            return Ok(vehiculos.listadoVehiculosDto);
        }

        //UPDATE
        [HttpPut("updateVehiculo")]
        public async Task<IActionResult> PutVehicle(int id, VehiculoDTO vehiculo, int idEditor) {
            _logger.LogInfo($"Actualizando vehículo con ID: {id} por el usuario con id {idEditor}");

            string conn = await _empresaServicio.GenerarCadenaDeConexionAsync((int)JwtUtil.ObtenerDatosEmpresaPeticion(Request.Headers, _configuration, _jwtSettings));

            if (id != vehiculo.Id) {
                _logger.LogError($"El ID no coincide: el ID solicitado - {id} no coincide con el ID de vehículo - {vehiculo.Id}");
                return BadRequest("No se encuentra el id buscado");
            }

            await _vehiculoServicio.UpdateVehiculo(vehiculo, conn, idEditor);
            _logger.LogInfo($"Vehículo con ID: {id} actualizado correctamente por el usuario con ID {idEditor}.");
            return NoContent();
        }

        // POST
        [HttpPost("postVehiculo")]
        public async Task<IActionResult> PostVehicle(VehiculoDTO vehiculo, int idCreador) {
            _logger.LogInfo($"Creando vehículo por parte del usuario con ID: {idCreador}");

            string conn = await _empresaServicio.GenerarCadenaDeConexionAsync((int)JwtUtil.ObtenerDatosEmpresaPeticion(Request.Headers, _configuration, _jwtSettings));

            await _vehiculoServicio.InsertVehiculoAsync(vehiculo, conn, idCreador);
            _logger.LogInfo($"Vehículo añadido con ID: {vehiculo.Id}");
            return CreatedAtAction(nameof(GetVehicle), new { id = vehiculo.Id }, vehiculo);
        }

        //DELETE
        [HttpDelete("deleteVehiculo")]
        public async Task<IActionResult> DeleteVehicle(int id, int idBorrador) {
            _logger.LogInfo($"Borrando vehículo con ID: {id} por usuario con ID: {idBorrador}");

            string conn = await _empresaServicio.GenerarCadenaDeConexionAsync((int)JwtUtil.ObtenerDatosEmpresaPeticion(Request.Headers, _configuration, _jwtSettings));
            var vehiculo = await _vehiculoServicio.GetById(id, conn);
            if (vehiculo == null) {
                _logger.LogError($"Vehículo con ID: {id} no encontrado.");
                return NotFound("No se encuentra el vehículo buscado");
            }

            await _vehiculoServicio.DeleteById(id, conn, idBorrador);
            _logger.LogInfo($"Vehículo con ID: {id} borrado correctamente por el usuario con ID {idBorrador}.");
            return NoContent();
        }
    }
}
