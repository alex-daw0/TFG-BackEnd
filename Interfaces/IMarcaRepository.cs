using backend2.Modelos;
using backend2.Utilidad;

namespace backend2.Interfaces {
    public interface IMarcaRepository {
        Task<Marca> GetById(int id, bool obtenerDesactivados = false);
        Task InsertAsync(Marca entity, int idCreador, string conn);
        Task Update(Marca entity, int idEditor);
        Task DeleteById(int id, int idBorrador);
        int NextIdSequence(string conn);
        IQueryable<Marca> GetAll(int idEmpresa, bool obtenerDesactivados = false);
        PagedList<Marca> GetAllPaginated(MarcaParams parameters, int idEmpresa, bool obtenerDesactivados = false);
        void SearchByName(ref IQueryable<Marca> marcas, string matricula);
        void ApplySort(ref IQueryable<Marca> query, string orderByQueryString);
    }
}
