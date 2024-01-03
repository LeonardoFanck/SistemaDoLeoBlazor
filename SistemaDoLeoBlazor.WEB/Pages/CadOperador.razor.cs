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

    // ERRO MENSAGEM
    private string erro = "";

    protected override async Task OnInitializedAsync()
    {
        try
        {
            OperadorAtual = await operadorService.GetOperadorById(4);

            Operador = new OperadorDTO
            {
                id = OperadorAtual.id,
                nome = OperadorAtual.nome,
                senha = OperadorAtual.senha,
                admin = OperadorAtual.admin,
                inativo = OperadorAtual.inativo
            };

            OperadorTelas = await operadorService.GetTelasByOperador(4);

            validaStatus(VISUALIZAR);
        }
        catch (Exception e)
        {
            erro = $"{e.Message} - {e.GetType}";
        }
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
            Operador.nome = string.Empty;
            Operador.senha = string.Empty;
            Operador.admin = false;
            Operador.inativo = false;
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

            // RESETA OS VALORES DO OPERADOR
            resetarOperador();
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

    private async void OnKeyDownTxtID(KeyboardEventArgs e)
    {
        try
        {
            if (e.Code == "Enter" || e.Code == "NumpadEnter")
            {
                OperadorAtual = await operadorService.GetOperadorById(Operador.id);

                Operador = new OperadorDTO
                {
                    id = OperadorAtual.id,
                    nome = OperadorAtual.nome,
                    senha = OperadorAtual.senha,
                    admin = OperadorAtual.admin,
                    inativo = OperadorAtual.inativo
                };

                // RENDERIZA NOVAMENTE O COMPONENTE
                _ = InvokeAsync(StateHasChanged);
            }
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _toasterService.AddToast(Toast.NewToast("Operador inválido", $"Operador {Operador.id} não cadastrado", MessageColour.Danger, 6));
                
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
}
