using backend2.Modelos;

namespace backend2.DTO {
    public class CombustibleDTO {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public int idEmpresa { get; set; }

        public override bool Equals(object? obj) {
            return obj is CombustibleDTO dTO &&
                   Nombre == dTO.Nombre;
        }

        public bool Equals(TiposCombustible combustible) {
            return Nombre == combustible.Nombre;
        }
    }
}
