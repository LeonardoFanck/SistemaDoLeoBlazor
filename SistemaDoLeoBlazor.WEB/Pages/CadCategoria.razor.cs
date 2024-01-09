using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Win32;
using SistemaDoLeoBlazor.MODELS.CategoriaDTO;
using SistemaDoLeoBlazor.MODELS.OperadorDTOs;
using SistemaDoLeoBlazor.MODELS.ProximoRegistroDTO;
using SistemaDoLeoBlazor.WEB.Services.OperadorService.OperadorService;
using SistemaDoLeoBlazor.WEB.Toaster;

namespace SistemaDoLeoBlazor.WEB.Pages
{
    public partial class CadCategoria
    {
        [Inject] private ToasterService? _toasterService { get; set; }
        [Inject] private NavigationManager? NavigationManager { get; set; }

        private CategoriaDTO? categoria { get; set; }
        private CategoriaDTO? categoriaAtual { get; set; }

        // OPERADOR LOGADO NO MOMENTO
        private OperadorDTO? OperadorLogado { get; set; }
        private IEnumerable<OperadorTelaDTO>? OperadorLogadoTelas { get; set; }

        // STATUS
        private bool stsCodCategoria { get; set; }
        private bool stsBtnPesquisa { get; set; }
        private bool stsNome { get; set; }
        private bool stsInativo { get; set; }

        private bool stsBtnNovo { get; set; }
        private bool stsBtnEditar { get; set; }
        private bool stsBtnExcluir { get; set; }
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

        protected async override Task OnInitializedAsync()
        {
            // VERIFICA A SESSÃO DO OPERADOR LOGADO
            await getOperadorSession();
            await validarAcesso();

            // PEGA O ID DO ULTIMO OPERADOR CADASTRADO
            var ultimoId = await getLastRegistro();

            // PEGA AS INFORMAÇÕES DO ULTIMO OPERADOR
            getRegistro(ultimoId);

            // ALTERA O STATUS
            validaStatus(VISUALIZAR);
        }

        private async Task getOperadorSession()
        {
            try
            {
                OperadorLogado = await session.GetSessaoOperador();

                OperadorLogadoTelas = await session.GetSessaoTelas();
            }
            catch (Exception ex)
            {
                _toasterService.AddToast(Toast.NewToast("ERRO", $"Ocorreu um erro: {ex.Message}", MessageColour.Danger, 8));
            }
        }

        private async Task validarAcesso()
        {
            if (OperadorLogado == null || OperadorLogadoTelas.Count() == 0)
            {
                NavigationManager.NavigateTo("/"); // ALTERAR PARA TELA DE LOGIN
            }
            else
            {
                var tela = OperadorLogadoTelas.FirstOrDefault(t => t.nome.Equals("Categoria"));

                if (tela is not null && tela.ativo == false)
                {
                    NavigationManager.NavigateTo("/");
                }
            }
        }

        private async Task<int> getLastRegistro()
        {
            try
            {
                var lista = await categoriaService.GetAll();

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
                categoriaAtual = await categoriaService.GetById(id);

                categoria = new CategoriaDTO
                {
                    id = categoriaAtual.id,
                    nome = categoriaAtual.nome,
                    inativo = categoriaAtual.inativo
                };

                StateHasChanged();
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _toasterService.AddToast(Toast.NewToast("Categoria inválida", $"Categoria {categoria.id} não cadastrado", MessageColour.Danger, 8));

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

                stsCodCategoria = true;
                stsBtnPesquisa = true;
                stsInativo = false;
                stsNome = false;
                stsBtnNovo = true;
                stsBtnEditar = true;
                stsBtnExcluir = true;
                stsBtnCancelar = false;
                stsBtnSalvar = false;

                await pegarProximoRegistro();

                // LIMPA OS VALORES DO OPERADOR
                categoria.id = nextRegistro.categoria;
                categoria.nome = string.Empty;
                categoria.inativo = false;

                StateHasChanged();
            }
            else if (status == EDITAR)
            {
                // false -> ATIVO | true -> INATIVO

                this.status = status;

                stsCodCategoria = true;
                stsBtnPesquisa = true;
                stsInativo = false;
                stsNome = false;
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

                stsCodCategoria = false;
                stsBtnPesquisa = false;
                stsInativo = true;
                stsNome = true;
                stsBtnNovo = false;
                stsBtnEditar = false;
                stsBtnExcluir = false;
                stsBtnCancelar = true;
                stsBtnSalvar = true;

                // SE FOR NULO É O PRIMEIRO REGISTRO
                if(categoriaAtual is not null)
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

                nextRegistro.categoria += 1;
            }
            catch (Exception ex)
            {
                _toasterService.AddToast(Toast.NewToast("ERRO", $"Ocorreu um erro: {ex.Message}", MessageColour.Danger, 8));
                throw;
            }
        }

