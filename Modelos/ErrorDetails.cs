using System.Text.Json;

namespace backend2.Modelos {
    public class ErrorDetails {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public override string ToString( ) {
            return JsonSerializer.Serialize(this);
        }
    }
}
