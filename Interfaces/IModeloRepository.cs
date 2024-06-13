using backend2.Modelos;
using backend2.Utilidad;

namespace backend2.Interfaces {
    public interface IModeloRepository {
        Task<Modelo> GetById(int id, bool obtenerDesactivados = false);
        Task InsertAsync(Modelo entity, int idCreador, string conn);
        Task Update(Modelo entity, int idEditor);
        Task DeleteById(int id, int idBorrador);
        int NextIdSequence(string conn);
        IQueryable<Modelo> GetAll(int idMarca, bool obtenerDesactivados = false);
        PagedList<Modelo> GetAllPaginated(ModeloParams parameters, int idEmpresa, bool obtenerDesactivados = false);
        void SearchByName(ref IQueryable<Modelo> modelos, string matricula);
        void ApplySort(ref IQueryable<Modelo> query, string orderByQueryString);
    }
}
