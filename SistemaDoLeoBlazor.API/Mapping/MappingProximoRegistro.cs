using SistemaDoLeoBlazor.API.Entities;
using SistemaDoLeoBlazor.MODELS.ProximoRegistroDTO;

namespace SistemaDoLeoBlazor.API.Mapping
{
    public static class MappingProximoRegistro
    {
        public static ProximoRegistroDTO ProximoRegistroToDto(this ProximoRegistro proximoRegistro)
        {
            return new ProximoRegistroDTO
            {
                id = proximoRegistro.id,
                categoria = proximoRegistro.categoria,
                cliente = proximoRegistro.cliente,
                formaPgto = proximoRegistro.formaPgto,
                operador = proximoRegistro.operador,
                pedido = proximoRegistro.pedido,
                produto = proximoRegistro.produto
            };
        }
    }
}
