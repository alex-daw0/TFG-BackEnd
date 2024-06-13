namespace backend2.Utilidad {
    public class VehiculoParams : QueryStringParameters {

        public VehiculoParams( ) {
            OrderBy = "Matricula";
        }

        //Filter & Serach
        public string? Matricula { get; set; }
        public int? Marca { get; set; }
        public int? Modelo { get; set; }
        public int? Combustible { get; set; }


    }
}
