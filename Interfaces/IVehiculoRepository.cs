using backend2.Modelos;
using backend2.Utilidad;
namespace backend2.Interfaces {
    public interface IVehiculoRepository {
        Task<Vehiculo> GetById(int id, bool obtenerDesactivados = false);
        Task InsertAsync(Vehiculo entity, int idCreador, string conn);
        void Update(Vehiculo entity, int idEditor);
        Task DeleteById(int id, int idBorrador);
        IQueryable<Vehiculo> GetAll(int idEmpresa, bool obtenerDesactivados = false);
        PagedList<Vehiculo> GetAllPaginated(VehiculoParams parameters, int idEmpresa, bool obtenerDesactivados = false);
        void SearchByName(ref IQueryable<Vehiculo> vehiculos, string matricula);
        void ApplySort(ref IQueryable<Vehiculo> query, string orderByQueryString);
        int NextIdSequence(string conn);

    }
}
