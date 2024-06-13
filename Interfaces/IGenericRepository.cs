using backend2.Utilidad;

namespace backend2.Interfaces {
    public interface IGenericRepository<TEntity> where TEntity : class {
        Task<TEntity> GetById(int id);
        Task Insert(TEntity entity);
        Task Update(TEntity entity);
        Task DeleteById(int id);
        IQueryable<TEntity> GetAll( );
        PagedList<TEntity> GetAllPaginated(QueryStringParameters parameters);
    }
}