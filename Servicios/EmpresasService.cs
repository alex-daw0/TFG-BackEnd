using AutoMapper;
using backend2.DTO;
using backend2.Interfaces;
using backend2.Modelos;


namespace backend2.Servicios {
    public class ServicioEmpresa : IEmpresaService {

        private readonly IUnitOfWork _repositorioEspecifico;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public ServicioEmpresa(IUnitOfWork repositorioEspecifico, IConfiguration configuration, IMapper mapper) {
            _repositorioEspecifico = repositorioEspecifico;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task InsertEmpresaAsync(EmpresaDTO empresaDTO, int idCreador) {
            Empresa empresa = _mapper.Map<Empresa>(empresaDTO);
            string conn = _configuration.GetConnectionString("cadena");

            await _repositorioEspecifico.EmpresaRepositorio.InsertAsync(empresa, idCreador, conn);
            await _repositorioEspecifico.SaveChangesAsync();

        }

        public async Task<Empresa> CheckEmpresa(int id) {
            IQueryable<Empresa> listaEmpresas = _repositorioEspecifico.EmpresaRepositorio.GetAll();
            Empresa empresa = listaEmpresas.Where(e => e.Id == id).FirstOrDefault();

            return empresa;
        }

        public async Task DeleteById(int id, int idBorrador) {
            await _repositorioEspecifico.EmpresaRepositorio.DeleteById(id, idBorrador);
            await _repositorioEspecifico.SaveChangesAsync();

        }

        public async Task<Empresa> GetByIdNoMap(int id) {
            Empresa empresa = await _repositorioEspecifico.EmpresaRepositorio.GetById(id);
            return empresa;
        }

        public async Task<EmpresaDTO> GetById(int id) {
            Empresa empresa = await _repositorioEspecifico.EmpresaRepositorio.GetById(id);
            EmpresaDTO empresaDto = _mapper.Map<EmpresaDTO>(empresa);
            return empresaDto;

        }

        public async Task<string> GenerarCadenaDeConexionAsync(int id) {

            string cadenaDeConexion = "";

            var empresa = await GetByIdNoMap(id);

            string baseActiva = empresa.BaseActiva;
            string conn = _configuration.GetConnectionString("cadena");

            int igual1 = conn.IndexOf('=');
            int igual2 = conn.IndexOf('=', igual1 + 1) + 1;

            int punto1 = conn.IndexOf(';');
            int punto2 = conn.IndexOf(';', punto1 + 1);

            int length = punto2 - igual2;

            string oldConn = conn.Substring(igual2, length);
            cadenaDeConexion = conn.Replace(oldConn, baseActiva);

            return cadenaDeConexion;
        }

        public async Task UpdateEmpresa(Empresa empresa, int idEditor) {
            await _repositorioEspecifico.EmpresaRepositorio.Update(empresa, idEditor);
            await _repositorioEspecifico.SaveChangesAsync();

        }

        public async Task<List<EmpresaDTO>> GetEmpresas( ) {

            List<Empresa> arrayEmpresas = _repositorioEspecifico.EmpresaRepositorio.GetAll().ToList();
            List<EmpresaDTO> listaEmpresas = _mapper.Map<List<EmpresaDTO>>(arrayEmpresas);
            return listaEmpresas;
        }
    }

}
