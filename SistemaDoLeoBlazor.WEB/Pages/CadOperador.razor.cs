using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using SistemaDoLeoBlazor.MODELS.OperadorDTOs;
using SistemaDoLeoBlazor.WEB.Toaster;
using SistemaDoLeoBlazor.WEB.Services;
using SistemaDoLeoBlazor.WEB.Shared;
using System.ComponentModel;

namespace SistemaDoLeoBlazor.WEB.Pages;

public partial class CadOperador
{
    [Inject] private ToasterService? _toasterService { get; set; }

    private OperadorDTO? Operador { get; set; }
    private IEnumerable<OperadorTelaDTO>? OperadorTelas { get; set; }
    private OperadorDTO? OperadorAtual { get; set; }
    private string erro { get; set; } = string.Empty;

    // STATUS CADASTRO
    private bool stsCodOperador { get; set; }
    private bool stsPesquisa { get; set; }
    private bool stsNome { get; set; }
    private bool stsSenha { get; set; }
    private bool stsChkSenha { get; set; }
    private bool stsChkAdmin { get; set; }
    private bool stsChkInativo { get; set; }
    private bool stsBtnNovo { get; set; }
    private bool stsBtnEditar { get; set; }
    private bool stsBtnExcluir { get; set; }
    private bool stsBtnCancelar { get; set; }
    private bool stsBtnSalvar { get; set; }

    // STATUS TELAS
    private bool stsChkItemAtivo { get; set; }
    private bool stsChkItemEditar { get; set; }
    private bool stsChkItemExcluir { get; set; }
    private bool stsChkItemNovo { get; set; }

    // STATUS
    private int status;
    private int CADASTRAR = 0;
    private int VISUALIZAR = 1;
    private int EDITAR = 2;

    // VISUALIZAR SENHA
    private bool chkSenha { get; set; }
    private string typeSenha { get; set; } = "password";

    // VALIDAÇÃO DELETE
    private bool DeleteDialogOpen {get; set; }
    private string mensagem = "";

    protected override async Task OnInitializedAsync()
    {
        // PEGA O ID DO ULTIMO OPERADOR CADASTRADO
        var ultimoId = await getLastOperador();

        // PEGA AS INFORMAÇÕES DO ULTIMO OPERADOR
        getOperador(ultimoId);

        // ALTERA O STATUS
        validaStatus(VISUALIZAR);        
    }

    private async void validaStatus(int status)
    {
        if (status == CADASTRAR)
        {
            // false -> ATIVO | true -> INATIVO

            this.status = status;

            stsCodOperador = true;
            stsPesquisa = true;
            stsNome = false;
            stsSenha = false;
            stsChkSenha = false;
            stsChkAdmin = false;
            stsChkInativo = false;
            stsBtnNovo = true;
            stsBtnEditar = true;
            stsBtnExcluir = true;
            stsBtnCancelar = false;
            stsBtnSalvar = false;

            // TELAS
            stsChkItemAtivo = false;
            stsChkItemEditar = false;
            stsChkItemExcluir = false;
            stsChkItemNovo = false;

            // LIMPA OS VALORES DO OPERADOR
            Operador.id = 99999;
            Operador.nome = string.Empty;
            Operador.senha = string.Empty;
            Operador.admin = false;
            Operador.inativo = false;

            foreach(var tela in OperadorTelas)
            {
                tela.ativo = false;
                tela.novo = false;
                tela.excluir = false;
                tela.editar = false;
            }
        }
        else if (status == EDITAR)
        {
            // false -> ATIVO | true -> INATIVO

            this.status = status;

            stsCodOperador = true;
            stsPesquisa = true;
            stsNome = false;
            stsSenha = false;
            stsChkSenha = false;
            stsChkAdmin = false;
            stsChkInativo = false;
            stsBtnNovo = true;
            stsBtnEditar = true;
            stsBtnExcluir = true;
            stsBtnCancelar = false;
            stsBtnSalvar = false;

            // TELAS
            stsChkItemAtivo = false;
            stsChkItemEditar = false;
            stsChkItemExcluir = false;
            stsChkItemNovo = false;

            // RESTAURA AS INFORMAÇÕES DO OPERADOR
            resetarOperador();
        }
        else if (status == VISUALIZAR)
        {
            // false -> ATIVO | true -> INATIVO

            this.status = status;

            stsCodOperador = false;
            stsPesquisa = false;
            stsNome = true;
            stsSenha = true;
            stsChkSenha = true;
            stsChkAdmin = true;
            stsChkInativo = true;
            stsBtnNovo = false;
            stsBtnEditar = false;
            stsBtnExcluir = false;
            stsBtnCancelar = true;
            stsBtnSalvar = true;

            // TELAS
            stsChkItemAtivo = true;
            stsChkItemEditar = true;
            stsChkItemExcluir = true;
            stsChkItemNovo = true;

            // ESCONDE NOVAMENTE A SENHA
            if (chkSenha == true)
            {
                mostrarEsconderSenha();
            }

            // SE FOR NULO É PORQUE É O PRIMEIRO SELECT
            if (OperadorAtual is not null)
            {
                // RESETA OS VALORES DO OPERADOR
                resetarOperador();

                resetarTelas(OperadorAtual.id);
            }
        }
    }

