using AutoMapper;
using backend2.Contexto;
using backend2.DTO;
using backend2.Interfaces;
using backend2.Modelos;
using backend2.Repositorios;
using backend2.Utilidad;

namespace backend2.Servicios {
    public class ModeloService : IModeloService {
        private readonly IUnitOfWork _repositorioEspecifico;
        private readonly RegistroGeneralContext _context;
        private readonly IMapper _mapper;

        public ModeloService(IUnitOfWork repositorioEspecifico, RegistroGeneralContext context, IMapper mapper) {
            _repositorioEspecifico = repositorioEspecifico;
            _context = context;
            _mapper = mapper;
        }

        public async Task InsertModeloAsync(ModeloDTO modeloDto, int idCreador, string cadenaConexion) {
            using (IUnitOfWork repositorioEspecifico = new UnitOfWork(cadenaConexion)) {
                Modelo modelo = _mapper.Map<Modelo>(modeloDto);
                await repositorioEspecifico.ModeloRepositorio.InsertAsync(modelo, idCreador, cadenaConexion);
                await repositorioEspecifico.SaveChangesAsync();
            }
        }

        public async Task DeleteById(int id, string cadenaConexion, int idBorrador) {
            using (IUnitOfWork repositorioEspecifico = new UnitOfWork(cadenaConexion)) {
                await repositorioEspecifico.ModeloRepositorio.DeleteById(id, idBorrador);
                    await repositorioEspecifico.SaveChangesAsync();
            }
        }

        public async Task<Modelo> GetByIdNoMap(int Id, string cadenaConexion) {
            using (IUnitOfWork repositorioEspecifico = new UnitOfWork(cadenaConexion)) {
                return await repositorioEspecifico.ModeloRepositorio.GetById(Id);
            }
        }

        public async Task<ModeloDTO> GetById(int Id, string cadenaConexion) {
            Modelo modelo = new();
            using (IUnitOfWork repositorioEspecifico = new UnitOfWork(cadenaConexion)) {
                modelo = await repositorioEspecifico.ModeloRepositorio.GetById(Id);
            }
            ModeloDTO modeloDTO = _mapper.Map<ModeloDTO>(modelo);
            return modeloDTO;

        }



        public async Task<List<ModeloDTO>> GetModelos(string cadenaConexion, int idEmpresa) {
            List<Modelo> arrayModelos = new List<Modelo>();
            using (IUnitOfWork repositorioEspecifico = new UnitOfWork(cadenaConexion)) {
                arrayModelos = repositorioEspecifico.ModeloRepositorio.GetAll(idEmpresa).ToList();

            }
            List<ModeloDTO> listamodelos = _mapper.Map<List<ModeloDTO>>(arrayModelos);
            return listamodelos;

        }

        public async Task<(List<ModeloDTO> listadoModelosDto, MetadataDto metadataDto)> GetModelosPaginated(ModeloParams parameters, string cadenaConexion, int idEmpresa) {
            PagedList<Modelo> arrayModelos = new PagedList<Modelo>();

            using (IUnitOfWork repositorioEspecifico = new UnitOfWork(cadenaConexion)) {
                arrayModelos = repositorioEspecifico.ModeloRepositorio.GetAllPaginated(parameters, idEmpresa);

            }

            List<ModeloDTO> ModelosDTO = _mapper.Map<List<ModeloDTO>>(arrayModelos);
            MetadataDto metadataDto = new MetadataDto(arrayModelos.TotalCount, arrayModelos.PageSize, arrayModelos.CurrentPage, arrayModelos.TotalPages, arrayModelos.HasNext, arrayModelos.HasPrevious);

            return (ModelosDTO, metadataDto);
        }

        private void SearchByName(ref IQueryable<Modelo> modelos, string nombre, string cadenaConexion) {
            using (IUnitOfWork repositorioEspecifico = new UnitOfWork(cadenaConexion)) {

                if (!modelos.Any() || string.IsNullOrEmpty(nombre))
                    return;

                modelos = modelos.Where(v => v.Nombre.ToLower().Contains(nombre.Trim().ToLower()));
            }

        }


        public async Task UpdateModelo(ModeloDTO modeloDto, int idEditor, string cadenaConexion) {
            Modelo modelo = new();
            using (IUnitOfWork repositorioEspecifico = new UnitOfWork(cadenaConexion)) {
                modelo = await repositorioEspecifico.ModeloRepositorio.GetById(modeloDto.Id);

                if (modelo != null) {
                    if (!modeloDto.Equals(modelo)) {
                        modelo.Nombre = modeloDto.Nombre;
                        modelo.MarcaId = modeloDto.IdMarca;
                    }
                    repositorioEspecifico.ModeloRepositorio.Update(modelo, idEditor);
                    await repositorioEspecifico.SaveChangesAsync();
                }
            }
        }
    }
}
