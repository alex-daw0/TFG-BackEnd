
using backend2.Contexto;
using backend2.Interfaces;
using backend2.Modelos;
using backend2.Utilidad;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;

namespace backend2.Repositorios {
    public class VehiculoRepository : IVehiculoRepository {
        private readonly DbSet<Vehiculo> _vehiculos;
        private readonly DbSet<Modelo> _modelos;
        private readonly DbSet<Marca> _marcas;
        private readonly RegistroGeneralContext _context;
        public VehiculoRepository(RegistroGeneralContext context) {
            _vehiculos = context.Set<Vehiculo>();
            _modelos = context.Set<Modelo>();
            _marcas = context.Set<Marca>();
            _context = context;
        }

        public async Task<Vehiculo> GetById(int id, bool obtenerDesactivados = false) {
            var query = _vehiculos.Include(e => e.Modelo).Include(e => e.Marca).Include(e => e.TipoCombustible).AsQueryable();
            if(!obtenerDesactivados) {
                query = query.Where(e => e.BorradoLogico == false || e.BorradoLogico == null);
            }
            return query.Where(x => x.Id == id).FirstOrDefault();

        }


        public async Task InsertAsync(Vehiculo entity, int idCreador, string conn) {
            entity.IdCreador = idCreador;
            entity.FechaCreacion = DateTime.Now;
            entity.GuidRegistro = Guid.NewGuid().ToString().ToUpper();
            entity.Id = NextIdSequence(conn);
            await _vehiculos.AddAsync(entity);
        }

        public void Update(Vehiculo entity, int idEditor) {
            entity.IdEditor = idEditor;
            entity.FechaModificacion = DateTime.Now;
            _vehiculos.Update(entity);
        }

        public async Task DeleteById(int id, int idBorrador) {
            var elem = await _vehiculos.FindAsync(id);
            if (elem != null) {
                elem.IdBorrador = idBorrador;
                elem.FechaBorradoLogico = DateTime.Now;
                elem.BorradoLogico = true;

                _vehiculos.Update(elem);
                

            }
        }

        public IQueryable<Vehiculo> GetAll(int idEmpresa, bool obtenerDesactivados = false) {
            var query = _vehiculos.AsNoTracking().Include(e => e.Modelo).Include(e => e.Marca).Include(e => e.TipoCombustible).Where(e => e.IdEmpresa == idEmpresa);

            if (!obtenerDesactivados) {
                query = query.Where(e => e.BorradoLogico == null || e.BorradoLogico == false);
            }

            return query;
        }

        public PagedList<Vehiculo> GetAllPaginated(VehiculoParams parameters, int idEmpresa, bool obtenerDesactivados = false) {
            var query = _vehiculos.AsNoTracking().Include(e => e.Modelo).Include(e => e.Marca).Include(e => e.TipoCombustible).Where(e => e.IdEmpresa == idEmpresa);

            var listadoElemetno = query.ToList();

            if (parameters.Marca != null) {
                query = query.Where(e => e.MarcaId == parameters.Marca);

            }

            if (parameters.Modelo != null) {
                query = query.Where(e => e.ModeloId == parameters.Modelo);
            }

            if (parameters.Combustible != null) {
                query = query.Where(e => e.TipoCombustibleId == parameters.Combustible);
            }

            if (!obtenerDesactivados) {
                query = query.Where(e => e.BorradoLogico == null || e.BorradoLogico == false);
            }

                

            SearchByName(ref query, parameters.Matricula);
            ApplySort(ref query, parameters.OrderBy);

            return PagedList<Vehiculo>.ToPagedList(query, parameters.PageNumber, parameters.PageSize);
        }

        public void SearchByName(ref IQueryable<Vehiculo> query, string matricula) {
            if (!query.Any() || string.IsNullOrWhiteSpace(matricula))
                return;

            query = query.Where(v => v.Matricula.ToLower().Contains(matricula.Trim().ToLower()));
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


        public void ApplySort(ref IQueryable<Vehiculo> query, string orderByQueryString) {
            if (!query.Any())
                return;

            if (string.IsNullOrWhiteSpace(orderByQueryString)) {
                query = query.OrderBy(x => x.Matricula);
                return;
            }

            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(Vehiculo).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();

            foreach (var param in orderParams) {
                if (string.IsNullOrWhiteSpace(param))
                    continue;

                var propertyFromQueryName = param.Split(" ")[0];
                var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

                if (objectProperty == null)
                    continue;

                var sortingOrder = param.EndsWith(" desc") ? "descending" : "ascending";
                if (objectProperty.Name.ToString().ToUpper() != "MATRICULA") {
                    orderQueryBuilder.Append($"{objectProperty.Name.ToString() + ".Nombre"} {sortingOrder}, ");
                } else {
                    orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {sortingOrder}, ");
                }
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

            if (string.IsNullOrWhiteSpace(orderQuery)) {
                query = query.OrderBy(x => x.Matricula);
                return;
            }

            query = query.OrderBy(orderQuery);
        }


    }
}
