using SistemaDoLeoBlazor.MODELS.FormaPgtoDTO;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;

namespace SistemaDoLeoBlazor.WEB.Services.FormaPgtoService
{
    public class FormaPgtoService : IFormaPgtoService
    {
        public HttpClient _httpClient;
        public ILogger<FormaPgtoService> _logger;

        public FormaPgtoService(HttpClient httpClient, ILogger<FormaPgtoService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<FormaPgtoDTO> Delete(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/FormaPgto/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<FormaPgtoDTO>();
                }
                return default;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<FormaPgtoDTO>> GetAll()
        {
            try
            {
                var registros = await _httpClient.GetFromJsonAsync<IEnumerable<FormaPgtoDTO>>($"api/FormaPgto/GetAll");

                return registros;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao acessar api/FormaPgto/GetAll");
                throw;
            }
        }

        public async Task<FormaPgtoDTO> GetById(int id)
        {
            try
            {
                var registro = await _httpClient.GetFromJsonAsync<FormaPgtoDTO>($"api/FormaPgto/{id}");

                return registro;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao acessar api/FormaPgto/{id}");
                throw;
            }
        }

        public async Task<FormaPgtoDTO> Insert(FormaPgtoDTO formaPgtoDTO)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/FormaPgto", formaPgtoDTO);

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default;
                    }

                    return await response.Content.ReadFromJsonAsync<FormaPgtoDTO>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"{response.StatusCode} Message - {message}");
                }

            }
            catch (Exception)
            {
                _logger.LogError($"Erro ao acessar api/FormaPgto");
                throw;
            }
        }

        public async Task<FormaPgtoDTO> Update(FormaPgtoDTO formaPgtoDTO)
        {
            try
            {
                var jsonRequest = JsonSerializer.Serialize(formaPgtoDTO);

                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");

                var response = await _httpClient.PatchAsync($"api/FormaPgto/{formaPgtoDTO.id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<FormaPgtoDTO>();
                }

                return null;

            }
            catch (Exception)
            {
                _logger.LogError($"Erro ao acessar api/FormaPgto/{formaPgtoDTO.id}");
                throw;
            }
        }
    }
}
