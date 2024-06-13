using backend2.Contexto;
using backend2.Interfaces;
using backend2.Utilidad;
using Microsoft.EntityFrameworkCore;

namespace backend2.Repositorios {
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class {

        private readonly DbSet<TEntity> _entity;
        public GenericRepository(RegistroGeneralContext context) {
            _entity = context.Set<TEntity>();
        }

        public async Task<TEntity> GetById(int id) {
            return await _entity.FindAsync(id);
        }

        public async Task Insert(TEntity entity) {
            _entity.Add(entity);
        }

        public async Task Update(TEntity entity) {
            _entity.Update(entity);
        }

        public async Task DeleteById(int id) {
            var elem = await _entity.FindAsync(id);
            if (elem != null) {
                _entity.Remove(elem);
            }
        }

        public IQueryable<TEntity> GetAll( ) {
            return _entity.AsQueryable();
        }

        public IQueryable<TEntity> GetAllPaginated(VehiculoParams parameters) {
            return _entity.AsQueryable().Skip((parameters.PageNumber - 1) * parameters.PageSize).Take(parameters.PageSize);
        }

        public PagedList<TEntity> GetAllPaginated(QueryStringParameters parameters) {
            return PagedList<TEntity>.ToPagedList(_entity.AsQueryable(), parameters.PageNumber, parameters.PageSize);
        }

    }
}
