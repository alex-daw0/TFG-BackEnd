namespace backend2.DTO {
    public class UsuarioEmpresaDTO(string? Nombre, string? Email, int? UserId, int? IdEmpresa, string? Token, DateTime FechaExpiracion, string GUID_Usuario, string GUID_Empresa) {
        public string? Nombre { get; set; } = Nombre;
        public string? Email { get; set; } = Email;
        public int? UserId { get; set; } = UserId;
        public int? IdEmpresa { get; set; } = IdEmpresa;
        public string? Token { get; set; } = Token;
        public DateTime? FechaExpiracion { get; set; } = FechaExpiracion;
        public string GUID_Usuario { get; set; } = GUID_Usuario;
        public string GUID_Empresa { get; set; } = GUID_Empresa;
    }
}
