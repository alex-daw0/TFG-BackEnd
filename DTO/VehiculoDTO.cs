using backend2.Modelos;

namespace backend2.DTO {
    public class VehiculoDTO {

        public int Id { get; set; }
        public string Matricula { get; set; }
        public int? IdMarca { get; set; }
        public string? Marca { get; set; }
        public int? IdModelo { get; set; }
        public string? Modelo { get; set; }
        public int? Id_TipoCombustible { get; set; }
        public string? Combustible { get; set; }
        public int IdEmpresa { get; set; }

        public VehiculoDTO(int id, string matricula, int? idMarca, string? marca, int? idModelo, string? modelo, int? id_TipoCombustible, string? combustible, int idEmpresa) {
            Id = id;
            Matricula = matricula;
            IdMarca = idMarca;
            Marca = marca;
            IdModelo = idModelo;
            Modelo = modelo;
            Id_TipoCombustible = id_TipoCombustible;
            Combustible = combustible;
            IdEmpresa = idEmpresa;
        }

        public VehiculoDTO(int id, string matricula, int? idMarca, int? idModelo, int? id_TipoCombustible, int idEmpresa) {
            Id = id;
            Matricula = matricula;
            IdMarca = idMarca;
            IdModelo = idModelo;
            Id_TipoCombustible = id_TipoCombustible;
            IdEmpresa = idEmpresa;
        }

        public VehiculoDTO( ) {
        }

        public override bool Equals(object? obj) {
            return obj is VehiculoDTO dTO &&
                   Matricula == dTO.Matricula &&
                   IdMarca == dTO.IdMarca &&
                   IdModelo == dTO.IdModelo &&
                   Id_TipoCombustible == dTO.Id_TipoCombustible;
        }

        public bool Equals(Vehiculo veh) {
            return Matricula == veh.Matricula &&
                   IdMarca == veh.MarcaId &&
                   IdModelo == veh.ModeloId &&
                   Id_TipoCombustible == veh.TipoCombustibleId;
        }
    }
}
