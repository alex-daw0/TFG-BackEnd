using backend2.Contexto;
using backend2.Interfaces;
using backend2.Modelos;

namespace backend2.Repositorios {
    public class UnitOfWork : IUnitOfWork, IDisposable {
        private readonly RegistroGeneralContext _context;
        private string _cadenaConexion;

        public UnitOfWork(RegistroGeneralContext context) {
            _context = context;
        }

        public UnitOfWork(string? cadenaConexion) {
            _cadenaConexion = cadenaConexion;
            _context = new RegistroGeneralContext(_cadenaConexion);
        }

        private IVehiculoRepository? _repositorioVehiculo;
        private IModeloRepository? _repositorioModelo;
        private IMarcaRepository? _repositorioMarca;
        private IGenericRepository<Usuario>? _repositorioUsuario;
        private IGenericRepository<EmpresasActiva>? _repositorioEmpresasActiva;
        private IEmpresaRepository? _repositorioEmpresa;
        private ICombustibleRepository? _repositorioCombustible;
        private IGenericRepository<EmpresasUsuario>? _repositorioEmpresasUsuario;
        private ICambioEmpresaRepository? _repositorioCambioEmpresa;

        IGenericRepository<Usuario> IUnitOfWork.UsuariosRepositorio => _repositorioUsuario ??= new GenericRepository<Usuario>(_context);
        IGenericRepository<EmpresasActiva> IUnitOfWork.EmpresasActivaRepositorio => _repositorioEmpresasActiva ??= new GenericRepository<EmpresasActiva>(_context);
        IGenericRepository<EmpresasUsuario> IUnitOfWork.EmpresasUsuarioRepositorio => _repositorioEmpresasUsuario ??= new GenericRepository<EmpresasUsuario>(_context);
        public ICombustibleRepository CombustibleRepositorio => _repositorioCombustible ??= new CombustibleRepository(_context);
        public IModeloRepository ModeloRepositorio => _repositorioModelo ??= new ModeloRepository(_context);
        public IMarcaRepository MarcaRepositorio => _repositorioMarca ??= new MarcaRepository(_context);
        public ICambioEmpresaRepository RepositorioCambioEmpresa => _repositorioCambioEmpresa ??= new CambioEmpresaRepository(_context);
        public IEmpresaRepository EmpresaRepositorio => _repositorioEmpresa ??= new EmpresaRepository(_context);
        public IVehiculoRepository VehiculoRepositorio => _repositorioVehiculo ??= new VehiculoRepository(_context);


        public void Dispose( ) {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            _context?.Dispose();
        }

        public int SaveChanges( ) {

            /*foreach (var item in _context.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Deleted &&
            e.Metadata.GetProperties().Any(x => x.Name == "BorradoLogico")))
            {
                item.State = EntityState.Unchanged;
                item.CurrentValues["BorradoLogico"] = true;
                item.CurrentValues["FechaBorradoLogico"] = DateTime.Now;
            }*/
            return _context.SaveChanges();
        }

        public Task<int> SaveChangesAsync( ) {
            return _context.SaveChangesAsync();
        }
    }
}
