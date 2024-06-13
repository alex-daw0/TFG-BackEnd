using backend2.Modelos;
using backend2.Utilidad;

namespace backend2.Interfaces {
    public interface IEmpresaRepository {
        Task<Empresa> GetById(int id, bool obtenerDesactivadosd = false);
        Task InsertAsync(Empresa entity, int idCreador, string conn);
        Task Update(Empresa entity, int idEditor);
        Task DeleteById(int id, int idBorrador);
        int NextIdSequence(string conn);
        IQueryable<Empresa> GetAll(bool obtenerDesactivados = false);
        PagedList<Empresa> GetAllPaginated(QueryStringParameters parameters);
    }
}
