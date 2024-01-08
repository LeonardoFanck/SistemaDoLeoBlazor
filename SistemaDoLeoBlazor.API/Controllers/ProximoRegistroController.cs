using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SistemaDoLeoBlazor.API.Entities;
using SistemaDoLeoBlazor.API.Mapping;
using SistemaDoLeoBlazor.API.Repositories.ProximoRegistroRepository;
using SistemaDoLeoBlazor.MODELS.OperadorDTOs;
using SistemaDoLeoBlazor.MODELS.ProximoRegistroDTO;

namespace SistemaDoLeoBlazor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProximoRegistroController : ControllerBase
    {
        private readonly IProximoRegistroRepository _repository;
        private ILogger<ProximoRegistroController> _logger;

        public ProximoRegistroController(IProximoRegistroRepository repository, ILogger<ProximoRegistroController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ProximoRegistroDTO>> GetProximoRegistro()
        {
            try
            {
                var registro = await _repository.GetRegistro();

                if (registro is null)
                {
                    return NotFound("Registro não cadastrado!");
                }
                
                var registroDto = registro.ProximoRegistroToDto();
                return Ok(registroDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"## Erro buscar o Registro");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPatch]
        public async Task<ActionResult<ProximoRegistroDTO>> PatchOperador(ProximoRegistroDTO proximoRegistroDTO)
        {
            try
            {
                var registro = await _repository.PatchRegistro(proximoRegistroDTO);

                if (registro is null)
                {
                    return NotFound();
                }

                var registroDto = registro.ProximoRegistroToDto();

                return registroDto;
            }
            catch (Exception ex)
            {
                _logger.LogError("## Erro ao alterar o Registro");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
