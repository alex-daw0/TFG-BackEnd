namespace backend2.Utilidad {
    public class ModeloParams : QueryStringParameters {
        public ModeloParams( ) {
            OrderBy = "Nombre";
        }

        //Filter & Serach
        public string? Nombre { get; set; }
        public int? Marca { get; set; }

    }
}
