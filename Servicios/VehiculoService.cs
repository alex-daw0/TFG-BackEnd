using AutoMapper;
using backend2.Contexto;
using backend2.DTO;
using backend2.Interfaces;
using backend2.Modelos;
using backend2.Repositorios;
using backend2.Utilidad;

namespace backend2.Servicios {
    public class VehiculoService : IVehiculoService {
        private readonly IUnitOfWork _repositorioEspecifico;
        private readonly RegistroGeneralContext _context;
        private readonly IMapper _mapper;

        public VehiculoService(IUnitOfWork repositorioEspecifico, RegistroGeneralContext context, IMapper mapper) {
            _repositorioEspecifico = repositorioEspecifico;
            _context = context;
            _mapper = mapper;
        }

        public async Task InsertVehiculoAsync(VehiculoDTO vehiculoDTO, string cadenaConexion, int idCreador) {
            using (IUnitOfWork repositorioEspecifico = new UnitOfWork(cadenaConexion)) {
                Vehiculo vehiculo = _mapper.Map<Vehiculo>(vehiculoDTO);
                await repositorioEspecifico.VehiculoRepositorio.InsertAsync(vehiculo, idCreador, cadenaConexion);
                    await repositorioEspecifico.SaveChangesAsync();
            }
        }

        public async Task DeleteById(int id, string cadenaConexion, int idBorrador) {
            using (IUnitOfWork repositorioEspecifico = new UnitOfWork(cadenaConexion)) {
                await repositorioEspecifico.VehiculoRepositorio.DeleteById(id, idBorrador);
                await repositorioEspecifico.SaveChangesAsync();
            }
        }

        public async Task<VehiculoDTO> GetById(int Id, string cadenaConexion) {
            Vehiculo vehiculo = new Vehiculo();
            using (IUnitOfWork repositorioEspecifico = new UnitOfWork(cadenaConexion)) {

                vehiculo = await repositorioEspecifico.VehiculoRepositorio.GetById(Id);

            }

            VehiculoDTO vehiculodto = _mapper.Map<VehiculoDTO>(vehiculo);
            return vehiculodto;
        }

        public async Task<Vehiculo> GetByIdNoMap(int Id, string cadenaConexion) {
            using (IUnitOfWork repositorioEspecifico = new UnitOfWork(cadenaConexion)) {

                return await repositorioEspecifico.VehiculoRepositorio.GetById(Id);

            }
        }

        public async Task<List<VehiculoDTO>> GetVehiculos(int idEmpresa, string cadenaConexion) {
            List<Vehiculo> arrayVehiculos = new List<Vehiculo>();
            using (IUnitOfWork repositorioEspecifico = new UnitOfWork(cadenaConexion)) {
                arrayVehiculos = repositorioEspecifico.VehiculoRepositorio.GetAll(idEmpresa).ToList();

            }
            List<VehiculoDTO> listaVehiculos = _mapper.Map<List<VehiculoDTO>>(arrayVehiculos);
            return listaVehiculos;

        }

        public async Task<(List<VehiculoDTO> listadoVehiculosDto, MetadataDto metadataDto)> GetVehiculosPaginated(VehiculoParams parameters, string cadenaConexion, int idEmpresa) {
            PagedList<Vehiculo> arrayVehiculos = new PagedList<Vehiculo>();

            using (IUnitOfWork repositorioEspecifico = new UnitOfWork(cadenaConexion)) {
                arrayVehiculos = repositorioEspecifico.VehiculoRepositorio.GetAllPaginated(parameters, idEmpresa);

            }

            List<VehiculoDTO> vehiculoDTOs = _mapper.Map<List<VehiculoDTO>>(arrayVehiculos);
            MetadataDto metadataDto = new MetadataDto(arrayVehiculos.TotalCount, arrayVehiculos.PageSize, arrayVehiculos.CurrentPage, arrayVehiculos.TotalPages, arrayVehiculos.HasNext, arrayVehiculos.HasPrevious);

            return (vehiculoDTOs, metadataDto);
        }

        private void SearchByName(ref IQueryable<Vehiculo> vehiculos, string matricula, string cadenaConexion) {
            using (IUnitOfWork repositorioEspecifico = new UnitOfWork(cadenaConexion)) {
                repositorioEspecifico.VehiculoRepositorio.SearchByName(ref vehiculos, matricula);
            }

        }


        public async Task UpdateVehiculo(VehiculoDTO vehiculoDto, string cadenaConexion, int idEditor) {
            using (IUnitOfWork repositorioEspecifico = new UnitOfWork(cadenaConexion)) {
                Vehiculo vehiculo = await repositorioEspecifico.VehiculoRepositorio.GetById(vehiculoDto.Id);

                if (vehiculo != null) {
                    if (!vehiculoDto.Equals(vehiculo)) {
                        vehiculo.Matricula = vehiculoDto.Matricula;
                        vehiculo.MarcaId = vehiculoDto.IdMarca;
                        vehiculo.ModeloId = vehiculoDto.IdModelo;
                        vehiculo.TipoCombustibleId = vehiculoDto.Id_TipoCombustible;
                    }
                    repositorioEspecifico.VehiculoRepositorio.Update(vehiculo, idEditor);
                    await repositorioEspecifico.SaveChangesAsync();
                }
            }
        }
    }
}
