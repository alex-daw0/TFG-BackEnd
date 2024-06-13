using AutoMapper;
using backend2.DTO;
using backend2.Interfaces;
using backend2.Modelos;
using backend2.Repositorios;
using backend2.Utilidad;

namespace backend2.Servicios {
    public class MarcaService : IMarcaService {
        private readonly IMapper _mapper;

        public MarcaService(IMapper mapper) {
            _mapper = mapper;
        }

        public async Task InsertMarcaAsync(MarcaDTO marcaDto, int idCreador, string cadenaConexion) {
            using (IUnitOfWork repositorioEspecifico = new UnitOfWork(cadenaConexion)) {
                Marca marca = _mapper.Map<Marca>(marcaDto);

                await repositorioEspecifico.MarcaRepositorio.InsertAsync(marca, idCreador, cadenaConexion);

                                    await repositorioEspecifico.SaveChangesAsync();

            }
        }

        public async Task DeleteById(int id, string cadenaConexion, int idBorrador) {
            using (IUnitOfWork repositorioEspecifico = new UnitOfWork(cadenaConexion)) {
                await repositorioEspecifico.MarcaRepositorio.DeleteById(id, idBorrador);
                                    await repositorioEspecifico.SaveChangesAsync();

            }
        }

        public async Task<Marca> GetByIdNoMap(int Id, string cadenaConexion) {
            using (IUnitOfWork repositorioEspecifico = new UnitOfWork(cadenaConexion)) {
                return await repositorioEspecifico.MarcaRepositorio.GetById(Id);
            }
        }

        public async Task<MarcaDTO> GetById(int Id, string cadenaConexion) {
            Marca marca = new();

            using (IUnitOfWork repositorioEspecifico = new UnitOfWork(cadenaConexion)) {
                marca = await repositorioEspecifico.MarcaRepositorio.GetById(Id);

            }
            MarcaDTO marcaDTO = _mapper.Map<MarcaDTO>(marca);

            return marcaDTO;
        }

        public async Task<List<MarcaDTO>> GetMarcas(string cadenaConexion, int idEmpresa) {
            List<Marca> arrayMarcas = [];

            using (IUnitOfWork repositorioEspecifico = new UnitOfWork(cadenaConexion)) {
                arrayMarcas = repositorioEspecifico.MarcaRepositorio.GetAll(idEmpresa).ToList();

            }

            List<MarcaDTO> listamarcas = _mapper.Map<List<MarcaDTO>>(arrayMarcas);

            return listamarcas;

        }

        public async Task<(List<MarcaDTO> listadoMarcasDto, MetadataDto metadataDto)> GetMarcasPaginatedAsync(MarcaParams parameters, string cadenaConexion, int idEmpresa) {
            PagedList<Marca> arrayMarcas = [];

            using (IUnitOfWork repositorioEspecifico = new UnitOfWork(cadenaConexion)) {
                arrayMarcas = repositorioEspecifico.MarcaRepositorio.GetAllPaginated(parameters, idEmpresa);
            }

            List<MarcaDTO> MarcasDTO = _mapper.Map<List<MarcaDTO>>(arrayMarcas);
            MetadataDto metadataDto =
                new(arrayMarcas.TotalCount, arrayMarcas.PageSize, arrayMarcas.CurrentPage, arrayMarcas.TotalPages, arrayMarcas.HasNext, arrayMarcas.HasPrevious);

            return (MarcasDTO, metadataDto);
        }

        private void SearchByName(ref IQueryable<Marca> marcas, string nombre, string cadenaConexion) {
            using (IUnitOfWork repositorioEspecifico = new UnitOfWork(cadenaConexion)) {
                if (!marcas.Any() || string.IsNullOrEmpty(nombre)) {
                    return;
                }

                marcas = marcas.Where(v => v.Nombre.ToLower().Contains(nombre.Trim().ToLower()));
            }

        }


        public async Task UpdateMarcaAsync(MarcaDTO marcaDto, int idEditor, string cadenaConexion) {
            Marca marca = new();

            using (IUnitOfWork repositorioEspecifico = new UnitOfWork(cadenaConexion)) {
                marca = await repositorioEspecifico.MarcaRepositorio.GetById(marcaDto.Id);
                if (marca != null) {
                    if (!marcaDto.Equals(marca)) {
                        marca.Nombre = marcaDto.Nombre;
                    }

                    await repositorioEspecifico.MarcaRepositorio.Update(marca, idEditor);

                    await repositorioEspecifico.SaveChangesAsync();

                }
            }
        }
    }
}
