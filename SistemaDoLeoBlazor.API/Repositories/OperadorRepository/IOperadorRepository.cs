﻿using SistemaDoLeoBlazor.API.Entities;
using SistemaDoLeoBlazor.MODELS.OperadorDTOs;

namespace SistemaDoLeoBlazor.API.Repositories.OperadorRepository
{
    public interface IOperadorRepository
    {
        Task<Operador> GetOperadorById(int Id);

        Task<IEnumerable<Operador>> GetOperadores();

        Task<IEnumerable<OperadorTela>> GetTelas(int Id);

        Task<Operador> PostOperador(OperadorDTO operadorDto);
        
        Task PostOperadorTela(OperadorDTO operadorDto);

        Task<Operador> DeleteOperador(int id);

        Task<Operador> PatchOperador(int id, OperadorDTO operadorDto);

        Task<OperadorTela> PatchOperadorTela(int id, OperadorTelaDTO operadorTelasDto);
    }
}
