using SistemaDoLeoBlazor.MODELS.OperadorDTOs;
using System.Net.Http;
using System.Net.Http.Json;

namespace SistemaDoLeoBlazor.WEB.Services
{
    public class OperadorService : IOperadorService
    {
        public HttpClient _httpClient;
        public ILogger<OperadorService> _logger;

        public OperadorService(HttpClient httpClient, ILogger<OperadorService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<OperadorDTO> GetOperadorById(int id)
        {
            try
            {
                var operadorDto = await _httpClient.GetFromJsonAsync<OperadorDTO>($"api/Operador/{id}");

                return operadorDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao acessar api/Operador/{id}");
                throw;
            }
        }

        public async Task<IEnumerable<OperadorTelaDTO>> GetTelasByOperador(int id)
        {
            try
            {
                var operadorTelaDto = await _httpClient.GetFromJsonAsync<IEnumerable<OperadorTelaDTO>>($"api/Operador/{id}/getTelas");

                return operadorTelaDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao acessar api/Operador/{id}/GetTelas");
                throw;
            }
        }
    }
}
