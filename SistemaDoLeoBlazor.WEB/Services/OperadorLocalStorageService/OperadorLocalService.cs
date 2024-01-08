using Blazored.SessionStorage;
using SistemaDoLeoBlazor.MODELS.OperadorDTOs;
using SistemaDoLeoBlazor.WEB.Services.OperadorService.OperadorService;

namespace SistemaDoLeoBlazor.WEB.Services.OperadorLocalStorageService
{
    public class OperadorLocalService : IOperadorLocalService
    {
        private readonly Blazored.SessionStorage.ISessionStorageService session;
        private readonly IOperadorService operadorService;

        public OperadorLocalService(ISessionStorageService session, IOperadorService operadorService)
        {
            this.session = session;
            this.operadorService = operadorService;
        }

        public async Task<OperadorDTO> GetSessaoOperador()
        {
            try
            {
                return await session.GetItemAsync<OperadorDTO>("OperadorAtual");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<OperadorTelaDTO>> GetSessaoTelas()
        {
            try
            {
                return await session.GetItemAsync<IEnumerable<OperadorTelaDTO>>("OperadorTelas");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task LimparSessao()
        {
            await session.ClearAsync();
        }

        public async Task SetSessao(int id)
        {
            try
            {
                var operador = await operadorService.GetOperadorById(id);
                var telas = await operadorService.GetTelasByOperador(id);

                session.SetItemAsync("OperadorAtual", operador);
                session.SetItemAsync("OperadorTelas", telas);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
