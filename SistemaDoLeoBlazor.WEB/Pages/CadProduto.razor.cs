using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using SistemaDoLeoBlazor.MODELS.OperadorDTOs;
using SistemaDoLeoBlazor.MODELS.ProximoRegistroDTO;
using SistemaDoLeoBlazor.WEB.Toaster;
using SistemaDoLeoBlazor.MODELS.ProdutoDTO;
using SistemaDoLeoBlazor.MODELS.CategoriaDTO;
using System.Runtime.InteropServices;

namespace SistemaDoLeoBlazor.WEB.Pages
{
    public partial class CadProduto
    {
        [Inject] private ToasterService? _toasterService { get; set; }
        [Inject] private NavigationManager? NavigationManager { get; set; }

        private ProdutoDTO? produto { get; set; }
        private ProdutoDTO? produtoAtual { get; set; }
        private IEnumerable<CategoriaDTO>? categorias { get; set; }
        private CategoriaDTO? categoria { get; set; }
        private int categoriaId { get; set; }

        // OPERADOR LOGADO NO MOMENTO
        private OperadorDTO? operadorLogado { get; set; }
        private OperadorTelaDTO? operadorLogadoTela { get; set; }

        // STATUS
        private bool stsCodProduto { get; set; }
        private bool stsBtnPesquisa { get; set; }
        private bool stsInativo { get; set; }
        private bool stsNome { get; set; }
        private bool stsCategoria { get; set; }
        private bool stsPreco { get; set; }
        private bool stsCusto { get; set; }
        private bool stsEstoque { get; set; } = true;
        private bool stsUnidade { get; set; }

        private bool stsBtnNovo { get; set; } = true;
        private bool stsBtnEditar { get; set; } = true;
        private bool stsBtnExcluir { get; set; } = true;
        private bool stsBtnCancelar { get; set; }
        private bool stsBtnSalvar { get; set; }

        // STATUS
        private int status;
        private int CADASTRAR = 0;
        private int VISUALIZAR = 1;
        private int EDITAR = 2;

        // PROXIMO REGISTRO
        private ProximoRegistroDTO? nextRegistro { get; set; }

        // VALIDAÇÃO DELETE
        private bool DeleteDialogOpen { get; set; }
        private string mensagem = "";

        // VALIDAÇÃO PESQUISA
        private bool pesquisaDialogOpen { get; set; }
        private bool pesquisaInativos { get; set; }

        // TOAST
        private string toastTitulo { get; set; } = "Produto";


        protected async override Task OnInitializedAsync()
        {
            // VERIFICA A SESSÃO DO OPERADOR LOGADO
            await getOperadorSession();
            await validarAcesso();

            await getCategorias();

            // PEGA O ID DO ULTIMO OPERADOR CADASTRADO
            var ultimoId = await getLastRegistro();

            if (ultimoId == -1)
            {
                produto = new ProdutoDTO();

                validaStatus(CADASTRAR);
            }
            else
            {
                // PEGA AS INFORMAÇÕES DO ULTIMO OPERADOR
                getRegistro(ultimoId);

                // ALTERA O STATUS
                validaStatus(VISUALIZAR);
            }
        }

        private async Task getCategorias()
        {
            try
            {
                var listCategoria = await categoriaService.GetAll();

                categorias = from categoria in listCategoria
                             where (categoria.inativo == false)
                             select new CategoriaDTO
                             {
                                 id = categoria.id,
                                 inativo = categoria.inativo,
                                 nome = categoria.nome
                             };

                if (categorias.Count() == 0)
                {
                    _toasterService.AddToast(Toast.NewToast(toastTitulo, $"Nenhuma Categoria cadastrada! Necessário ter ao menos um cadastro", MessageColour.Warning, 30));
                }
            }
            catch(Exception ex)
            {
                _toasterService.AddToast(Toast.NewToast("ERRO", $"Ocorreu um erro: {ex.Message}", MessageColour.Danger, 8));
            }
        }

        private async Task getOperadorSession()
        {
            try
            {
                operadorLogado = await session.GetSessaoOperador();

                var operadorLogadoTelas = await session.GetSessaoTelas();

                if(operadorLogadoTelas is not null) {
                    operadorLogadoTela = operadorLogadoTelas.First(o => o.nome.Equals("Produto"));
                }
            }
            catch (Exception ex)
            {
                _toasterService.AddToast(Toast.NewToast("ERRO", $"Ocorreu um erro: {ex.Message}", MessageColour.Danger, 8));
            }
        }

        private async Task validarAcesso()
        {
            if (operadorLogado == null || operadorLogadoTela is null)
            {
                NavigationManager.NavigateTo("/"); // ALTERAR PARA TELA DE LOGIN
            }
            else
            {
                if (operadorLogadoTela is not null && operadorLogadoTela.ativo == false)
                {
                    NavigationManager.NavigateTo("/");
                }
            }
        }

