using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SistemaDoLeoBlazor.API.Mapping;
using SistemaDoLeoBlazor.API.Repositories.CategoriaRepository;
using SistemaDoLeoBlazor.MODELS.CategoriaDTO;

namespace SistemaDoLeoBlazor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaRepository _repository;
        private ILogger<CategoriaController> _logger;

        public CategoriaController(ICategoriaRepository repository, ILogger<CategoriaController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaDTO>> GetCategoria(int id)
        {
            try
            {
                var categoria = await _repository.GetCategoriaById(id);

                if (categoria is null)
                {
                    return NotFound("Categoria não cadastrada");
                }
                else
                {
                    var categoriaDto = categoria.CategoriaToDto();
                    return Ok(categoriaDto);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"## Erro buscar a Categoria {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetAllCategorias()
        {
            try
            {
                var categorias = await _repository.GetAllCategorias();

                if (categorias is null)
                {
                    return NoContent();
                }

                var categoriasDto = categorias.CategoriasToDto();

                return Ok(categoriasDto);
            }
            catch (Exception ex) 
            {
                _logger.LogError("## Erro ao buscar todas as Categorias");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<CategoriaDTO>> PostCategoria([FromBody]
                                        CategoriaDTO categoriaDTO)
        {
            try
            {
                var novaCategoria = await _repository.PostCategoria(categoriaDTO);

                var novaCategoriaDto = novaCategoria.CategoriaToDto();

                return CreatedAtAction(nameof(GetCategoria), new { id = novaCategoriaDto.id }, novaCategoriaDto);
            }
            catch (Exception ex)
            {
                _logger.LogError("## Erro criar uma nova Categoria");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> PatchCategoria(int id, CategoriaDTO categoriaDTO)
        {
            try
            {
                var categoria = await _repository.PatchCategoria(id, categoriaDTO);

                if (categoria is null)
                {
                    return NotFound();
                }

                var categoriaDto = categoria.CategoriaToDto();

                return Ok(categoriaDto);
            }
            catch (Exception ex)
            {
                _logger.LogError("## Erro ao alterar a Categoria");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> DeleteCategoria(int id)
        {
            try
            {
                var categoria = await _repository.DeleteCategoria(id);

                if (categoria is null)
                {
                    return NotFound("Categoria não localizada");
                }

                var categoriaDto = categoria.CategoriaToDto();

                return Ok(categoriaDto);
            }
            catch (Exception ex)
            {
                _logger.LogError("## Erro ao excluir a Categoria");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
