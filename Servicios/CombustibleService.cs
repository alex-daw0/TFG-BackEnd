using AutoMapper;
using backend2.Contexto;
using backend2.DTO;
using backend2.Interfaces;
using backend2.Modelos;
using backend2.Repositorios;
using backend2.Utilidad;

namespace backend2.Servicios {
    public class CombustibleService : ICombustibleService {
        private readonly IUnitOfWork _repositorioEspecifico;
        private readonly RegistroGeneralContext _context;
        private readonly IMapper _mapper;

        public CombustibleService(IUnitOfWork repositorioEspecifico, RegistroGeneralContext context, IMapper mapper) {
            _repositorioEspecifico = repositorioEspecifico;
            _context = context;
            _mapper = mapper;
        }

        public async Task InsertCombustibleAsync(CombustibleDTO combustibleDto, int idCreador, string cadenaConexion) {
            using (IUnitOfWork repositorioEspecifico = new UnitOfWork(cadenaConexion)) {
                TiposCombustible combustible = _mapper.Map<TiposCombustible>(combustibleDto);
                await repositorioEspecifico.CombustibleRepositorio.InsertAsync(combustible, idCreador, cadenaConexion);
                                    await repositorioEspecifico.SaveChangesAsync();

            }
        }

        public async Task DeleteById(int id, string cadenaConexion, int idBorrador) {
            using (IUnitOfWork repositorioEspecifico = new UnitOfWork(cadenaConexion)) {
                await repositorioEspecifico.CombustibleRepositorio.DeleteById(id, idBorrador);
                                    await repositorioEspecifico.SaveChangesAsync();

            }
        }

        public async Task<CombustibleDTO> GetById(int Id, string cadenaConexion) {
            TiposCombustible combustible = new();
            using (IUnitOfWork repositorioEspecifico = new UnitOfWork(cadenaConexion)) {
                combustible = await repositorioEspecifico.CombustibleRepositorio.GetById(Id);
            }
            CombustibleDTO combustibleDto = _mapper.Map<CombustibleDTO>(combustible);
            return combustibleDto;
        }

        public async Task<List<CombustibleDTO>> GetAll(string cadenaConexion) {
            List<TiposCombustible> combustibles = new();
            using (IUnitOfWork repositorioEspecifico = new UnitOfWork(cadenaConexion)) {
                combustibles = repositorioEspecifico.CombustibleRepositorio.GetAll().ToList();

            }

            List<CombustibleDTO> listaDto = _mapper.Map<List<CombustibleDTO>>(combustibles);
            return listaDto;

        }

        public async Task<(List<CombustibleDTO> listadoCombustiblesDto, MetadataDto metadataDto)> GetCombustiblesPaginated(CombustibleParams parameters, string cadenaConexion, int idEmpresa) {
            PagedList<TiposCombustible> arrayCombustibles = new PagedList<TiposCombustible>();
            using (IUnitOfWork repositorioEspecifico = new UnitOfWork(cadenaConexion)) {
                arrayCombustibles = repositorioEspecifico.CombustibleRepositorio.GetAllPaginated(parameters, idEmpresa);

            }

            List<CombustibleDTO> CombustiblesDTO = _mapper.Map<List<CombustibleDTO>>(arrayCombustibles);
            MetadataDto metadataDto = new MetadataDto(arrayCombustibles.TotalCount, arrayCombustibles.PageSize, arrayCombustibles.CurrentPage, arrayCombustibles.TotalPages, arrayCombustibles.HasNext, arrayCombustibles.HasPrevious);

            return (CombustiblesDTO, metadataDto);
        }

        private void SearchByName(ref IQueryable<TiposCombustible> combustibles, string nombre, string cadenaConexion) {
            using (IUnitOfWork repositorioEspecifico = new UnitOfWork(cadenaConexion)) {

                if (!combustibles.Any() || string.IsNullOrEmpty(nombre))
                    return;

                combustibles = combustibles.Where(v => v.Nombre.ToLower().Contains(nombre.Trim().ToLower()));
            }

        }


        public async Task UpdateCombustible(CombustibleDTO combustibleDto, int idEditor, string cadenaConexion) {
            using (IUnitOfWork repositorioEspecifico = new UnitOfWork(cadenaConexion)) {
                    TiposCombustible combustible = await repositorioEspecifico.CombustibleRepositorio.GetById(combustibleDto.Id);
                    if (!combustibleDto.Equals(combustible)) {
                        combustible.Nombre = combustibleDto.Nombre;
                    }
                if (combustible.IdEmpresa != 0) {
                    await repositorioEspecifico.CombustibleRepositorio.Update(combustible, idEditor);
                    await repositorioEspecifico.SaveChangesAsync();

                }

            }
        }
    }
}
