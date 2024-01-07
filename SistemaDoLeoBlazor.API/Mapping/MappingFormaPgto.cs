using SistemaDoLeoBlazor.API.Entities;
using SistemaDoLeoBlazor.MODELS.FormaPgtoDTO;

namespace SistemaDoLeoBlazor.API.Mapping
{
    public static class MappingFormaPgto
    {
        public static FormaPgtoDTO FormaPgtoToDto(this FormaPgto formaPgto)
        {
            return new FormaPgtoDTO
            {
                id = formaPgto.id,
                nome = formaPgto.nome,
                inativo = formaPgto.inativo
            };
        }

        public static IEnumerable<FormaPgtoDTO> FormasPgtoToDto(this IEnumerable<FormaPgto> formasPgto)
        {
            return (from formaPgto in formasPgto
                    select new FormaPgtoDTO
                    {
                        id = formaPgto.id,
                        nome = formaPgto.nome,
                        inativo = formaPgto.inativo
                    }).ToList();
        }
    }
}
