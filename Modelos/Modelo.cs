namespace backend2.Modelos;

public partial class Modelo {
    public string GuidRegistro { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public int Id { get; set; }

    public int? MarcaId { get; set; }
    public virtual Marca? Marca { get; set; }

    //public int IdEmpresa { get; set; } lo cambiamos por EmpresaId para incluirlo en las peticiones
    public int EmpresaId { get; set; }
    //Añadimos el public virtual empresa nullable, de tal forma que tan solo se enviarán las empresas cuando las pidamos
    public virtual Empresa? Empresa { get; set; }


    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public DateTime? FechaBorradoLogico { get; set; }

    public bool? BorradoLogico { get; set; }

    public int? IdCreador { get; set; }

    public int? IdEditor { get; set; }

    public int? IdBorrador { get; set; }
}
