using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using SistemaDoLeoBlazor.MODELS.ClienteDTO;
using SistemaDoLeoBlazor.MODELS.OperadorDTOs;
using SistemaDoLeoBlazor.MODELS.ProximoRegistroDTO;
using SistemaDoLeoBlazor.WEB.Services.CLienteService;
using SistemaDoLeoBlazor.WEB.Toaster;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.ConstrainedExecution;
using System.Runtime.CompilerServices;

namespace SistemaDoLeoBlazor.WEB.Pages
{
    public partial class CadCliente
    {
        [Inject] private ToasterService? _toasterService { get; set; }
        [Inject] private NavigationManager? NavigationManager { get; set; }

        private ClienteDTO? cliente { get; set; }
        private ClienteDTO? clienteAtual { get; set; }

        // OPERADOR LOGADO NO MOMENTO
        private OperadorDTO? operadorLogado { get; set; }
        private OperadorTelaDTO? operadorLogadoTela { get; set; }

        // STATUS
        private bool stsCodCliente { get; set; }
        private bool stsBtnPesquisa { get; set; }
        private bool stsTipoCliente { get; set; }
        private bool stsTipoFornecedor { get; set; }
        private bool stsInativo { get; set; }
        private bool stsNome { get; set; }
        private bool stsTipoDoc {  get; set; }
        private bool stsDocumento {  get; set; }
        private bool stsCep {  get; set; }
        private bool stsUF {  get; set; }
        private bool stsCidade {  get; set; }
        private bool stsBairro {  get; set; }
        private bool stsEndereco {  get; set; }
        private bool stsNumero {  get; set; }
        private bool stsComplemento {  get; set; }

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

        private bool pesquisaDialogOpen { get; set; }
        private bool pesquisaInativos { get; set; }

        // TITULO TOAST
        private string toastTitulo { get; set; } = "Cliente";

        // TIPO DOCUMENTO
        private string tipoDoc { get; set; } = "CPF";
        private int tamanhoDoc { get; set; } = 14;