        private async Task<int> getLastRegistro()
        {
            try
            {
                var lista = await produtoService.GetAll();

                if (lista.Count() == 0)
                {
                    return -1;
                }

                var registro = lista.Last();

                return registro.id;
            }
            catch (HttpRequestException ex)
            {
                _toasterService.AddToast(Toast.NewToast("ERRO", $"Ocorreu um erro: {ex.Message}", MessageColour.Danger, 8));
                throw;
            }
        }

        private async void getRegistro(int id)
        {
            try
            {
                produtoAtual = await produtoService.GetById(id);

                categoria = categorias.FirstOrDefault(c => c.id.Equals(produtoAtual.categoriaId));

                produto = new ProdutoDTO
                {
                    id = produtoAtual.id,
                    nome = produtoAtual.nome,
                    inativo = produtoAtual.inativo,
                    categoriaId = produtoAtual.categoriaId,
                    custo = produtoAtual.custo,
                    estoque = produtoAtual.estoque,
                    preco = produtoAtual.preco,
                    unidade = produtoAtual.unidade
                };

                StateHasChanged();
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _toasterService.AddToast(Toast.NewToast(toastTitulo, $"Produto {produto.id} não cadastrado", MessageColour.Danger, 8));

                    resetaRegistro();

                    // RENDERIZA NOVAMENTE O COMPONENTE
                    StateHasChanged();
                }
            }
        }

        private async void validaStatus(int status)
        {
            if (status == CADASTRAR)
            {
                // false -> ATIVO | true -> INATIVO

                this.status = status;

                stsCodProduto = true;
                stsBtnPesquisa = true;
                stsInativo = false;
                stsNome = false;
                stsCategoria = false;
                stsPreco = false;
                stsCusto = false;
                stsUnidade = false;

                stsBtnNovo = true;
                stsBtnEditar = true;
                stsBtnExcluir = true;
                stsBtnCancelar = false;
                stsBtnSalvar = false;

                await pegarProximoRegistro();

                // LIMPA OS VALORES DO OPERADOR
                produto.id = nextRegistro.produto;
                produto.nome = string.Empty;
                produto.inativo = false;
                produto.categoriaId = 0;
                produto.preco = decimal.Zero;
                produto.custo = decimal.Zero;
                produto.estoque = 0;
                produto.unidade = string.Empty;

                StateHasChanged();
            }
            else if (status == EDITAR)
            {
                // false -> ATIVO | true -> INATIVO

                this.status = status;

                stsCodProduto = true;
                stsBtnPesquisa = true;
                stsInativo = false;
                stsNome = false;
                stsCategoria = false;
                stsPreco = false;
                stsCusto = false;
                stsUnidade = false;

                stsBtnNovo = true;
                stsBtnEditar = true;
                stsBtnExcluir = true;
                stsBtnCancelar = false;
                stsBtnSalvar = false;

                // RESTAURA AS INFORMAÇÕES DA CATEGORIA
                resetaRegistro();
            }
            else if (status == VISUALIZAR)
            {
                // false -> ATIVO | true -> INATIVO

                this.status = status;

                stsCodProduto = false;
                stsBtnPesquisa = false;
                stsInativo = true;
                stsNome = true;
                stsCategoria = true;
                stsPreco = true;
                stsCusto = true;
                stsUnidade = true;

                if (operadorLogadoTela.novo || operadorLogado.admin)
                {
                    stsBtnNovo = false;
                }

                if (operadorLogadoTela.editar || operadorLogado.admin)
                {
                    stsBtnEditar = false;
                }

                if (operadorLogadoTela.excluir || operadorLogado.admin)
                {
                    stsBtnExcluir = false;
                }
                
                stsBtnCancelar = true;
                stsBtnSalvar = true;

                // SE FOR NULO É O PRIMEIRO REGISTRO
                if (produtoAtual is not null)
                {
                    resetaRegistro();
                }
            }
        }

        private async Task pegarProximoRegistro()
        {
            try
            {
                nextRegistro = await registroService.GetProximoRegistro();

                nextRegistro.produto += 1;
            }
            catch (Exception ex)
            {
                _toasterService.AddToast(Toast.NewToast("ERRO", $"Ocorreu um erro: {ex.Message}", MessageColour.Danger, 8));
                throw;
            }
        }

