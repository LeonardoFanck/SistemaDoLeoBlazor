using SistemaDoLeoBlazor.MODELS.ProximoRegistroDTO;

namespace SistemaDoLeoBlazor.WEB.Services.ProximoRegistroService
{
    public interface IProximoRegistroService
    {
        Task<ProximoRegistroDTO> GetProximoRegistro();
        Task<ProximoRegistroDTO> PatchProximoRegistro(ProximoRegistroDTO proximoRegistroDTO);
    }
}