        private void resetaRegistro()
        {
            categoria.id = categoriaAtual.id;
            categoria.nome = categoriaAtual.nome;
            categoria.inativo = categoriaAtual.inativo;
        }

        private void OnKeyDownTxtID(KeyboardEventArgs e)
        {
            if (e.Code == "Enter" || e.Code == "NumpadEnter")
            {
                getRegistro(categoria.id);
            }
        }

        private async void salvarCadastro()
        {
            if (status == CADASTRAR)
            {
                try
                {
                    // ADD OPERADOR
                    categoriaAtual = await categoriaService.Insert(categoria);

                    // MENSAGEM DE SUCESSO
                    _toasterService.AddToast(Toast.NewToast("Cadastro Categoria", $"Cadastro Realizdo com sucesso!", MessageColour.Success, 8));

                    // ATUALIZA O ULTIMO REGISTRO CADASTRADO
                    await registroService.PatchProximoRegistro(nextRegistro);

                    // ALTERA O STATUS
                    validaStatus(VISUALIZAR);
                }
                catch (HttpRequestException ex)
                {
                    _toasterService.AddToast(Toast.NewToast("Erro", $"Erro ao cadastrar a Categoria: {ex.Message}", MessageColour.Danger, 8));
                }
                catch (Exception ex)
                {
                    _toasterService.AddToast(Toast.NewToast("Erro", $"Erro ao cadastrar a Categoria: {ex.Message}", MessageColour.Danger, 8));
                }
            }
            else if (status == EDITAR)
            {
                try
                {
                    // ATUALIZA O OPERADOR
                    await categoriaService.Update(categoria);

                    // MENSAGEM DE SUCESSO
                    _toasterService.AddToast(Toast.NewToast("Atualizar Categoria", $"Cadastro atualizado com sucesso!", MessageColour.Success, 8));

                    // BUSCA AS NOVA INFORMAÇÕES
                    getRegistro(categoria.id);

                    // ALTERA O STATUS
                    validaStatus(VISUALIZAR);
                }
                catch (HttpRequestException ex)
                {
                    _toasterService.AddToast(Toast.NewToast("Erro", $"Erro ao cadastrar a Categoria: {ex.Message}", MessageColour.Danger, 8));
                }
                catch (Exception ex)
                {
                    _toasterService.AddToast(Toast.NewToast("Erro", $"Erro ao cadastrar a Categoria: {ex.Message}", MessageColour.Danger, 8));
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
                    await categoriaService.Delete(categoria.id);

                    // MENSAGEM DE SUCESSO
                    _toasterService.AddToast(Toast.NewToast("Categoria Deletada", $"Cadastro deletado com sucesso!", MessageColour.Success, 8));
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
                _toasterService.AddToast(Toast.NewToast("Erro", $"Erro ao cadastrar a Categoria: {ex.Message}", MessageColour.Danger, 8));
            }
            catch (Exception ex)
            {
                _toasterService.AddToast(Toast.NewToast("Erro", $"Erro ao cadastrar a Categoria: {ex.Message}", MessageColour.Danger, 8));
            }
        }

        private void OpenDeleteDialog()
        {
            mensagem = $"Deseja realmente excluir a Categoria {categoria.id} - {categoria.nome} ?";

            DeleteDialogOpen = true;
            StateHasChanged();
        }


















    }
}
