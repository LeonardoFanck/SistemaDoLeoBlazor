using SistemaDoLeoBlazor.MODELS.CategoriaDTO;
using SistemaDoLeoBlazor.MODELS.PedidoDTO;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;

namespace SistemaDoLeoBlazor.WEB.Services.PedidoService
{
    public class PedidoService : IPedidoService
    {
        public HttpClient _httpClient;
        public ILogger<PedidoService> _logger;

        public PedidoService(HttpClient httpClient, ILogger<PedidoService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<PedidoDTO> Delete(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Pedido/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<PedidoDTO>();
                }
                return default;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PedidoItemDTO> DeleteItem(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Pedido/Item/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<PedidoItemDTO>();
                }
                return default;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<PedidoDTO>> GetAll()
        {
            try
            {
                var registro = await _httpClient.GetFromJsonAsync<IEnumerable<PedidoDTO>>($"api/Pedido/GetAll");

                return registro;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao acessar api/Pedido/GetAll");
                throw;
            }
        }

        public async Task<IEnumerable<PedidoItemDTO>> GetAllItens(int id)
        {
            try
            {
                var registros = await _httpClient.GetFromJsonAsync<IEnumerable<PedidoItemDTO>>($"api/Pedido/{id}/GetAllItem");

                return registros;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao acessar api/Pedido/{id}/GetAllItem");
                throw;
            }
        }

        public async Task<PedidoDTO> GetById(int id)
        {
            try
            {
                var registro = await _httpClient.GetFromJsonAsync<PedidoDTO>($"api/Pedido/{id}");

                return registro;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao acessar api/Pedido/{id}");
                throw;
            }
        }

        public async Task<PedidoItemDTO> GetItemById(int id)
        {
            try
            {
                var registro = await _httpClient.GetFromJsonAsync<PedidoItemDTO>($"api/Pedido/Item/{id}");

                return registro;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao acessar api/Pedido/Item/{id}");
                throw;
            }
        }

        public async Task<PedidoDTO> Insert(PedidoDTO pedidoDTO)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Pedido", pedidoDTO);

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default;
                    }

                    return await response.Content.ReadFromJsonAsync<PedidoDTO>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"{response.StatusCode} Message - {message}");
                }

            }
            catch (Exception)
            {
                _logger.LogError($"Erro ao acessar api/Pedido");
                throw;
            }
        }

        public async Task<PedidoItemDTO> InsertItem(PedidoItemDTO pedidoItemDTO)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Pedido/Item", pedidoItemDTO);

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default;
                    }

                    return await response.Content.ReadFromJsonAsync<PedidoItemDTO>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"{response.StatusCode} Message - {message}");
                }

            }
            catch (Exception)
            {
                _logger.LogError($"Erro ao acessar api/Pedido/Item");
                throw;
            }
        }

        public async Task<PedidoDTO> Update(PedidoDTO pedidoDTO)
        {
            try
            {
                var jsonRequest = JsonSerializer.Serialize(pedidoDTO);

                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");

                var response = await _httpClient.PatchAsync($"api/Pedido/{pedidoDTO.id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<PedidoDTO>();
                }

                return null;

            }
            catch (Exception)
            {
                _logger.LogError($"Erro ao acessar api/Pedido/{pedidoDTO.id}");
                throw;
            }
        }

        public async Task<PedidoItemDTO> UpdateItem(PedidoItemDTO pedidoItemDTO)
        {
            try
            {
                var jsonRequest = JsonSerializer.Serialize(pedidoItemDTO);

                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");

                var response = await _httpClient.PatchAsync($"api/Pedido/Item/{pedidoItemDTO.id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<PedidoItemDTO>();
                }

                return null;

            }
            catch (Exception)
            {
                _logger.LogError($"Erro ao acessar api/Pedido/Item/{pedidoItemDTO.id}");
                throw;
            }
        }
    }
}
