using SistemaDoLeoBlazor.API.Entities;
using SistemaDoLeoBlazor.MODELS.CategoriaDTO;

namespace SistemaDoLeoBlazor.API.Mapping
{
    public static class MappingCategoria
    {
        public static CategoriaDTO CategoriaToDto(this Categoria categoria)
        {
            return new CategoriaDTO{
                id = categoria.id,
                nome = categoria.nome,
                inativo = categoria.inativo
            };
        }

        public static IEnumerable<CategoriaDTO> CategoriasToDto(this IEnumerable<Categoria> categorias)
        {
            return (from categoria in categorias
                    select new CategoriaDTO
                    {
                        id = categoria.id,
                        nome = categoria.nome,
                        inativo = categoria.inativo
                    }).ToList();
        }
    }
}
