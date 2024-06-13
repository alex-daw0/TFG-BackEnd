using backend2.Interfaces;
using backend2.Modelos;

namespace backend2.Servicios {
    public class EmpresasUsuarioService : IEmpresasUsuariosService {
        private readonly IUnitOfWork _repositorioEspecifico;
        public EmpresasUsuarioService(IUnitOfWork repositorioEspecifico) {
            _repositorioEspecifico = repositorioEspecifico;
        }

        public async Task<List<EmpresasUsuario>?> GetEmpresasPorUsuario(int idUsuario) {
            IQueryable<EmpresasUsuario> listaEmpresas = _repositorioEspecifico.EmpresasUsuarioRepositorio.GetAll();
            List<EmpresasUsuario> listaFiltrada = listaEmpresas.Where(e => e.IdUsuario == idUsuario && (e.BorradoLogico == null || e.BorradoLogico == false) && e.FechaBorradoLogico == null).ToList();
            if (listaFiltrada == null) {
                Console.WriteLine("No hay empresas a las que pertenezca ese usuario");
                return null;
            } else {
                return listaFiltrada;

            }
        }
    }
}
