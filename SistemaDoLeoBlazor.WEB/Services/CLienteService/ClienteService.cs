using SistemaDoLeoBlazor.MODELS.ClienteDTO;
using SistemaDoLeoBlazor.MODELS.ClienteDTO;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;

namespace SistemaDoLeoBlazor.WEB.Services.CLienteService
{
    public class ClienteService : IClienteService
    {
        public HttpClient _httpClient;
        public ILogger<ClienteService> _logger;

        public ClienteService(HttpClient httpClient, ILogger<ClienteService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<ClienteDTO> Delete(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Cliente/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ClienteDTO>();
                }
                return default;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ClienteDTO>> GetAll()
        {
            try
            {
                var clientes = await _httpClient.GetFromJsonAsync<IEnumerable<ClienteDTO>>($"api/Cliente/GetAll");

                return clientes;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao acessar api/Cliente/GetAll");
                throw;
            }
        }

        public async Task<ClienteDTO> GetById(int id)
        {
            try
            {
                var cliente = await _httpClient.GetFromJsonAsync<ClienteDTO>($"api/Cliente/{id}");

                return cliente;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao acessar api/Cliente/{id}");
                throw;
            }
        }

        public async Task<ClienteDTO> Insert(ClienteDTO clienteDTO)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Cliente", clienteDTO);

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default;
                    }

                    return await response.Content.ReadFromJsonAsync<ClienteDTO>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"{response.StatusCode} Message - {message}");
                }

            }
            catch (Exception)
            {
                _logger.LogError($"Erro ao acessar api/Cliente");
                throw;
            }
        }

        public async Task<ClienteDTO> Update(ClienteDTO clienteDTO)
        {
            try
            {
                var jsonRequest = JsonSerializer.Serialize(clienteDTO);

                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");

                var response = await _httpClient.PatchAsync($"api/Cliente/{clienteDTO.id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ClienteDTO>();
                }

                return null;

            }
            catch (Exception)
            {
                _logger.LogError($"Erro ao acessar api/Cliente/{clienteDTO.id}");
                throw;
            }
        }
    }
}
