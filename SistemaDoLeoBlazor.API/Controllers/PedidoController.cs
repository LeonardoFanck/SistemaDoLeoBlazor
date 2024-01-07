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
                    var pedidoDTO = pedido.PedidoToDto();
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

                var pedidoDTO = pedido.PedidosToDto();

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

                var novoPedidoDTO = novoPedido.PedidoToDto();

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

                var pedidoDto = pedido.PedidoToDto();

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

                var pedidoDto = pedido.PedidoToDto();

                return Ok(pedidoDto);
            }
            catch (Exception ex)
            {
                _logger.LogError("## Erro ao excluir o registro");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
