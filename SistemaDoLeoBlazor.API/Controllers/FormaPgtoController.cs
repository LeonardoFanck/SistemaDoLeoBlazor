using Microsoft.AspNetCore.Mvc;
using SistemaDoLeoBlazor.API.Mapping;
using SistemaDoLeoBlazor.API.Repositories.FormaPgtoRepository;
using SistemaDoLeoBlazor.MODELS.FormaPgtoDTO;
using SistemaDoLeoBlazor.MODELS.ProdutoDTO;

namespace SistemaDoLeoBlazor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormaPgtoController : ControllerBase
    {
        private readonly IFormaPgtoRepository _repository;
        private ILogger<FormaPgtoRepository> _logger;

        public FormaPgtoController(IFormaPgtoRepository repository, ILogger<FormaPgtoRepository> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<FormaPgtoDTO>> GetFormaPgto(int id)
        {
            try
            {
                var formaPgto = await _repository.GetFormaPgtoById(id);

                if (formaPgto is null)
                {
                    return NotFound("Registro não localizado");
                }
                else
                {
                    var formaPgtoDto = formaPgto.FormaPgtoToDto();
                    return Ok(formaPgtoDto);
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
        public async Task<ActionResult<IEnumerable<FormaPgtoDTO>>> GetAllFormaPgto()
        {
            try
            {
                var formaPgto = await _repository.GetAllFormaPgto();

                if (formaPgto is null)
                {
                    return NoContent();
                }

                var formaPgtoDto = formaPgto.FormasPgtoToDto();

                return Ok(formaPgtoDto);
            }
            catch (Exception ex)
            {
                _logger.LogError("## Erro ao buscar os registros");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<FormaPgtoDTO>> PostFormaPgto([FromBody]
                                        FormaPgtoDTO formaPgtoDTO)
        {
            try
            {
                var novaFormaPgto = await _repository.PostFormaPgto(formaPgtoDTO);

                var novaFormaPgtoDto = novaFormaPgto.FormaPgtoToDto();

                return CreatedAtAction(nameof(GetFormaPgto), new { id = novaFormaPgtoDto.id }, novaFormaPgtoDto);
            }
            catch (Exception ex)
            {
                _logger.LogError("## Erro criar uma novo Registro");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<FormaPgtoDTO>> PatchFormaPgto(int id, FormaPgtoDTO formaPgtoDTO)
        {
            try
            {
                var formaPgto = await _repository.PatchFormaPgto(id, formaPgtoDTO);

                if (formaPgto is null)
                {
                    return NotFound();
                }

                var formaPgtoDto = formaPgto.FormaPgtoToDto();

                return Ok(formaPgtoDto);
            }
            catch (Exception ex)
            {
                _logger.LogError("## Erro ao alterar o registro");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<FormaPgtoDTO>> DeleteFormaPgto(int id)
        {
            try
            {
                var formaPgto = await _repository.DeleteFormaPgto(id);

                if (formaPgto is null)
                {
                    return NotFound("Registro não localizado");
                }

                var formaPgtoDto = formaPgto.FormaPgtoToDto();

                return Ok(formaPgtoDto);
            }
            catch (Exception ex)
            {
                _logger.LogError("## Erro ao excluir o registro");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
