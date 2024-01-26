using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using SistemaDoLeoBlazor.MODELS.CategoriaDTO;
using SistemaDoLeoBlazor.MODELS.OperadorDTOs;
using SistemaDoLeoBlazor.MODELS.ProximoRegistroDTO;
using SistemaDoLeoBlazor.WEB.Services.CategoriaService;
using SistemaDoLeoBlazor.WEB.Toaster;
using SistemaDoLeoBlazor.MODELS.FormaPgtoDTO;
using Blazored.SessionStorage.JsonConverters;

namespace SistemaDoLeoBlazor.WEB.Pages
{
    public partial class CadFormaPgto
    {
        [Inject] private ToasterService? _toasterService { get; set; }
        [Inject] private NavigationManager? NavigationManager { get; set; }

        private FormaPgtoDTO? formaPgto { get; set; }
        private FormaPgtoDTO? formaPgtoAtual { get; set; }

        // OPERADOR LOGADO NO MOMENTO
        private OperadorDTO? OperadorLogado { get; set; }
        private OperadorTelaDTO? OperadorLogadoTela { get; set; }

        // STATUS
        private bool stsCodFormaPgto { get; set; }
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

        // VALIDAÇÃO PESQUISA
        private bool pesquisaDialogOpen { get; set; }
        private bool pesquisaInativos { get; set; }

        // TOAST
        private string toastTitulo { get; set; } = "Categoria";

        protected async override Task OnInitializedAsync()
        {
            // VERIFICA A SESSÃO DO OPERADOR LOGADO
            await getOperadorSession();
            await validarAcesso();

            // PEGA O ID DO ULTIMO OPERADOR CADASTRADO
            var ultimoId = await getLastRegistro();

            if (ultimoId == -1)
            {
                formaPgto = new FormaPgtoDTO();

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

        private async Task getOperadorSession()
        {
            try
            {
                OperadorLogado = await session.GetSessaoOperador();

                var OperadorLogadoTelas = await session.GetSessaoTelas();

                if (OperadorLogadoTelas is not null)
                {
                    OperadorLogadoTela = OperadorLogadoTelas.FirstOrDefault(t => t.nome.Equals("Forma Pagamento"));
                }
            }
            catch (Exception ex)
            {
                _toasterService.AddToast(Toast.NewToast("ERRO", $"Ocorreu um erro: {ex.Message}", MessageColour.Danger, 8));
            }
        }

        private async Task validarAcesso()
        {
            if (OperadorLogado == null || OperadorLogadoTela is null)
            {
                NavigationManager.NavigateTo("/"); // ALTERAR PARA TELA DE LOGIN
            }
            else
            {
                if (OperadorLogadoTela is not null && OperadorLogadoTela.ativo == false)
                {
                    NavigationManager.NavigateTo("/");
                }
            }
        }

        private async Task<int> getLastRegistro()
        {
            try
            {
                var lista = await formaPgtoService.GetAll();

                if(lista.Count() == 0)
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
                formaPgtoAtual = await formaPgtoService.GetById(id);

                formaPgto = new FormaPgtoDTO
                {
                    id = formaPgtoAtual.id,
                    nome = formaPgtoAtual.nome,
                    inativo = formaPgtoAtual.inativo
                };

                StateHasChanged();
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _toasterService.AddToast(Toast.NewToast(toastTitulo, $"Forma de Pagamento {formaPgto.id} não cadastrado", MessageColour.Danger, 8));

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

                stsCodFormaPgto = true;
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
                formaPgto.id = nextRegistro.formaPgto;
                formaPgto.nome = string.Empty;
                formaPgto.inativo = false;

                StateHasChanged();
            }
            else if (status == EDITAR)
            {
                // false -> ATIVO | true -> INATIVO

                this.status = status;

                stsCodFormaPgto = true;
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

                stsCodFormaPgto = false;
                stsBtnPesquisa = false;
                stsInativo = true;
                stsNome = true;

                if (OperadorLogadoTela.novo || OperadorLogado.admin)
                {
                    stsBtnNovo = false;
                }

                if (OperadorLogadoTela.editar || OperadorLogado.admin)
                {
                    stsBtnEditar = false;
                }

                if (OperadorLogadoTela.excluir || OperadorLogado.admin)
                {
                    stsBtnExcluir = false;
                }
                stsBtnCancelar = true;
                stsBtnSalvar = true;

                // SE FOR NULO É O PRIMEIRO REGISTRO
                if (formaPgtoAtual is not null)
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

                nextRegistro.formaPgto += 1;
            }
            catch (Exception ex)
            {
                _toasterService.AddToast(Toast.NewToast("ERRO", $"Ocorreu um erro: {ex.Message}", MessageColour.Danger, 8));
                throw;
            }
        }

        private void resetaRegistro()
        {
            if (formaPgtoAtual is not null)
            {
                formaPgto.id = formaPgtoAtual.id;
                formaPgto.nome = formaPgtoAtual.nome;
                formaPgto.inativo = formaPgtoAtual.inativo;
            }
            else
            {
                formaPgto.id = 1;
                formaPgto.nome = string.Empty;
                formaPgto.inativo = false;
            }
        }

        private void OnKeyDownTxtID(KeyboardEventArgs e)
        {
            if (e.Code == "Enter" || e.Code == "NumpadEnter")
            {
                getRegistro(formaPgto.id);
            }
            else if (e.Code == "F4")
            {
                openPesquisaDialog();
            }
        }

        private async void salvarCadastro()
        {
            if (status == CADASTRAR)
            {
                try
                {
                    // ADD OPERADOR
                    formaPgtoAtual = await formaPgtoService.Insert(formaPgto);

                    // MENSAGEM DE SUCESSO
                    _toasterService.AddToast(Toast.NewToast(toastTitulo, $"Cadastro Realizdo com sucesso!", MessageColour.Success, 8));

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
                    await formaPgtoService.Update(formaPgto);

                    // MENSAGEM DE SUCESSO
                    _toasterService.AddToast(Toast.NewToast(toastTitulo, $"Cadastro atualizado com sucesso!", MessageColour.Success, 8));

                    // BUSCA AS NOVA INFORMAÇÕES
                    getRegistro(formaPgto.id);

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
                    await formaPgtoService.Delete(formaPgto.id);

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
            mensagem = $"Deseja realmente excluir a Forma de Pagamento[ {formaPgto.id} - {formaPgto.nome} ] ?";

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
