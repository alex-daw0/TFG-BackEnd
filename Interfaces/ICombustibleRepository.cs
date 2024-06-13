using backend2.Modelos;
using backend2.Utilidad;

namespace backend2.Interfaces {
    public interface ICombustibleRepository {
        Task<TiposCombustible> GetById(int id, bool obtenerDesactivados = false);
        Task InsertAsync(TiposCombustible entity, int idCreador, string conn);
        Task Update(TiposCombustible entity, int idEditor);
        Task DeleteById(int id, int idBorrador);
        int NextIdSequence(string conn);
        IQueryable<TiposCombustible> GetAll(bool obtenerDesactivados = false);
        PagedList<TiposCombustible> GetAllPaginated(CombustibleParams parameters, int idEmpresa, bool obtenerDesactivados = false);
        void SearchByName(ref IQueryable<TiposCombustible> combustibles, string matricula);
        void ApplySort(ref IQueryable<TiposCombustible> query, string orderByQueryString);
    }
}
