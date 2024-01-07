using Microsoft.AspNetCore.Mvc;
using SistemaDoLeoBlazor.API.Mapping;
using SistemaDoLeoBlazor.API.Repositories.ClienteRepository;
using SistemaDoLeoBlazor.MODELS.ClienteDTO;
using SistemaDoLeoBlazor.MODELS.FormaPgtoDTO;

namespace SistemaDoLeoBlazor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteRepository _repository;
        private ILogger<ClienteController> _logger;

        public ClienteController(IClienteRepository repository, ILogger<ClienteController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ClienteDTO>> GetCliente(int id)
        {
            try
            {
                var cliente = await _repository.GetClienteById(id);

                if (cliente is null)
                {
                    return NotFound("Registro não localizado");
                }
                else
                {
                    var clienteDto = cliente.ClienteToDto();
                    return Ok(clienteDto);
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
        public async Task<ActionResult<IEnumerable<ClienteDTO>>> GetAllCliente()
        {
            try
            {
                var cliente = await _repository.GetAllClientes();

                if (cliente is null)
                {
                    return NoContent();
                }

                var clienteDto = cliente.ClientesToDto();

                return Ok(clienteDto);
            }
            catch (Exception ex)
            {
                _logger.LogError("## Erro ao buscar os registros");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ClienteDTO>> PostCliente([FromBody]
                                        ClienteDTO clienteDTO)
        {
            try
            {
                var novoCliente = await _repository.PostCliente(clienteDTO);

                var novoClienteDto = novoCliente.ClienteToDto();

                return CreatedAtAction(nameof(GetCliente), new { id = novoClienteDto.id }, novoClienteDto);
            }
            catch (Exception ex)
            {
                _logger.LogError("## Erro criar uma novo Registro");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<ClienteDTO>> PatchCliente(int id, ClienteDTO clienteDTO)
        {
            try
            {
                var cliente = await _repository.PatchCliente(id, clienteDTO);

                if (cliente is null)
                {
                    return NotFound();
                }

                var clienteDto = cliente.ClienteToDto();

                return Ok(clienteDto);
            }
            catch (Exception ex)
            {
                _logger.LogError("## Erro ao alterar o registro");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ClienteDTO>> DeleteCliente(int id)
        {
            try
            {
                var cliente = await _repository.DeleteCliente(id);

                if (cliente is null)
                {
                    return NotFound("Registro não localizado");
                }

                var clienteDto = cliente.ClienteToDto();

                return Ok(clienteDto);
            }
            catch (Exception ex)
            {
                _logger.LogError("## Erro ao excluir o registro");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
