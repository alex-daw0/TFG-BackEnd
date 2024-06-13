
using backend2.Modelos;

namespace backend2.DTO {
    public class ModeloDTO {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public int IdEmpresa { get; set; }
        public string? Empresa { get; set; }
        public int IdMarca { get; set; }
        public string? Marca { get; set; }

        public override bool Equals(object? obj) {
            return obj is ModeloDTO dTO &&
                   Nombre == dTO.Nombre &&
                   IdMarca == dTO.IdMarca;
        }

        public bool Equals(Modelo modelo) {
            return Nombre == modelo.Nombre &&
            IdMarca == modelo.MarcaId;
        }


    }


}
