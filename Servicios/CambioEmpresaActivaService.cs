using backend2.Interfaces;
using backend2.Modelos;

namespace backend2.Servicios {
    public class CambioEmpresaActivaService : ICambioEmpresaActivaService {
        private readonly IUnitOfWork _unidadTrabajo;
        public CambioEmpresaActivaService(IUnitOfWork unidadDeTrabajo) {
            _unidadTrabajo = unidadDeTrabajo;
        }
        public async Task CambioEmpresaActiva(int empAnterior, int empSiguiente) {
            await _unidadTrabajo.RepositorioCambioEmpresa.CambioEmpresaActiva(empAnterior, empSiguiente);
            await _unidadTrabajo.SaveChangesAsync();

        }

        public async Task<IQueryable> GetAllEmpresasActivas( ) {
            return await _unidadTrabajo.RepositorioCambioEmpresa.GetAllEmpresasActivas();
        }

        public async Task<Empresa> GetById(int id) {
            return await _unidadTrabajo.RepositorioCambioEmpresa.GetById(id);
        }


    }
}
