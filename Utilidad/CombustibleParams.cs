namespace backend2.Utilidad {
    public class CombustibleParams : QueryStringParameters {
        public CombustibleParams( ) {
            OrderBy = "Nombre";
        }

        //Filter & Serach
        public string? Nombre { get; set; }
    }
}
