using backend2.Modelos;
using Microsoft.EntityFrameworkCore;

namespace backend2.Contexto;

public partial class RegistroGeneralContext : DbContext {
    public RegistroGeneralContext( ) {
    }

    public RegistroGeneralContext(string cadenaConexion) {
        _cadenaConexion = cadenaConexion;
    }

    public RegistroGeneralContext(DbContextOptions<RegistroGeneralContext> options)
        : base(options) {
    }

    public virtual DbSet<Empresa> Empresas { get; set; }

    public virtual DbSet<EmpresasActiva> EmpresasActivas { get; set; }

    public virtual DbSet<EmpresasUsuario> EmpresasUsuarios { get; set; }

    public virtual DbSet<Marca> Marcas { get; set; }

    public virtual DbSet<Modelo> Modelos { get; set; }

    public virtual DbSet<TiposCombustible> TiposCombustibles { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Vehiculo> Vehiculos { get; set; }

    private DbContextOptionsBuilder _optionBuilder;

    private string? _cadenaConexion = string.Empty;


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {


        if (!optionsBuilder.IsConfigured) {
            optionsBuilder.UseSqlServer(_cadenaConexion);
        }

        _optionBuilder = optionsBuilder;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Empresa>(entity => {
            entity.HasIndex(e => e.GuidRegistro, "UK_Empresas").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.BaseActiva).HasMaxLength(100);
            entity.Property(e => e.FechaBorradoLogico).HasColumnType("datetime");
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.GuidRegistro)
                .HasMaxLength(50)
                .HasColumnName("GUID_Registro");
            entity.Property(e => e.IdBorrador).HasColumnName("Id_Borrador");
            entity.Property(e => e.IdCreador).HasColumnName("Id_Creador");
            entity.Property(e => e.IdEditor).HasColumnName("Id_Editor");
            entity.Property(e => e.Nombre).HasMaxLength(200);
        });

        modelBuilder.Entity<EmpresasActiva>(entity => {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.FechaBorradoLogico).HasColumnType("datetime");
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.GuidRegistro)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("GUID_Registro");
            entity.Property(e => e.IdBorrador).HasColumnName("Id_Borrador");
            entity.Property(e => e.IdCreador).HasColumnName("Id_Creador");
            entity.Property(e => e.IdEditor).HasColumnName("Id_Editor");
            entity.Property(e => e.IdEmpresa).HasColumnName("Id_Empresa");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");
        });

        modelBuilder.Entity<EmpresasUsuario>(entity => {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.FechaBorradoLogico).HasColumnType("datetime");
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.GuidRegistro)
                .HasMaxLength(50)
                .HasColumnName("GUID_Registro");
            entity.Property(e => e.IdBorrador).HasColumnName("Id_Borrador");
            entity.Property(e => e.IdCreador).HasColumnName("Id_Creador");
            entity.Property(e => e.IdEditor).HasColumnName("Id_Editor");
            entity.Property(e => e.IdEmpresa).HasColumnName("Id_Empresa");
            entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");
        });

        modelBuilder.Entity<Marca>(entity => {
            entity.HasIndex(e => e.GuidRegistro, "UK_Marcas").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.FechaBorradoLogico).HasColumnType("datetime");
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.GuidRegistro)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("GUID_Registro");
            entity.Property(e => e.IdBorrador).HasColumnName("Id_Borrador");
            entity.Property(e => e.IdCreador).HasColumnName("Id_Creador");
            entity.Property(e => e.IdEditor).HasColumnName("Id_Editor");
            entity.Property(e => e.EmpresaId).HasColumnName("Id_Empresa");
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<Modelo>(entity => {
            entity.HasIndex(e => e.GuidRegistro, "UK_Modelos").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.FechaBorradoLogico).HasColumnType("datetime");
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.GuidRegistro)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("GUID_Registro");
            entity.Property(e => e.IdBorrador).HasColumnName("Id_Borrador");
            entity.Property(e => e.IdCreador).HasColumnName("Id_Creador");
            entity.Property(e => e.IdEditor).HasColumnName("Id_Editor");
            entity.Property(e => e.EmpresaId).HasColumnName("Id_Empresa");
            entity.Property(e => e.MarcaId).HasColumnName("Id_Marca");
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<TiposCombustible>(entity => {
            entity.ToTable("TiposCombustible");

            entity.HasIndex(e => e.GuidRegistro, "UK_TiposCombustible").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.FechaBorradoLogico).HasColumnType("datetime");
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.GuidRegistro)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("GUID_Registro");
            entity.Property(e => e.IdBorrador).HasColumnName("Id_Borrador");
            entity.Property(e => e.IdCreador).HasColumnName("Id_Creador");
            entity.Property(e => e.IdEditor).HasColumnName("Id_Editor");
            entity.Property(e => e.IdEmpresa).HasColumnName("Id_Empresa");
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Usuario>(entity => {
            entity.HasIndex(e => e.GuidRegistro, "UK_Usuarios").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Contraseña).HasMaxLength(50);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("EMail");
            entity.Property(e => e.FechaBorradoLogico).HasColumnType("datetime");
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.GuidRegistro)
                .HasMaxLength(50)
                .HasColumnName("GUID_Registro");
            entity.Property(e => e.IdBorrador).HasColumnName("Id_Borrador");
            entity.Property(e => e.IdCreador).HasColumnName("Id_Creador");
            entity.Property(e => e.IdEditor).HasColumnName("Id_Editor");
            entity.Property(e => e.IdEmpresa).HasColumnName("Id_Empresa");
            entity.Property(e => e.Nombre).HasMaxLength(60);
        });

        modelBuilder.Entity<Vehiculo>(entity => {
            entity.HasIndex(e => e.GuidRegistro, "UK_Vehiculos").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.FechaBorradoLogico).HasColumnType("datetime");
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.GuidRegistro)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("GUID_Registro");
            entity.Property(e => e.IdBorrador).HasColumnName("Id_Borrador");
            entity.Property(e => e.IdCreador).HasColumnName("Id_Creador");
            entity.Property(e => e.IdEditor).HasColumnName("Id_Editor");
            entity.Property(e => e.IdEmpresa).HasColumnName("Id_Empresa");
            entity.Property(e => e.MarcaId).HasColumnName("Id_Marca");
            entity.Property(e => e.ModeloId).HasColumnName("Id_Modelo");
            entity.Property(e => e.TipoCombustibleId).HasColumnName("Id_TipoCombustible");
            entity.Property(e => e.Matricula)
                .HasMaxLength(50)
                .IsUnicode(false);
        });
        modelBuilder.HasSequence<int>("SECUENCIAIDENTIFICADORES").StartsAt(157356L);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
