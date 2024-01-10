using SistemaDoLeoBlazor.MODELS.ProdutoDTO;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;

namespace SistemaDoLeoBlazor.WEB.Services.ProdutoService
{
    public class ProdutoService : IProdutoService
    {
        public HttpClient _httpClient;
        public ILogger<ProdutoService> _logger;

        public ProdutoService(HttpClient httpClient, ILogger<ProdutoService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<ProdutoDTO> Delete(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Produto/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ProdutoDTO>();
                }
                return default;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ProdutoDTO>> GetAll()
        {
            try
            {
                var registros = await _httpClient.GetFromJsonAsync<IEnumerable<ProdutoDTO>>($"api/Produto/GetAll");

                return registros;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao acessar api/Produto/GetAll");
                throw;
            }
        }

        public async Task<ProdutoDTO> GetById(int id)
        {
            try
            {
                var registro = await _httpClient.GetFromJsonAsync<ProdutoDTO>($"api/Produto/{id}");

                return registro;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao acessar api/Produto/{id}");
                throw;
            }
        }

        public async Task<ProdutoDTO> Insert(ProdutoDTO produtoDTO)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Produto", produtoDTO);

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default;
                    }

                    return await response.Content.ReadFromJsonAsync<ProdutoDTO>();
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

        public async Task<ProdutoDTO> Update(ProdutoDTO produtoDTO)
        {
            try
            {
                var jsonRequest = JsonSerializer.Serialize(produtoDTO);

                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");

                var response = await _httpClient.PatchAsync($"api/Produto/{produtoDTO.id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ProdutoDTO>();
                }

                return null;

            }
            catch (Exception)
            {
                _logger.LogError($"Erro ao acessar api/Produto/{produtoDTO.id}");
                throw;
            }
        }
    }
}
