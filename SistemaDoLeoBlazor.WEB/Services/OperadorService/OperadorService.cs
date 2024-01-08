using SistemaDoLeoBlazor.MODELS.OperadorDTOs;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Unicode;

namespace SistemaDoLeoBlazor.WEB.Services.OperadorService.OperadorService
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

        public async Task<OperadorDTO> PostOperador(OperadorDTO operadorDTO)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Operador/Operador", operadorDTO);

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default;
                    }

                    return await response.Content.ReadFromJsonAsync<OperadorDTO>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"{response.StatusCode} Message -{message}");
                }

            }
            catch (Exception)
            {
                _logger.LogError($"Erro ao acessar api/Operador/Operador");
                throw;
            }
        }

        public async Task<OperadorDTO> PostOperadorTelas(OperadorDTO operadorDTO)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Operador/Telas", operadorDTO);

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default;
                    }

                    return await response.Content.ReadFromJsonAsync<OperadorDTO>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"{response.StatusCode} Message -{message}");
                }

            }
            catch (Exception)
            {
                _logger.LogError($"Erro ao acessar api/Operador/Telas");
                throw;
            }
        }

        public async Task<OperadorDTO> PatchOperador(OperadorDTO operadorDTO)
        {
            try
            {
                var jsonRequest = JsonSerializer.Serialize(operadorDTO);

                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");

                var response = await _httpClient.PatchAsync($"api/Operador/{operadorDTO.id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<OperadorDTO>();
                }

                return null;

            }
            catch (Exception)
            {
                _logger.LogError($"Erro ao acessar api/Operador/{operadorDTO.id}");
                throw;
            }
        }

        public async Task<OperadorTelaDTO> PatchOperadorTelas(OperadorTelaDTO operadorTelaDTO)
        {
            try
            {
                var jsonRequest = JsonSerializer.Serialize(operadorTelaDTO);

                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");

                var response = await _httpClient.PatchAsync($"api/Operador/Tela/{operadorTelaDTO.id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<OperadorTelaDTO>();
                }

                return null;

            }
            catch (Exception)
            {
                _logger.LogError($"Erro ao acessar api/Operador/Tela/{operadorTelaDTO.id}");
                throw;
            }
        }

        public async Task<OperadorDTO> DeleteOperador(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Operador/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<OperadorDTO>();
                }
                return default;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<OperadorDTO>> GetAllOperadores()
        {
            try
            {
                var listaOperadorDto = await _httpClient.GetFromJsonAsync<IEnumerable<OperadorDTO>>($"api/Operador/GetAll");

                return listaOperadorDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao acessar api/Operador/GetAll");
                throw;
            }
        }
    }
}
