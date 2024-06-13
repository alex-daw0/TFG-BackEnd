using backend2.Modelos;

namespace backend2.Interfaces {
    public interface ICambioEmpresaRepository {
        Task<Empresa> GetById(int id);
        Task CambioEmpresaActiva(int empresaAnterior, int empresaSiguiente);
        Task<IQueryable<EmpresasActiva>> GetAllEmpresasActivas( );
        public int SaveChanges( );
    }
}
