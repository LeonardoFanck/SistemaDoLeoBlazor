using SistemaDoLeoBlazor.API.Entities;
using SistemaDoLeoBlazor.MODELS.OperadorDTOs;

namespace SistemaDoLeoBlazor.API.Mapping
{
    public static class MappingOperador
    {
        public static OperadorDTO OperadorToDto(this Operador operador)
        {
            return new OperadorDTO
            {
                id = operador.id,
                nome = operador.nome,
                senha = operador.senha,
                admin = operador.admin,
                inativo = operador.inativo
            };
        }

        public static IEnumerable<OperadorDTO> OperadoresToDto(this IEnumerable<Operador> operadores)
        {
            return (from operador in operadores
                    select new OperadorDTO
                    {
                        id = operador.id,
                        nome = operador.nome,
                        senha = operador.senha,
                        admin = operador.admin,
                        inativo = operador.inativo
                    }).ToList();
        }

        public static IEnumerable<OperadorTelaDTO> OperadorTelaToDto(this IEnumerable<OperadorTela> operadorTelas)
        {
            return (from telas in operadorTelas
                    select new OperadorTelaDTO
                    {
                        id = telas.id,
                        idOperador = telas.operadorId,
                        idPermissao = telas.operadorPermissoesTela.id,
                        nome = telas.nome,
                        editar = telas.operadorPermissoesTela.editar,
                        excluir = telas.operadorPermissoesTela.excluir,
                        novo = telas.operadorPermissoesTela.novo
                    }).ToList();
        }
    }
}
