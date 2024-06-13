namespace backend2.Modelos;

public partial class Vehiculo {
    public string GuidRegistro { get; set; } = null!;

    public string Matricula { get; set; } = null!;

    public int Id { get; set; }

    public int? ModeloId { get; set; }
    public int? MarcaId { get; set; }
    public int? TipoCombustibleId { get; set; }


    public virtual Modelo? Modelo { get; set; }
    public virtual Marca? Marca { get; set; }
    public virtual TiposCombustible? TipoCombustible { get; set; }

    public int IdEmpresa { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public DateTime? FechaBorradoLogico { get; set; }

    public bool? BorradoLogico { get; set; }

    public int? IdCreador { get; set; }

    public int? IdEditor { get; set; }

    public int? IdBorrador { get; set; }

}
