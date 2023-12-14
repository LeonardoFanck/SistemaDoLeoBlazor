using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaDoLeoBlazor.API.Mapping;
using SistemaDoLeoBlazor.API.Repositories.OperadorRepository;
using SistemaDoLeoBlazor.MODELS.OperadorDTOs;

namespace SistemaDoLeoBlazor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperadorController : ControllerBase
    {
        private readonly IOperadorRepository _operadorRepository;

        public OperadorController(IOperadorRepository operadorRepository)
        {
            _operadorRepository = operadorRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OperadorDTO>> GetOperador(int id)
        {
            try
            {
                var operador = await _operadorRepository.GetOperadorById(id);
                
                if (operador is null)
                {
                    return NotFound("Operador não cadastrado!");
                }
                else
                {
                    var operadorDto = operador.OperadorToDto();
                    return Ok(operadorDto);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                              "Erro ao acessar o banco de dados");
            }
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<OperadorDTO>>> GetOperadores()
        {
            try
            {
                var operadores = await _operadorRepository.GetOperadores();

                if (operadores is null)
                {
                    return NoContent();
                }

                var operadoresDto = operadores.OperadoresToDto();

                return Ok(operadoresDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("{id}/GetTelas")]
        public async Task<ActionResult<IEnumerable<OperadorTelaDTO>>> GetTelas(int id)
        {
            try
            {
                var operador = await _operadorRepository.GetOperadorById(id);
                if (operador is null)
                {
                    return NoContent();
                }

                var telas = await _operadorRepository.GetTelas(id);
                if (telas == null)
                {
                    throw new Exception("Nenhuma tela localizada");
                }

                var operadorTelaDto = telas.OperadorTelaToDto();

                return Ok(operadorTelaDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
