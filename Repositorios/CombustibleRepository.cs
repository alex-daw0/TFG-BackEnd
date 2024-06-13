using backend2.Contexto;
using backend2.Interfaces;
using backend2.Modelos;
using backend2.Utilidad;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Net.Quic;
using System.Reflection;
using System.Text;

namespace backend2.Repositorios {
    public class CombustibleRepository : ICombustibleRepository {
        private readonly DbSet<TiposCombustible> _entity;
        public CombustibleRepository(RegistroGeneralContext context) {
            _entity = context.Set<TiposCombustible>();
        }

        public async Task<TiposCombustible> GetById(int id, bool obtenerDesactivados = false) {
            var query = _entity.AsNoTracking().AsQueryable();
            if(!obtenerDesactivados) {
                query = query.Where(c => c.BorradoLogico == null || c.BorradoLogico == false);
            }
            return query.Where(x => x.Id == id).FirstOrDefault();

        }

        public async Task InsertAsync(TiposCombustible entity, int idCreador, string conn) {
            entity.IdCreador = idCreador;
            entity.FechaCreacion = DateTime.Now;
            entity.GuidRegistro = Guid.NewGuid().ToString().ToUpper();
            entity.Id = NextIdSequence(conn);
            _entity.Add(entity);
        }

        public async Task Update(TiposCombustible entity, int idEditor) {
            if (entity.IdEmpresa != 0) {
                entity.IdEditor = idEditor;
                entity.FechaModificacion = DateTime.Now;
                _entity.Update(entity);
            }
        }

        public async Task DeleteById(int id, int idBorrador) {
            var elem = await _entity.FindAsync(id);
            if (elem != null) {
                if (elem.IdEmpresa != 0) {
                    elem.IdBorrador = idBorrador;
                    elem.FechaBorradoLogico = DateTime.Now;
                    elem.BorradoLogico = true;
                    _entity.Update(elem);
                }
            }
        }

        public int NextIdSequence(string conn) {
            int nextId = 0;
            using (SqlConnection connection = new SqlConnection(conn)) {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT NEXT VALUE FOR SECUENCIAIDENTIFICADORES", connection)) {
                    nextId = (int)cmd.ExecuteScalar();
                }
                connection.Close();
            }
            return nextId;
        }

        public IQueryable<TiposCombustible> GetAll(bool obtenerDesactivados = false) {
            return _entity.Where(e => e.BorradoLogico == null || e.BorradoLogico == false);

        }


        public PagedList<TiposCombustible> GetAllPaginated(CombustibleParams parameters, int idEmpresa, bool obtenerDesactivados = false) {


            var query = _entity.Where(e => e.IdEmpresa == 0 || e.IdEmpresa == idEmpresa);

            if (!obtenerDesactivados) {
                query = query.Where(e => e.BorradoLogico == null || e.BorradoLogico == false);
            }

            SearchByName(ref query, parameters.Nombre);
            ApplySort(ref query, parameters.OrderBy);

            return PagedList<TiposCombustible>.ToPagedList(query, parameters.PageNumber, parameters.PageSize);
        }

        public void SearchByName(ref IQueryable<TiposCombustible> query, string nombre) {
            if (!query.Any() || string.IsNullOrWhiteSpace(nombre))
                return;

            query = query.Where(v => v.Nombre.ToLower().Contains(nombre.Trim().ToLower()));
        }
        public void ApplySort(ref IQueryable<TiposCombustible> query, string orderByQueryString) {
            if (!query.Any())
                return;

            if (string.IsNullOrWhiteSpace(orderByQueryString)) {
                query = query.OrderBy(x => x.Nombre);
                return;
            }

            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(TiposCombustible).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();

            foreach (var param in orderParams) {
                if (string.IsNullOrWhiteSpace(param))
                    continue;

                var propertyFromQueryName = param.Split(" ")[0];
                var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

                if (objectProperty == null)
                    continue;

                var sortingOrder = param.EndsWith(" desc") ? "descending" : "ascending";
                if (objectProperty.Name.ToString().ToUpper() != "NOMBRE") {
                    orderQueryBuilder.Append($"{objectProperty.Name.ToString() + ".Nombre"} {sortingOrder}, ");
                } else {
                    orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {sortingOrder}, ");
                }
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

            if (string.IsNullOrWhiteSpace(orderQuery)) {
                query = query.OrderBy(x => x.Nombre);
                return;
            }

            query = query.OrderBy(orderQuery);
        }

    }
}
