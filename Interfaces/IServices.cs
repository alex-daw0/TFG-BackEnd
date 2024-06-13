using backend2.DTO;
using backend2.Modelos;
using backend2.Utilidad;

namespace backend2.Interfaces {
    public interface IVehiculoService {
        Task<VehiculoDTO> GetById(int Id, string cadenaConexion);
        Task<Vehiculo> GetByIdNoMap(int Id, string cadenaConexion);
        Task<List<VehiculoDTO>> GetVehiculos(int idEmpresa, string cadenaConexion);
        Task<(List<VehiculoDTO> listadoVehiculosDto, MetadataDto metadataDto)> GetVehiculosPaginated(VehiculoParams parameters, string cadenaConexion, int idEmpresa);
        Task InsertVehiculoAsync(VehiculoDTO vehiculo, string cadenaConexion, int idCreador);
        Task UpdateVehiculo(VehiculoDTO vehiculo, string cadenaConexion, int idEditor);
        Task DeleteById(int id, string cadenaConexion, int idBorrador);
    }

    public interface IModeloService {
        Task<ModeloDTO> GetById(int Id, string cadenaConexion);
        Task<Modelo> GetByIdNoMap(int Id, string cadenaConexion);
        Task<List<ModeloDTO>> GetModelos(string cadenaConexion, int idMarca);
        Task<(List<ModeloDTO> listadoModelosDto, MetadataDto metadataDto)> GetModelosPaginated(ModeloParams parameters, string cadenaConexion, int idEmpresa);
        Task InsertModeloAsync(ModeloDTO modelo, int idCreador, string cadenaConexion);
        Task UpdateModelo(ModeloDTO modeloDto, int idEditor, string cadenaConexion);
        Task DeleteById(int id, string cadenaConexion, int idBorrador);

    }

    public interface IMarcaService {
        Task<MarcaDTO> GetById(int Id, string cadenaConexion);
        Task<Marca> GetByIdNoMap(int Id, string cadenaConexion);
        Task<List<MarcaDTO>> GetMarcas(string cadenaConexion, int idEmpresa);
        Task<(List<MarcaDTO> listadoMarcasDto, MetadataDto metadataDto)> GetMarcasPaginatedAsync(MarcaParams parameters, string cadenaConexion, int idEmpresa);
        Task InsertMarcaAsync(MarcaDTO marca, int idCreador, string cadenaConexion);
        Task UpdateMarcaAsync(MarcaDTO marcaDto, int idEditor, string cadenaConexion);
        Task DeleteById(int id, string cadenaConexion, int idBorrador);
    }

    public interface ISesionService {
        Task<Usuario?> CheckUser(string email, string pass);

    }

    public interface IEmpresaActivaService {
        Task<EmpresasActiva?> CheckEmpresa(int IdUsuario);

    }

    public interface IEmpresaService {
        Task<EmpresaDTO> GetById(int id);
        Task<Empresa> GetByIdNoMap(int Id);
        Task InsertEmpresaAsync(EmpresaDTO empresa, int idCreador);
        Task UpdateEmpresa(Empresa empresa, int idEditor);
        Task DeleteById(int id, int idBorrador);
        Task<List<EmpresaDTO>> GetEmpresas( );
        Task<Empresa> CheckEmpresa(int IdUsuario);
        Task<string?> GenerarCadenaDeConexionAsync(int id);
    }

    public interface IEmpresasUsuariosService {
        Task<List<EmpresasUsuario>> GetEmpresasPorUsuario(int idUsuario);
    }

    public interface ICombustibleService {
        Task<CombustibleDTO> GetById(int id, string cadenaConexion);
        Task<List<CombustibleDTO>> GetAll(string cadenaConexion);
        Task<(List<CombustibleDTO> listadoCombustiblesDto, MetadataDto metadataDto)> GetCombustiblesPaginated(CombustibleParams parameters, string cadenaConexion, int idEmpresa);
        Task InsertCombustibleAsync(CombustibleDTO combustibleDto, int idCreador, string cadenaConexion);
        Task UpdateCombustible(CombustibleDTO combustibleDto, int idEditor, string cadenaConexion);
        Task DeleteById(int id, string cadenaConexion, int idBorrador);

    }

    public interface ICambioEmpresaActivaService {
        Task<Empresa> GetById(int id);
        Task CambioEmpresaActiva(int empAnterior, int empSiguiente);
        Task<IQueryable> GetAllEmpresasActivas( );
    }
}
