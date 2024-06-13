using AutoMapper;
using backend2.DTO;
using backend2.Modelos;

namespace backend2.Mapppings {
    public class AutoMapperProfile : Profile {

        public AutoMapperProfile( ) {
            #region MapeoVehiculos
            CreateMap<Vehiculo, VehiculoDTO>().ConvertUsing<MapeoVehiculos_VehiculosDtoTypeConverter>();
            //Mapeo inverso
            CreateMap<VehiculoDTO, Vehiculo>().ConvertUsing<MapeoVehiculosDTO_VehiculosTypeConverter>();
            #endregion

            #region MapeoMarcas
            CreateMap<Marca, MarcaDTO>().ConvertUsing<MapeoMarcas_MarcasDTOTypeConverter>();
            CreateMap<MarcaDTO, Marca>().ConvertUsing<MapeoMarcasDTO_MarcasTypeConverter>();
            #endregion

            #region MapeoModelos
            CreateMap<Modelo, ModeloDTO>().ConvertUsing<MapeoModelos_ModelosDTOTypeConverter>();
            CreateMap<ModeloDTO, Modelo>().ConvertUsing<MapeoModelosDTO_ModelosTypeConverter>();
            #endregion

            #region MapeoEmpresas
            CreateMap<Empresa, EmpresaDTO>().ReverseMap();
            #endregion

            #region MapeoCombustibles
            CreateMap<TiposCombustible, CombustibleDTO>().ConvertUsing<MapeoCombustibles_CombustiblesDTOTypeConverter>();
            CreateMap<CombustibleDTO, TiposCombustible>().ConvertUsing<MapeoCombustiblesDTO_CombustiblesTypeConverter>();

            #endregion

        }
    }
}
