using AutoMapper;
using backend2.DTO;
using backend2.Modelos;

namespace backend2.Mapppings {

    #region mapeoVehiculos
    public class MapeoVehiculos_VehiculosDtoTypeConverter : ITypeConverter<Vehiculo, VehiculoDTO> {
        public VehiculoDTO Convert(Vehiculo source, VehiculoDTO destination, ResolutionContext context) {
            destination = new();

            if (source is not null) {
                destination.Id = source.Id;
                destination.Matricula = source.Matricula;
                if (source.Marca is not null) {
                    destination.IdMarca = source.Marca.Id;
                    destination.Marca = source.Marca.Nombre;
                }

                if (source.Modelo is not null) {
                    destination.IdModelo = source.Modelo.Id;
                    destination.Modelo = source.Modelo.Nombre;
                }

                if (source.TipoCombustible is not null) {
                    destination.Id_TipoCombustible = source.TipoCombustible.Id;
                    destination.Combustible = source.TipoCombustible.Nombre;
                }

                destination.IdEmpresa = source.IdEmpresa;
            }

            return destination;
        }
    }

    public class MapeoVehiculosDTO_VehiculosTypeConverter : ITypeConverter<VehiculoDTO, Vehiculo> {
        public Vehiculo Convert(VehiculoDTO source, Vehiculo destination, ResolutionContext context) {
            destination = new();

            if (source is not null) {
                destination.Id = source.Id;
                destination.Matricula = source.Matricula;
                if (source.IdMarca is not null) {
                    destination.MarcaId = source.IdMarca;
                }

                if (source.IdModelo is not null) {
                    destination.ModeloId = source.IdModelo;
                }

                if (source.Id_TipoCombustible is not null) {
                    destination.TipoCombustibleId = source.Id_TipoCombustible;
                }

                destination.IdEmpresa = source.IdEmpresa;

            }

            return destination;
        }
    }

    #endregion
    #region MapeoMarcas
    public class MapeoMarcas_MarcasDTOTypeConverter : ITypeConverter<Marca, MarcaDTO> {
        public MarcaDTO Convert(Marca source, MarcaDTO destination, ResolutionContext context) {
            destination = new();

            if (source is not null) {
                destination.Id = source.Id;
                destination.IdEmpresa = source.EmpresaId;

                if (source.Empresa is not null) {
                    destination.Empresa = source.Empresa.Nombre;
                }

                if (source.Nombre is not null) {
                    destination.Nombre = source.Nombre;
                }
            }
            return destination;
        }
    }

    public class MapeoMarcasDTO_MarcasTypeConverter : ITypeConverter<MarcaDTO, Marca> {
        public Marca Convert(MarcaDTO source, Marca destination, ResolutionContext context) {
            destination = new();

            if (source is not null) {
                destination.Id = source.Id;

                destination.EmpresaId = source.IdEmpresa;

                if (source.Nombre is not null) {
                    destination.Nombre = source.Nombre;
                }
            }
            return destination;
        }
    }

    #endregion
    #region mapeoModelos
    public class MapeoModelos_ModelosDTOTypeConverter : ITypeConverter<Modelo, ModeloDTO> {
        public ModeloDTO Convert(Modelo source, ModeloDTO destination, ResolutionContext context) {
            destination = new();

            if (source is not null) {
                destination.Id = source.Id;
                destination.IdEmpresa = source.EmpresaId;

                if (source.MarcaId is not null) {
                    destination.IdMarca = (int)source.MarcaId;
                }

                if (source.Marca is not null) {
                    destination.Marca = source.Marca.Nombre;
                }

                if (source.Empresa is not null) {
                    destination.Empresa = source.Empresa.Nombre;
                }

                if (source.Nombre is not null) {
                    destination.Nombre = source.Nombre;
                }
            }
            return destination;
        }
    }

    public class MapeoModelosDTO_ModelosTypeConverter : ITypeConverter<ModeloDTO, Modelo> {
        public Modelo Convert(ModeloDTO source, Modelo destination, ResolutionContext context) {
            destination = new();

            if (source is not null) {
                destination.Id = source.Id;
                destination.EmpresaId = source.IdEmpresa;
                destination.MarcaId = (int)source.IdMarca;

                if (source.Nombre is not null) {
                    destination.Nombre = source.Nombre;
                }
            }
            return destination;
        }
    }
    #endregion

    #region mapeoCombustibles
    public class MapeoCombustibles_CombustiblesDTOTypeConverter : ITypeConverter<TiposCombustible, CombustibleDTO> {
        public CombustibleDTO Convert(TiposCombustible source, CombustibleDTO destination, ResolutionContext context) {
            destination = new();

            if (source is not null) {
                destination.Id = source.Id;
                destination.idEmpresa = source.IdEmpresa;

                if (source.Nombre is not null) {
                    destination.Nombre = source.Nombre;
                }
            }
            return destination;
        }
    }

    public class MapeoCombustiblesDTO_CombustiblesTypeConverter : ITypeConverter<CombustibleDTO, TiposCombustible> {
        public TiposCombustible Convert(CombustibleDTO source, TiposCombustible destination, ResolutionContext context) {
            destination = new();

            if (source is not null) {
                destination.Id = source.Id;
                destination.IdEmpresa = source.idEmpresa;

                if (source.Nombre is not null) {
                    destination.Nombre = source.Nombre;
                }
            }
            return destination;
        }
    }
    #endregion

}