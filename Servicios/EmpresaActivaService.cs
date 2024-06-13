using backend2.Interfaces;
using backend2.Modelos;

namespace backend2.Servicios {
    public class EmpresaActivaService : IEmpresaActivaService {

        private readonly IUnitOfWork _repositorioEspecifico;
        public EmpresaActivaService(IUnitOfWork repositorioEspecifico) {
            _repositorioEspecifico = repositorioEspecifico;
        }

        public async Task<EmpresasActiva?> CheckEmpresa(int IdUsuario) {
            IQueryable<EmpresasActiva> listaEmpresasActivas = _repositorioEspecifico.EmpresasActivaRepositorio.GetAll();
            return listaEmpresasActivas.Where(e => e.IdUsuario == IdUsuario).FirstOrDefault();
        }
    }
}
