using backend2.Contexto;
using backend2.Interfaces;
using backend2.Modelos;
using Microsoft.EntityFrameworkCore;

namespace backend2.Repositorios {
    public class CambioEmpresaRepository : GenericRepository<EmpresasActiva>, ICambioEmpresaRepository {
        private readonly DbSet<EmpresasActiva> _empresasActiva;
        private readonly DbSet<Empresa> _empresa;
        private readonly RegistroGeneralContext _context;

        public CambioEmpresaRepository(RegistroGeneralContext context) : base(context) {
            _empresa = context.Set<Empresa>();
            _empresasActiva = context.Set<EmpresasActiva>();
        }


        public async Task CambioEmpresaActiva(int empresaAnterior, int empresaSiguiente) {
            Empresa empAnterior = await GetById(empresaAnterior);
            Empresa empSiguiente = await GetById(empresaSiguiente);

            if (empAnterior != null && empSiguiente != null) {
                IQueryable<EmpresasActiva> empresasActivas = await GetAllEmpresasActivas();

                EmpresasActiva emp = empresasActivas.Where(e => e.IdEmpresa == empresaAnterior).FirstOrDefault();
                emp.IdEmpresa = empresaSiguiente;
            }


        }

        public async Task<IQueryable<EmpresasActiva>> GetAllEmpresasActivas( ) {
            return _empresasActiva.AsQueryable();
        }

        public async Task<Empresa> GetById(int id) {
            return await _empresa.FindAsync(id);
        }

        public int SaveChanges( ) {
            return _context.SaveChanges();
        }
    }
}
