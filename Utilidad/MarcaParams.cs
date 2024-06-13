namespace backend2.Utilidad {
    public class MarcaParams : QueryStringParameters {
        public MarcaParams( ) {
            OrderBy = "Nombre";
        }

        //Filter & Serach
        public string? Nombre { get; set; }

    }
}
