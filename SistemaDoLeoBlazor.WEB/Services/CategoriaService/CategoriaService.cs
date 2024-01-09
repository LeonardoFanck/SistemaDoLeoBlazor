using SistemaDoLeoBlazor.MODELS.CategoriaDTO;
using SistemaDoLeoBlazor.MODELS.OperadorDTOs;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;

namespace SistemaDoLeoBlazor.WEB.Services.CategoriaService
{
    public class CategoriaService : ICategoriaService
    {
        public HttpClient _httpClient;
        public ILogger<CategoriaService> _logger;

        public CategoriaService(HttpClient httpClient, ILogger<CategoriaService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<CategoriaDTO> Delete(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Categoria/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CategoriaDTO>();
                }
                return default;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<CategoriaDTO>> GetAll()
        {
            try
            {
                var categorias = await _httpClient.GetFromJsonAsync<IEnumerable<CategoriaDTO>>($"api/Categoria/GetAll");

                return categorias;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao acessar api/Categoria/GetAll");
                throw;
            }
        }

        public async Task<CategoriaDTO> GetById(int id)
        {
            try
            {
                var categoria = await _httpClient.GetFromJsonAsync<CategoriaDTO>($"api/Categoria/{id}");

                return categoria;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao acessar api/Categoria/{id}");
                throw;
            }
        }

        public async Task<CategoriaDTO> Insert(CategoriaDTO categoriaDTO)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Categoria", categoriaDTO);

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default;
                    }

                    return await response.Content.ReadFromJsonAsync<CategoriaDTO>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"{response.StatusCode} Message - {message}");
                }

            }
            catch (Exception)
            {
                _logger.LogError($"Erro ao acessar api/Categoria");
                throw;
            }
        }

        public async Task<CategoriaDTO> Update(CategoriaDTO categoriaDTO)
        {
            try
            {
                var jsonRequest = JsonSerializer.Serialize(categoriaDTO);

                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");

                var response = await _httpClient.PatchAsync($"api/Categoria/{categoriaDTO.id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CategoriaDTO>();
                }

                return null;

            }
            catch (Exception)
            {
                _logger.LogError($"Erro ao acessar api/Categoria/{categoriaDTO.id}");
                throw;
            }
        }
    }
}
