using AutoMapper;
using BoerisCreaciones.Core.Models;
using BoerisCreaciones.Core.Models.Compras;
using BoerisCreaciones.Core.Models.Proveedores;
using BoerisCreaciones.Core.Models.Rubros;
using BoerisCreaciones.Core.Models.Sucursales;

namespace BoerisCreaciones.Core.Profiles
{
    public class CompraVMtoDTOProfile : Profile
    {
        public CompraVMtoDTOProfile()
        {
            CreateMap<CompraVM, CompraDTO>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.id_compra))
                .ForMember(dest => dest.requester_partner, opt => opt.MapFrom((src, dest, _, context) => context.Mapper.Map<UsuarioDTO>(src.socio_que_hizo_pedido)))
                .ForMember(dest => dest.provider, opt => opt.MapFrom((src, dest, _, context) => context.Mapper.Map<ProveedorDTO>(src.proveedor)))
                .ForMember(dest => dest.description, opt => opt.MapFrom(src => src.descripcion))
                .ForMember(dest => dest.order_date, opt => opt.MapFrom(src => src.fecha_pedido))
                .ForMember(dest => dest.reception_date, opt => opt.MapFrom(src => src.fecha_recepcion))
                .ForMember(dest => dest.canceled_date, opt => opt.MapFrom(src => src.fecha_cancelado))
                .ForMember(dest => dest.currency, opt => opt.MapFrom(src => src.moneda))
                .ForMember(dest => dest.payment_type, opt => opt.MapFrom(src => src.tipo_pago))
                .ForMember(dest => dest.reception_mode, opt => opt.MapFrom(src => src.modo_recepcion))
                .ForMember(dest => dest.status, opt => opt.MapFrom(src => src.estado))
                .ForMember(dest => dest.reception_branch, opt => opt.MapFrom((src, dest, _, context) => context.Mapper.Map<SucursalDTO>(src.sucursal)))
                .ForMember(dest => dest.price, opt => opt.MapFrom(src => src.precio))
                .ForMember(dest => dest.invoice, opt => opt.MapFrom(src => src.factura))
            ;
        }
    }

    public class MateriaPrimaCompraVMtoDTOProfile : Profile
    {
        public MateriaPrimaCompraVMtoDTOProfile()
        {
            CreateMap<MateriaPrimaCompraVM, MateriaPrimaCompraDTO>()
                .ForMember(dest => dest.raw_material_id, opt => opt.MapFrom(src => src.id_matP))
                .ForMember(dest => dest.category, opt => opt.MapFrom(src => new RubroMateriaPrimaDTO(src.id_rubroMP, src.rubro)))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.nombre))
                .ForMember(dest => dest.quantity, opt => opt.MapFrom(src => src.cantidad))
                .ForMember(dest => dest.unit_price, opt => opt.MapFrom(src => src.precio_unitario))
            ;
        }
    }
}
