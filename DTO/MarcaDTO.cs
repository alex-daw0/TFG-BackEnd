using backend2.Modelos;

namespace backend2.DTO {
    public class MarcaDTO {

        public int Id { get; set; }
        public string? Nombre { get; set; }
        public int IdEmpresa { get; set; }
        public string? Empresa { get; set; }

        public MarcaDTO( ) {
        }

        public MarcaDTO(int id, string? nombre, int idEmpresa, string? empresa) {
            Id = id;
            Nombre = nombre;
            IdEmpresa = idEmpresa;
            Empresa = empresa;
        }

        public override bool Equals(object? obj) {
            return obj is MarcaDTO dTO &&
                   Nombre == dTO.Nombre;
        }

        public bool Equals(Marca marca) {
            return Nombre == marca.Nombre;
        }
    }
}
