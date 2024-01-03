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

        public static OperadorTelaDTO OperadorTelaToDto (this OperadorTela operadorTela)
        {
            return new OperadorTelaDTO
            {
                id = operadorTela.id,
                idOperador = operadorTela.operadorId,
                idTela = operadorTela.telaId,
                nome = operadorTela.tela.Nome,
                ativo = operadorTela.ativo,
                novo = operadorTela.novo,
                editar = operadorTela.editar,
                excluir = operadorTela.excluir
            };
        }

        public static IEnumerable<OperadorTelaDTO> OperadorTelasToDto(this IEnumerable<OperadorTela> operadorTelas)
        {
            return (from telas in operadorTelas
                    select new OperadorTelaDTO
                    {
                        id = telas.id,
                        idOperador = telas.operadorId,
                        idTela = telas.telaId,
                        nome = telas.tela.Nome,
                        ativo = telas.ativo,
                        editar = telas.editar,
                        excluir = telas.excluir,
                        novo = telas.novo
                    }).ToList();
        }
    }
}
