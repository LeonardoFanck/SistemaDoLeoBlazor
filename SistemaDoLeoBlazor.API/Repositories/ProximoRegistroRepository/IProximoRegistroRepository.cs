using SistemaDoLeoBlazor.API.Entities;
using SistemaDoLeoBlazor.MODELS.ProximoRegistroDTO;

namespace SistemaDoLeoBlazor.API.Repositories.ProximoRegistroRepository
{
    public interface IProximoRegistroRepository
    {
        Task<ProximoRegistro> GetRegistro();
        Task<ProximoRegistro> PatchRegistro(ProximoRegistroDTO proximoRegistroDTO);
    }
}
