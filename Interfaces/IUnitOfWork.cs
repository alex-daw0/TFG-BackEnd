using backend2.Modelos;
namespace backend2.Interfaces {
    public interface IUnitOfWork : IDisposable {
        IVehiculoRepository VehiculoRepositorio { get; }
        IMarcaRepository MarcaRepositorio { get; }
        IModeloRepository ModeloRepositorio { get; }
        IGenericRepository<Usuario> UsuariosRepositorio { get; }
        IGenericRepository<EmpresasActiva> EmpresasActivaRepositorio { get; }
        IEmpresaRepository EmpresaRepositorio { get; }
        ICombustibleRepository CombustibleRepositorio { get; }
        IGenericRepository<EmpresasUsuario> EmpresasUsuarioRepositorio { get; }
        ICambioEmpresaRepository RepositorioCambioEmpresa { get; }
        public int SaveChanges( );
        public Task<int> SaveChangesAsync( );
    }

}
