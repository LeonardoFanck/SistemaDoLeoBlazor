using Microsoft.AspNetCore.Mvc;
using SistemaDoLeoBlazor.API.Mapping;
using SistemaDoLeoBlazor.API.Repositories.PedidoRepository;
using SistemaDoLeoBlazor.MODELS.PedidoDTO;

namespace SistemaDoLeoBlazor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoRepository _repository;
        private ILogger<PedidoController> _logger;

        public PedidoController(IPedidoRepository repository, ILogger<PedidoController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PedidoDTO>> GetPedido(int id)
        {
            try
            {
                var pedido = await _repository.GetPedidoById(id);

                if (pedido is null)
                {
                    return NotFound("Registro não localizado");
                }
                else
                {
                    var pedidoDTO = pedido.PedidoToDtoGet();
                    return Ok(pedidoDTO);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"## Erro buscar o registro {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<PedidoDTO>>> GetAllPedido()
        {
            try
            {
                var pedido = await _repository.GetAllPedidos();

                if (pedido is null)
                {
                    return NoContent();
                }

                var pedidoDTO = pedido.PedidosToDtoGet();

                return Ok(pedidoDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError("## Erro ao buscar os registros");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<PedidoDTO>> PostPedido([FromBody]
                                        PedidoDTO pedidoDTO)
        {
            try
            {
                var novoPedido = await _repository.PostPedido(pedidoDTO);

                var novoPedidoDTO = novoPedido.PedidoToDtoSet();

                return CreatedAtAction(nameof(GetPedido), new { id = novoPedidoDTO.id }, novoPedidoDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError("## Erro criar uma novo Registro");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<PedidoDTO>> PatchPedido(int id, PedidoDTO pedidoDTO)
        {
            try
            {
                var pedido = await _repository.PatchPedido(id, pedidoDTO);

                if (pedido is null)
                {
                    return NotFound();
                }

                var pedidoDto = pedido.PedidoToDtoSet();

                return Ok(pedidoDto);
            }
            catch (Exception ex)
            {
                _logger.LogError("## Erro ao alterar o registro");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<PedidoDTO>> DeletePedido(int id)
        {
            try
            {
                var pedido = await _repository.DeletePedido(id);

                if (pedido is null)
                {
                    return NotFound("Registro não localizado");
                }

                var pedidoDto = pedido.PedidoToDtoSet();

                return Ok(pedidoDto);
            }
            catch (Exception ex)
            {
                _logger.LogError("## Erro ao excluir o registro");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // ITENS

        [HttpGet]
        [Route("{id:int}/GetAllItem")]
        public async Task<ActionResult<IEnumerable<PedidoItemDTO>>> GetAllItens(int id)
        {
            try
            {
                var item = await _repository.GetAllItens(id);

                if (item is null)
                {
                    return NoContent();
                }

                var itemDto = item.PedidoItensToDtoGet();

                return Ok(itemDto);
            }
            catch (Exception ex)
            {
                _logger.LogError("## Erro ao buscar os registros");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("Item")]
        public async Task<ActionResult<PedidoItemDTO>> PostItem([FromBody]
                                        PedidoItemDTO pedidoItemDTO)
        {
            try
            {
                var novoItem = await _repository.PostItem(pedidoItemDTO);

                var novoItemDto = novoItem.PedidoItemToDtoSet();

                return Ok(novoItemDto);
            }
            catch (Exception ex)
            {
                _logger.LogError("## Erro criar uma novo Registro");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPatch("Item/{id:int}")]
        public async Task<ActionResult<PedidoItemDTO>> PatchItem(int id, PedidoItemDTO pedidoItemDTO)
        {
            try
            {
                var item = await _repository.PatchItem(id, pedidoItemDTO);

                if (item is null)
                {
                    return NotFound();
                }

                var itemDto = item.PedidoItemToDtoSet();

                return Ok(itemDto);
            }
            catch (Exception ex)
            {
                _logger.LogError("## Erro ao alterar o registro");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("Item/{id:int}")]
        public async Task<ActionResult<PedidoItemDTO>> DeleteItem(int id)
        {
            try
            {
                var item = await _repository.DeleteItem(id);

                if (item is null)
                {
                    return NotFound("Registro não localizado");
                }

                var itemDto = item.PedidoItemToDtoSet();

                return Ok(itemDto);
            }
            catch (Exception ex)
            {
                _logger.LogError("## Erro ao excluir o registro");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