    private void mostrarEsconderSenha()
    {
        if (typeSenha.Equals("password"))
        {
            typeSenha = "text";
            chkSenha = true;
        }
        else
        {
            typeSenha = "password";
            chkSenha = false;
        }
    }

    private void OnKeyDownTxtID(KeyboardEventArgs e)
    {
        if (e.Code == "Enter" || e.Code == "NumpadEnter")
        {
            getOperador(Operador.id);
        }
    }

    private async Task<int> getLastOperador()
    {
        try
        {
            var listaOperadores = await operadorService.GetAllOperadores();

            var operador = listaOperadores.Last();

            return operador.id;
        }
        catch (HttpRequestException ex)
        {
            _toasterService.AddToast(Toast.NewToast("ERRO", $"Ocorreu um erro: {ex.Message}", MessageColour.Danger, 8));
            throw;
        }
    }

    private async void getOperador(int id)
    {
        try
        {
            OperadorAtual = await operadorService.GetOperadorById(id);

            Operador = new OperadorDTO
            {
                id = OperadorAtual.id,
                nome = OperadorAtual.nome,
                senha = OperadorAtual.senha,
                admin = OperadorAtual.admin,
                inativo = OperadorAtual.inativo
            };

            // ADICIONA AS TELAS AO OPERADOR CASO NÃO TENHA TODAS
            await AddTelasOperador();

            // PEGA AS TELAS DO OPERAODR
            OperadorTelas = await operadorService.GetTelasByOperador(id);

            // RENDERIZA NOVAMENTE O COMPONENTE
            _ = InvokeAsync(StateHasChanged);
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _toasterService.AddToast(Toast.NewToast("Operador inválido", $"Operador {Operador.id} não cadastrado", MessageColour.Danger, 8));

                resetarOperador();

                // RENDERIZA NOVAMENTE O COMPONENTE
                _ = InvokeAsync(StateHasChanged);
            }
        }
    }

    private void resetarOperador()
    {
        Operador.id = OperadorAtual.id;
        Operador.nome = OperadorAtual.nome;
        Operador.senha = OperadorAtual.senha;
        Operador.admin = OperadorAtual.admin;
        Operador.inativo = OperadorAtual.inativo;
    }

    private async void resetarTelas(int id)
    {
        OperadorTelas = await operadorService.GetTelasByOperador(id);

        // RENDERIZA NOVAMENTE O COMPONENTE
        _ = InvokeAsync(StateHasChanged);
    }

    private async Task<OperadorDTO> AddTelasOperador()
    {
        return await operadorService.PostOperadorTelas(OperadorAtual);
    }

    private async void salvarCadastro()
    {
        if(status == CADASTRAR)
        {
            try
            {
                // ADD OPERADOR
                OperadorAtual = await operadorService.PostOperador(Operador);

                // ADD TELAS
                OperadorAtual = await AddTelasOperador();

                // PEGA AS INFORMAÇÕES DAS TELAS QUE FORAM CADASTRADAS ACIMA
                var novasTela = await operadorService.GetTelasByOperador(OperadorAtual.id);

                // JUNTA AS 2 LISTAS EM UMAS SÓ PARA ATUALIZAR
                var telas = from novasTelas in novasTela
                                join config in OperadorTelas on novasTelas.idTela equals config.idTela
                                select new OperadorTelaDTO
                                {
                                    id = novasTelas.id,
                                    idOperador = novasTelas.idOperador,
                                    idTela = novasTelas.idTela,
                                    nome = novasTelas.nome,
                                    ativo = config.ativo,
                                    novo = config.novo,
                                    editar = config.editar,
                                    excluir = config.excluir
                                };

                // ATUALIZA AS TELAS 
                foreach (var tela in telas)
                {
                    await operadorService.PatchOperadorTelas(tela);
                }

                // MENSAGEM DE SUCESSO
                _toasterService.AddToast(Toast.NewToast("Cadastro Operador", $"Cadastro Realizdo com sucesso!", MessageColour.Success, 8));

                // ALTERA O STATUS
                validaStatus(VISUALIZAR);
            }
            catch (HttpRequestException ex)
            {
                _toasterService.AddToast(Toast.NewToast("Erro", $"Erro ao cadastrar o Operador: {ex.Message}", MessageColour.Danger, 8));
            }
            catch (Exception ex)
            {
                _toasterService.AddToast(Toast.NewToast("Erro", $"Erro ao cadastrar o Operador: {ex.Message}", MessageColour.Danger, 8));
            }
        }
        else if (status == EDITAR)
        {
            try
            {
                // ATUALIZA O OPERADOR
                await operadorService.PatchOperador(Operador);

                // ATUALIZA AS TELAS;
                foreach(var tela in OperadorTelas)
                {
                    await operadorService.PatchOperadorTelas(tela);
                }

                // MENSAGEM DE SUCESSO
                _toasterService.AddToast(Toast.NewToast("Atualizar Operador", $"Cadastro atualizado com sucesso!", MessageColour.Success, 8));

                // BUSCA AS NOVA INFORMAÇÕES
                getOperador(Operador.id);

                // ALTERA O STATUS
                validaStatus(VISUALIZAR);
            }
            catch (HttpRequestException ex)
            {
                _toasterService.AddToast(Toast.NewToast("Erro", $"Erro ao cadastrar o Operador: {ex.Message}", MessageColour.Danger, 8));
            }
            catch (Exception ex)
            {
                _toasterService.AddToast(Toast.NewToast("Erro", $"Erro ao cadastrar o Operador: {ex.Message}", MessageColour.Danger, 8));
            }
        }
    }

    private async void OnDeleteDialogClose(bool accepted)
    {
        try
        {
            if (accepted)
            {
                await operadorService.DeleteOperador(Operador.id);

                // MENSAGEM DE SUCESSO
                _toasterService.AddToast(Toast.NewToast("Operador Deletado", $"Cadastro deletado com sucesso!", MessageColour.Success, 8));
            }

            DeleteDialogOpen = false;
            StateHasChanged();

            // PEGA O ID DO ULTIMO OPERADOR CADASTRADO
            var ultimoId = await getLastOperador();

            // BUSCA AS INFORMAÇÕES DO ULTIMO OPERADOR CADASTRADO
            getOperador(ultimoId); 
        }
        catch (HttpRequestException ex)
        {
            _toasterService.AddToast(Toast.NewToast("Erro", $"Erro ao cadastrar o Operador: {ex.Message}", MessageColour.Danger, 8));
        }
        catch (Exception ex)
        {
            _toasterService.AddToast(Toast.NewToast("Erro", $"Erro ao cadastrar o Operador: {ex.Message}", MessageColour.Danger, 8));
        }

    }

    private void OpenDeleteDialog()
    {
        mensagem = $"Deseja realmente excluir o Operador {Operador.id} - {Operador.nome}";

        DeleteDialogOpen = true;
        StateHasChanged();
    }
}
