using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SistemaDoLeoBlazor.API.Entities;
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

        private ILogger<OperadorController> logger;

        public OperadorController(IOperadorRepository operadorRepository, ILogger<OperadorController> logger)
        {
            _operadorRepository = operadorRepository;
            this.logger = logger;
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
            catch (Exception ex) 
            {
                logger.LogError("## Erro criar um novo Operador");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
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
                logger.LogError("## Erro criar um novo Operador");
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
                    return NotFound("Operador não cadastrado!");
                }

                var telas = await _operadorRepository.GetTelas(id);
                if (telas.Count() == 0)
                {
                    return NotFound("Nenhuma tela localizada");
                }

                var operadorTelaDto = telas.OperadorTelasToDto();

                return Ok(operadorTelaDto);
            }
            catch (Exception ex)
            {
                logger.LogError("## Erro criar um novo Operador");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("Operador")]
        public async Task<ActionResult<OperadorDTO>> PostOperador([FromBody]
                                        OperadorDTO operadorDTO)
        {
            try
            {
                var novoOperador = await _operadorRepository.PostOperador(operadorDTO);

                var novoOperadorDto = novoOperador.OperadorToDto();

                return CreatedAtAction(nameof(GetOperador), new {id = novoOperadorDto.id}, novoOperadorDto);
            }
            catch (Exception ex)
            {
                logger.LogError("## Erro criar um novo Operador");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("Telas")]
        public async Task<ActionResult<OperadorTelaDTO>> PostOperadorTela([FromBody] 
                                        OperadorDTO operadorDto)
        {
            try
            {
                var operador = await _operadorRepository.GetOperadorById(operadorDto.id);

                if (operador is null)
                {
                    return NotFound("Operador não cadastrado!");
                }

                await _operadorRepository.PostOperadorTela(operadorDto);

                //var novaTelaDto = novaTela.OperadorTelaToDto();

                // ############## AJUSTAR ####################
                return CreatedAtAction(nameof(GetOperador), new { id = operador.id }, operador);
            }
            catch (Exception ex)
            {
                logger.LogError("## Erro ao vincular uma nova Tela");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<OperadorDTO>> DeleteOperador(int id)
        {
            try
            {
                var operador = await _operadorRepository.DeleteOperador(id);

                if (operador is null)
                {
                    return NotFound("Operador não Localizado");
                }

                var operadorDto = operador.OperadorToDto();

                return operadorDto;
            }
            catch (Exception ex)
            {
                logger.LogError("## Erro ao excluir o Operador");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<Operador>> PatchOperador(int id, OperadorDTO operadorDTO)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                logger.LogError("## Erro ao excluir o Operador");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
