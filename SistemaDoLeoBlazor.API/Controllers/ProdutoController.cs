using Microsoft.AspNetCore.Mvc;
using SistemaDoLeoBlazor.API.Mapping;
using SistemaDoLeoBlazor.API.Repositories.ProdutoRepository;
using SistemaDoLeoBlazor.MODELS.CategoriaDTO;
using SistemaDoLeoBlazor.MODELS.ProdutoDTO;

namespace SistemaDoLeoBlazor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoRepository _repository;
        private ILogger<ProdutoRepository> _logger;

        public ProdutoController(IProdutoRepository repository, ILogger<ProdutoRepository> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProdutoDTO>> GetProduto(int id)
        {
            try
            {
                var produto = await _repository.GetProdutoById(id);

                if (produto is null)
                {
                    return NotFound("Produto não cadastrado");
                }
                else
                {
                    var produtoDto = produto.ProdutoToDto();
                    return Ok(produtoDto);
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
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetAllProdutos()
        {
            try
            {
                var produtos = await _repository.GetAllProdutos();

                if (produtos is null)
                {
                    return NoContent();
                }

                var produtosDto = produtos.ProdutosToDto();

                return Ok(produtosDto);
            }
            catch (Exception ex)
            {
                _logger.LogError("## Erro ao buscar os registros");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ProdutoDTO>> PostProduto([FromBody]
                                        ProdutoDTO produtoDTO)
        {
            try
            {
                var novoProduto = await _repository.PostProduto(produtoDTO);

                var novoProdutoDto = novoProduto.ProdutoToDto();

                return CreatedAtAction(nameof(GetProduto), new { id = novoProdutoDto.id }, novoProdutoDto);
            }
            catch (Exception ex)
            {
                _logger.LogError("## Erro criar uma novo Registro");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<ProdutoDTO>> PatchProduto(int id, ProdutoDTO produtoDTO)
        {
            try
            {
                var produto = await _repository.PatchProduto(id, produtoDTO);

                if (produto is null)
                {
                    return NotFound();
                }

                var produtoDto = produto.ProdutoToDto();

                return Ok(produtoDto);
            }
            catch (Exception ex)
            {
                _logger.LogError("## Erro ao alterar o registro");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ProdutoDTO>> DeleteProduto(int id)
        {
            try
            {
                var produto = await _repository.DeleteProduto(id);

                if (produto is null)
                {
                    return NotFound("Registro não localizado");
                }

                var produtoDto = produto.ProdutoToDto();

                return Ok(produtoDto);
            }
            catch (Exception ex)
            {
                _logger.LogError("## Erro ao excluir o registro");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
