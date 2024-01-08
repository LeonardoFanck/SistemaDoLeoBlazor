using SistemaDoLeoBlazor.MODELS.ProximoRegistroDTO;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace SistemaDoLeoBlazor.WEB.Services.ProximoRegistroService
{
    public class ProximoRegistroService : IProximoRegistroService
    {
        private readonly HttpClient _httpClient;
        private ILogger<ProximoRegistroService> _logger;

        public ProximoRegistroService(HttpClient httpClient, ILogger<ProximoRegistroService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<ProximoRegistroDTO> GetProximoRegistro()
        {
            try
            {
                var registroDto = await _httpClient.GetFromJsonAsync<ProximoRegistroDTO>("api/ProximoRegistro");

                return registroDto;
            }
            catch (Exception)
            {
                _logger.LogError("Erro ao acessar api/ProximoRegistro");
                throw;
            }
        }

        public async Task<ProximoRegistroDTO> PatchProximoRegistro(ProximoRegistroDTO proximoRegistroDTO)
        {
            try
            {
                var jsonRequest = JsonSerializer.Serialize(proximoRegistroDTO);

                var  content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");

                var response = await _httpClient.PatchAsync("api/ProximoRegistro", content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ProximoRegistroDTO>();
                }

                return null;
            }
            catch (Exception)
            {
                _logger.LogError("Erro ao acessar api/ProximoRegistro");
                throw;
            }
        }
    }
}