        private void resetaRegistro()
        {
            if (produtoAtual is not null)
            {
                produto.id = produtoAtual.id;
                produto.nome = produtoAtual.nome;
                produto.inativo = produtoAtual.inativo;
                produto.categoriaId = produtoAtual.categoriaId;
                produto.preco = produtoAtual.preco;
                produto.custo = produtoAtual.custo;
                produto.estoque = produtoAtual.estoque;
                produto.unidade = produtoAtual.unidade;

                categoria = categorias.FirstOrDefault(c => c.id.Equals(produtoAtual.categoriaId));
            }
            else
            {
                produto.id = nextRegistro.produto;
                produto.nome = string.Empty;
                produto.inativo = false;
                produto.categoriaId = 0;
                produto.preco = decimal.Zero;
                produto.custo = decimal.Zero;
                produto.estoque = 0;
                produto.unidade = string.Empty;
            }
        }

        private void OnKeyDownTxtID(KeyboardEventArgs e)
        {
            if (e.Code == "Enter" || e.Code == "NumpadEnter")
            {
                getRegistro(produto.id);
            }
            else if (e.Code == "F4")
            {
                openPesquisaDialog();
            }
        }

        private void alterarCategoria(ChangeEventArgs e)
        {
            var id = e.Value;

            if (!id.Equals(""))
            {
                categoriaId = Convert.ToInt32(id);
            }
        }

        private async void salvarCadastro()
        {
            if (status == CADASTRAR)
            {
                try
                {
                    produto.categoriaId = categoriaId;

                    // ADD OPERADOR
                    produtoAtual = await produtoService.Insert(produto);

                    // MENSAGEM DE SUCESSO
                    _toasterService.AddToast(Toast.NewToast(toastTitulo, $"Cadastro Realizado com sucesso!", MessageColour.Success, 8));

                    // ATUALIZA O ULTIMO REGISTRO CADASTRADO
                    await registroService.PatchProximoRegistro(nextRegistro);

                    // ALTERA O STATUS
                    validaStatus(VISUALIZAR);
                }
                catch (HttpRequestException ex)
                {
                    _toasterService.AddToast(Toast.NewToast("Erro", $"Erro ao cadastrar: {ex.Message}", MessageColour.Danger, 8));
                }
                catch (Exception ex)
                {
                    _toasterService.AddToast(Toast.NewToast("Erro", $"Erro ao cadastrar: {ex.Message}", MessageColour.Danger, 8));
                }
            }
            else if (status == EDITAR)
            {
                try
                {
                    // ATUALIZA O OPERADOR
                    await produtoService.Update(produto);

                    // MENSAGEM DE SUCESSO
                    _toasterService.AddToast(Toast.NewToast(toastTitulo, $"Cadastro atualizado com sucesso!", MessageColour.Success, 8));

                    // BUSCA AS NOVA INFORMAÇÕES
                    getRegistro(produto.id);

                    // ALTERA O STATUS
                    validaStatus(VISUALIZAR);
                }
                catch (HttpRequestException ex)
                {
                    _toasterService.AddToast(Toast.NewToast("Erro", $"Erro ao cadastrar: {ex.Message}", MessageColour.Danger, 8));
                }
                catch (Exception ex)
                {
                    _toasterService.AddToast(Toast.NewToast("Erro", $"Erro ao cadastrar: {ex.Message}", MessageColour.Danger, 8));
                }
            }

            StateHasChanged();
        }

        private async void OnDeleteDialogClose(bool accepted)
        {
            try
            {
                if (accepted)
                {
                    await produtoService.Delete(produto.id);

                    // MENSAGEM DE SUCESSO
                    _toasterService.AddToast(Toast.NewToast(toastTitulo, $"Cadastro deletado com sucesso!", MessageColour.Success, 8));
                }

                DeleteDialogOpen = false;
                StateHasChanged();

                // PEGA O ID DO ULTIMO REGISTRO
                var ultimoId = await getLastRegistro();

                // BUSCA AS INFORMAÇÕES DO ULTIMO REGISTRO
                getRegistro(ultimoId);
            }
            catch (HttpRequestException ex)
            {
                _toasterService.AddToast(Toast.NewToast("Erro", $"Erro ao cadastrar: {ex.Message}", MessageColour.Danger, 8));
            }
            catch (Exception ex)
            {
                _toasterService.AddToast(Toast.NewToast("Erro", $"Erro ao cadastrar: {ex.Message}", MessageColour.Danger, 8));
            }
        }

        private void OpenDeleteDialog()
        {
            mensagem = $"Deseja realmente excluir o Produto {produto.id} - {produto.nome} ?";

            DeleteDialogOpen = true;
            StateHasChanged();
        }

        private void openPesquisaDialog()
        {
            pesquisaInativos = true;
            pesquisaDialogOpen = true;
            StateHasChanged();
        }

        private async void onPesquisaDialogClose(int id)
        {
            if (id != -1)
            {
                getRegistro(id);
            }

            pesquisaDialogOpen = false;
        }
    }
}
