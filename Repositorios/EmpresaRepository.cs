using backend2.Contexto;
using backend2.Interfaces;
using backend2.Modelos;
using backend2.Utilidad;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace backend2.Repositorios {
    public class EmpresaRepository : IEmpresaRepository {
        private readonly DbSet<Empresa> _entity;
        public EmpresaRepository(RegistroGeneralContext context) {
            _entity = context.Set<Empresa>();
        }

        public async Task<Empresa> GetById(int id, bool obtenerDesactivados = false) {
            var query = _entity.AsQueryable();
            if(!obtenerDesactivados) {
                query = query.Where(e => e.BorradoLogico == false || e.BorradoLogico == null);
            }
            return query.Where(e => e.Id == id).FirstOrDefault();
        }

        public async Task InsertAsync(Empresa entity, int idCreador, string conn) {
            entity.IdCreador = idCreador;
            entity.FechaCreacion = DateTime.Now;
            entity.Id = NextIdSequence(conn);
            _entity.Add(entity);
        }

        public async Task Update(Empresa entity, int idEditor) {
            entity.IdEditor = idEditor;
            entity.FechaModificacion = DateTime.Now;
            _entity.Update(entity);
        }

        public async Task DeleteById(int id, int idBorrador) {
            var elem = await _entity.FindAsync(id);
            if (elem != null) {
                elem.BorradoLogico = true;
                elem.IdBorrador = idBorrador;
                elem.FechaBorradoLogico = DateTime.Now;
                _entity.Update(elem);
            }
        }

        public IQueryable<Empresa> GetAll(bool obtenerDesactivados = false) {
            var query = _entity.AsQueryable();

            if (!obtenerDesactivados) {
                query = query.Where(e => e.BorradoLogico == null || e.BorradoLogico == false);
            }

            return query;
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

        public PagedList<Empresa> GetAllPaginated(QueryStringParameters parameters) {
            return PagedList<Empresa>.ToPagedList(_entity.AsQueryable(), parameters.PageNumber, parameters.PageSize);
        }
    }
}