        protected async override Task OnInitializedAsync()
        {
            // VERIFICA A SESSÃO DO OPERADOR LOGADO
            await getOperadorSession();
            await validarAcesso();

            // PEGA O ID DO ULTIMO OPERADOR CADASTRADO
            var ultimoId = await getLastRegistro();

            if (ultimoId == -1)
            {
                cliente = new ClienteDTO();

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
                operadorLogado = await session.GetSessaoOperador();

                var operadorLogadoTelas = await session.GetSessaoTelas();

                if (operadorLogadoTelas is not null)
                {
                    operadorLogadoTela = operadorLogadoTelas.FirstOrDefault(t => t.nome.Equals("Cliente"));
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
                var lista = await clienteService.GetAll();

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

        private void formataDocumento(ChangeEventArgs e)
        {
            if (tipoDoc.Equals("CPF"))
            {
                tamanhoDoc = 14;

                // Remove caracteres não numéricos
                var cleanedInput = new string(e.Value.ToString().Where(char.IsDigit).ToArray());

                // Formata o CPF
                if (cleanedInput.Length >= 3)
                {
                    cleanedInput = $"{cleanedInput.Substring(0, 3)}.{cleanedInput.Substring(3)}";
                }
                if (cleanedInput.Length >= 7)
                {
                    cleanedInput = $"{cleanedInput.Substring(0, 7)}.{cleanedInput.Substring(7)}";
                }
                if (cleanedInput.Length >= 11)
                {
                    cleanedInput = $"{cleanedInput.Substring(0, 11)}-{cleanedInput.Substring(11)}";
                }

                cliente.documento = cleanedInput;
            }
            else
            {
                tamanhoDoc = 18;

                // Remove caracteres não numéricos
                var cleanedInput = new string(e.Value.ToString().Where(char.IsDigit).ToArray());

                // Formata o CNPJ
                if (cleanedInput.Length >= 2)
                {
                    cleanedInput = $"{cleanedInput.Substring(0, 2)}.{cleanedInput.Substring(2)}";
                }
                if (cleanedInput.Length >= 6)
                {
                    cleanedInput = $"{cleanedInput.Substring(0, 6)}.{cleanedInput.Substring(6)}";
                }
                if (cleanedInput.Length >= 10)
                {
                    cleanedInput = $"{cleanedInput.Substring(0, 10)}/{cleanedInput.Substring(10)}";
                }
                if (cleanedInput.Length >= 15)
                {
                    cleanedInput = $"{cleanedInput.Substring(0, 15)}-{cleanedInput.Substring(15)}";
                }

                cliente.documento = cleanedInput;
            }
        }

        private void formataCep(ChangeEventArgs e)
        {
            // Remove caracteres não numéricos
            var cleanedInput = new string(e.Value.ToString().Where(char.IsDigit).ToArray());

            // Formata o CEP
            if (cleanedInput.Length >= 5)
            {
                cleanedInput = $"{cleanedInput.Substring(0, 5)}-{cleanedInput.Substring(5)}";
            }

            cliente.cep = cleanedInput;
        
        }

        private async void getRegistro(int id)
        {
            try
            {
                clienteAtual = await clienteService.GetById(id);

                cliente = new ClienteDTO
                {
                    id = clienteAtual.id,
                    nome = clienteAtual.nome,
                    inativo = clienteAtual.inativo,
                    bairro = clienteAtual.bairro,
                    cep = clienteAtual.cep,
                    cidade = clienteAtual.cidade,
                    complemento = clienteAtual.complemento,
                    documento = clienteAtual.documento,
                    dtNasc = clienteAtual.dtNasc,
                    endereco = clienteAtual.endereco,
                    numero = clienteAtual.numero,
                    tipoCliente = clienteAtual.tipoCliente,
                    tipoFornecedor = clienteAtual.tipoFornecedor,
                    uf = clienteAtual.uf
                };

                await validarTipoDoc();

                StateHasChanged();
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _toasterService.AddToast(Toast.NewToast(toastTitulo, $"Cliente {cliente.id} não cadastrado", MessageColour.Danger, 8));

                    resetaRegistro();

                    // RENDERIZA NOVAMENTE O COMPONENTE
                    StateHasChanged();
                }
            }
        }

        private async Task validarTipoDoc()
        {
            if (cliente.documento.Count() == 14)
            {
                tipoDoc = "CPF";
            }
            else
            {
                tipoDoc = "CNPJ";
            }
        }

        private async void validaStatus(int status)
        {
            if (status == CADASTRAR)
            {
                // false -> ATIVO | true -> INATIVO

                this.status = status;

                stsCodCliente = true;
                stsBtnPesquisa = true;
                stsTipoCliente = false;
                stsTipoFornecedor = false;
                stsInativo = false;
                stsNome = false;
                stsTipoDoc = false;
                stsDocumento = false;
                stsCep = false;
                stsUF = false;
                stsCidade = false;
                stsBairro = false;
                stsEndereco = false;
                stsNumero = false;
                stsComplemento = false;

                stsBtnNovo = true;
                stsBtnEditar = true;
                stsBtnExcluir = true;
                stsBtnCancelar = false;
                stsBtnSalvar = false;

                await pegarProximoRegistro();

                // LIMPA OS VALORES DO OPERADOR
                cliente.id = nextRegistro.cliente;
                cliente.nome = string.Empty;
                cliente.inativo = false;
                cliente.bairro = string.Empty;
                cliente.cep = string.Empty;
                cliente.cidade = string.Empty;
                cliente.complemento = string.Empty;
                cliente.documento = string.Empty;
                cliente.dtNasc = DateTime.Today;
                cliente.endereco = string.Empty;
                cliente.numero = string.Empty;
                cliente.tipoCliente = false;
                cliente.tipoFornecedor = false;
                cliente.uf = string.Empty;

                tipoDoc = "CPF";

                StateHasChanged();
            }
            else if (status == EDITAR)
            {
                // false -> ATIVO | true -> INATIVO

                this.status = status;

                stsCodCliente = true;
                stsBtnPesquisa = true;
                stsTipoCliente = false;
                stsTipoFornecedor = false;
                stsInativo = false;
                stsNome = false;
                stsTipoDoc = false;
                stsDocumento = false;
                stsCep = false;
                stsUF = false;
                stsCidade = false;
                stsBairro = false;
                stsEndereco = false;
                stsNumero = false;
                stsComplemento = false;

                stsBtnNovo = true;
                stsBtnEditar = true;
                stsBtnExcluir = true;
                stsBtnCancelar = false;
                stsBtnSalvar = false;

                // RESTAURA AS INFORMAÇÕES DA cliente
                resetaRegistro();
            }
            else if (status == VISUALIZAR)
            {
                // false -> ATIVO | true -> INATIVO

                this.status = status;

                stsCodCliente = false;
                stsBtnPesquisa = false;
                stsTipoCliente = true;
                stsTipoFornecedor = true;
                stsInativo = true;
                stsNome = true;
                stsTipoDoc = true;
                stsDocumento = true;
                stsCep = true;
                stsUF = true;
                stsCidade = true;
                stsBairro = true;
                stsEndereco = true;
                stsNumero = true;
                stsComplemento = true;

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
                if (clienteAtual is not null)
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

                nextRegistro.cliente += 1;
            }
            catch (Exception ex)
            {
                _toasterService.AddToast(Toast.NewToast("ERRO", $"Ocorreu um erro: {ex.Message}", MessageColour.Danger, 8));
                throw;
            }
        }

        private void resetaRegistro()
        {
            if (clienteAtual is not null)
            {
                cliente.id = clienteAtual.id;
                cliente.nome = clienteAtual.nome;
                cliente.inativo = clienteAtual.inativo;
                cliente.bairro = clienteAtual.bairro;
                cliente.cep = clienteAtual.cep;
                cliente.cidade = clienteAtual.cidade;
                cliente.complemento = clienteAtual.complemento;
                cliente.documento = clienteAtual.documento;
                cliente.dtNasc = clienteAtual.dtNasc;
                cliente.endereco = clienteAtual.endereco;
                cliente.numero = clienteAtual.numero;
                cliente.tipoCliente = clienteAtual.tipoCliente;
                cliente.tipoFornecedor = clienteAtual.tipoFornecedor;
                cliente.uf = clienteAtual.uf;

                validarTipoDoc();
            }
            else
            {
                cliente.id = nextRegistro.cliente;
                cliente.nome = string.Empty;
                cliente.inativo = false;
                cliente.bairro = string.Empty;
                cliente.cep = string.Empty;
                cliente.cidade = string.Empty;
                cliente.complemento = string.Empty;
                cliente.documento = string.Empty;
                cliente.dtNasc = DateTime.Today;
                cliente.endereco = string.Empty;
                cliente.numero = string.Empty;
                cliente.tipoCliente = false;
                cliente.tipoFornecedor = false;
                cliente.uf = string.Empty;

                validarTipoDoc();
            }
        }

        private void OnKeyDownTxtID(KeyboardEventArgs e)
        {
            if (e.Code == "Enter" || e.Code == "NumpadEnter")
            {
                getRegistro(cliente.id);
            }
        }

        private async Task validarCampos()
        {
            if(tipoDoc.Equals("CPF") && cliente.documento.Count() != 14)
            {
                throw new FormatException("Documento com tamanho inválido!");
            }

            if (tipoDoc.Equals("CNPJ") && cliente.documento.Count() != 18)
            {
                throw new FormatException("Documento com tamanho inválido!");
            }

            if(cliente.cep.Count() != 9)
            {
                throw new FormatException("CEP com tamanho inválido!");
            }

            if (cliente.tipoCliente == false && cliente.tipoFornecedor == false)
            {
                throw new FormatException("Necessário selecionar ao menos um tipo para o Clinte!");
            }
        }

        private async void salvarCadastro()
        {
            if (status == CADASTRAR)
            {
                try
                {
                    await validarCampos();

                    // ADD OPERADOR
                    clienteAtual = await clienteService.Insert(cliente);

                    // MENSAGEM DE SUCESSO
                    _toasterService.AddToast(Toast.NewToast(toastTitulo, $"Cadastro Realizdo com sucesso!", MessageColour.Success, 8));

                    // ATUALIZA O ULTIMO REGISTRO CADASTRADO
                    await registroService.PatchProximoRegistro(nextRegistro);

                    // ALTERA O STATUS
                    validaStatus(VISUALIZAR);
                }
                catch (HttpRequestException ex)
                {
                    _toasterService.AddToast(Toast.NewToast("Erro", $"Erro ao cadastrar o cliente: {ex.Message}", MessageColour.Danger, 8));
                }
                catch (FormatException ex)
                {
                    _toasterService.AddToast(Toast.NewToast(toastTitulo, $"Erro ao cadastrar o cliente: {ex.Message}", MessageColour.Warning, 8));
                }
                catch (Exception ex)
                {
                    _toasterService.AddToast(Toast.NewToast("Erro", $"Erro ao cadastrar o cliente: {ex.Message}", MessageColour.Danger, 8));
                }
            }
            else if (status == EDITAR)
            {
                try
                {
                    await validarCampos();

                    // ATUALIZA O OPERADOR
                    await clienteService.Update(cliente);

                    // MENSAGEM DE SUCESSO
                    _toasterService.AddToast(Toast.NewToast(toastTitulo, $"Cadastro atualizado com sucesso!", MessageColour.Success, 8));

                    // BUSCA AS NOVA INFORMAÇÕES
                    getRegistro(cliente.id);

                    // ALTERA O STATUS
                    validaStatus(VISUALIZAR);
                }
                catch (HttpRequestException ex)
                {
                    _toasterService.AddToast(Toast.NewToast("Erro", $"Erro ao cadastrar o cliente: {ex.Message}", MessageColour.Danger, 8));
                }
                catch (FormatException ex)
                {
                    _toasterService.AddToast(Toast.NewToast(toastTitulo, $"Erro ao cadastrar o cliente: {ex.Message}", MessageColour.Warning, 8));
                }
                catch (Exception ex)
                {
                    _toasterService.AddToast(Toast.NewToast("Erro", $"Erro ao cadastrar o cliente: {ex.Message}", MessageColour.Danger, 8));
                }
            }

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

        private async void OnDeleteDialogClose(bool accepted)
        {
            try
            {
                if (accepted)
                {
                    await clienteService.Delete(cliente.id);

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
                _toasterService.AddToast(Toast.NewToast("Erro", $"Erro ao cadastrar o cliente: {ex.Message}", MessageColour.Danger, 8));
            }
            catch (Exception ex)
            {
                _toasterService.AddToast(Toast.NewToast("Erro", $"Erro ao cadastrar o cliente: {ex.Message}", MessageColour.Danger, 8));
            }
        }

        private void OpenDeleteDialog()
        {
            mensagem = $"Deseja realmente excluir a cliente {cliente.id} - {cliente.nome} ?";

            DeleteDialogOpen = true;
            StateHasChanged();
        }

        private void openPesquisaDialog()
        {
            pesquisaInativos = true;
            pesquisaDialogOpen = true;
            StateHasChanged();
        }
    }
}
