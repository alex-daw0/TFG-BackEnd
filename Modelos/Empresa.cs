namespace backend2.Modelos;

public partial class Empresa {
    public string? Nombre { get; set; }

    public string? BaseActiva { get; set; }

    public string GuidRegistro { get; set; } = null!;

    public int Id { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public DateTime? FechaBorradoLogico { get; set; }

    public bool? BorradoLogico { get; set; }

    public int? IdCreador { get; set; }

    public int? IdEditor { get; set; }

    public int? IdBorrador { get; set; }
}
