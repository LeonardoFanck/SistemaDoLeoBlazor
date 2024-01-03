using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using SistemaDoLeoBlazor.MODELS.OperadorDTOs;
using SistemaDoLeoBlazor.WEB.Toaster;
using SistemaDoLeoBlazor.WEB.Services;

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

    protected override async Task OnInitializedAsync()
    {
        // DEPOIS VAI TER UM SELECT AONDE VAI PEGAR O PRIMEIRO REGISTRO
        getOperador(4); //   <-- ALTERAR DEPOIS

        validaStatus(VISUALIZAR);        
    }

    private void validaStatus(int status)
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

    private async void salvarCadastro()
    {
        if(status == CADASTRAR)
        {
            try
            {
                // ADD OPERADOR
                OperadorAtual = await operadorService.PostOperador(Operador);

                // ADD TELAS
                OperadorAtual = await operadorService.PostOperadorTelas(OperadorAtual);

                // PEGA O OPERADOR CADASTRADO PARA ATUALIZAR OS OBJETOS OPERADOR E TELA
                getOperador(OperadorAtual.id);

                foreach (var tela in OperadorTelas)
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

        }
    }
}
