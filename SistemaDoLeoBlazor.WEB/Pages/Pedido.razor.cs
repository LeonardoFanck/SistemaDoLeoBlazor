using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using SistemaDoLeoBlazor.MODELS.OperadorDTOs;
using SistemaDoLeoBlazor.MODELS.PedidoDTO;
using SistemaDoLeoBlazor.MODELS.ProximoRegistroDTO;
using SistemaDoLeoBlazor.WEB.Toaster;
using SistemaDoLeoBlazor.MODELS.PedidoDTO;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Drawing;

namespace SistemaDoLeoBlazor.WEB.Pages
{
    public partial class Pedido
    {
        [Inject] private ToasterService? _toasterService { get; set; }
        [Inject] private NavigationManager? NavigationManager { get; set; }

        private PedidoDTO? pedido { get; set; }
        private PedidoDTO? pedidoAtual { get; set; }
        private IEnumerable<PedidoItemDTO>? itens { get; set; }

        // OPERADOR LOGADO NO MOMENTO
        private OperadorDTO? operadorLogado { get; set; }
        private OperadorTelaDTO? operadorLogadoTela { get; set; }

        // STATUS
        private bool stsCodPedido { get; set; }
        private bool stsBtnPesquisaPedido { get; set; }
        private bool stsTipoOperacao { get; set; }
        private bool stsData { get; set; }
        private bool stsCliente { get; set; }
        private bool stsBtnPesquisaCliente { get; set; }
        private bool stsClienteNome { get; set; } = true;
        private bool stsFormaPgto { get; set; }
        private bool stsBtnPesquisaFormaPgto { get; set; }
        private bool stsFormaPgtoNome { get; set; } = true;

        private bool stsBtnNovo { get; set; } = true;
        private bool stsBtnEditar { get; set; } = true;
        private bool stsBtnExcluir { get; set; } = true;
        private bool stsBtnCancelar { get; set; }
        private bool stsBtnSalvar { get; set; }

        private bool stsBtnNovoItem { get; set; } = true;
        private bool stsBtnEditarItem { get; set; } = true;
        private bool stsBtnExcluirItem { get; set; } = true;

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

        // ADD PRODUTO
        private bool produtoDialogOpen { get; set; }
        private string tipoOperacaoProd { get; set; }
        private string addProduto { get; set; } = "Adicionar";
        private string editProduto { get; set; } = "Alterar";
        private PedidoItemDTO item { get; set; }

        // TOAST
        private string toastTitulo { get; set; } = "Pedido";

        // TIPO OPERAÇÃO
        private string tipoVenda { get; } = "Venda";
        private string tipoCompra { get; } = "Compra";

        protected async override Task OnInitializedAsync()
        {
            // VERIFICA A SESSÃO DO OPERADOR LOGADO
            await getOperadorSession();
            await validarAcesso();

            // PEGA O ID DO ULTIMO OPERADOR CADASTRADO
            var ultimoId = await getLastRegistro();

            if (ultimoId == -1)
            {
                pedido = new PedidoDTO();

                validaStatus(CADASTRAR);
            }
            else
            {
                // PEGA AS INFORMAÇÕES DO ULTIMO OPERADOR
                await getRegistro(ultimoId);

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
                    operadorLogadoTela = operadorLogadoTelas.First(o => o.nome.Equals("Pedido"));
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
                var lista = await pedidoService.GetAll();

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

        private async Task getRegistro(int id)
        {
            try
            {
                pedidoAtual = await pedidoService.GetById(id);

                pedido = new PedidoDTO
                {
                    id = pedidoAtual.id,
                    clienteId = pedidoAtual.clienteId,
                    clienteNome = pedidoAtual.clienteNome,
                    formaPgtoId = pedidoAtual.formaPgtoId,
                    formaPgtoNome = pedidoAtual.formaPgtoNome,
                    data = pedidoAtual.data,
                    desconto = pedidoAtual.desconto,
                    total = pedidoAtual.total,
                    valor = pedidoAtual.valor,
                    tipoOperacao = pedidoAtual.tipoOperacao
                };

                await getItens(id);

                StateHasChanged();
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _toasterService.AddToast(Toast.NewToast(toastTitulo, $"Pedido {pedido.id} não cadastrado", MessageColour.Danger, 8));

                    resetaRegistro();

                    // RENDERIZA NOVAMENTE O COMPONENTE
                    StateHasChanged();
                }
            }
        }

        private async Task getItens(int id)
        {
            try {
                itens = await pedidoService.GetAllItens(id);
            }
            catch(Exception ex) {
                _toasterService.AddToast(Toast.NewToast(toastTitulo, $"Erro: {ex.Message}", MessageColour.Danger, 8));
            }
        }

        private async void validaStatus(int status)
        {
            if (status == CADASTRAR)
            {
                // false -> ATIVO | true -> INATIVO

                this.status = status;

                stsCodPedido = true;
                stsBtnPesquisaPedido = true;
                stsTipoOperacao = false;
                stsData = false;
                stsCliente = false;
                stsBtnPesquisaCliente = false;
                stsFormaPgto = false;
                stsBtnPesquisaFormaPgto = false;

                stsBtnNovo = true;
                stsBtnEditar = true;
                stsBtnExcluir = true;
                stsBtnCancelar = false;
                stsBtnSalvar = false;

                stsBtnNovoItem = true;
                stsBtnEditarItem = true;
                stsBtnExcluirItem = true;

                await pegarProximoRegistro();

                // LIMPA OS VALORES DO REGISTRO
                pedido.id = nextRegistro.pedido;
                pedido.clienteId = 0;
                pedido.clienteNome = string.Empty;
                pedido.formaPgtoId = 0;
                pedido.formaPgtoNome = string.Empty;
                pedido.data = DateTime.Today;
                pedido.desconto = decimal.Zero;
                pedido.total = decimal.Zero;
                pedido.valor = decimal.Zero;
                pedido.tipoOperacao = tipoVenda;

                StateHasChanged();
            }
            else if (status == EDITAR)
            {
                // false -> ATIVO | true -> INATIVO

                this.status = status;

                stsCodPedido = true;
                stsBtnPesquisaPedido = true;
                stsTipoOperacao = true;
                stsData = false;
                stsCliente = false;
                stsBtnPesquisaCliente = false;
                stsFormaPgto = false;
                stsBtnPesquisaFormaPgto = false;

                stsBtnNovo = true;
                stsBtnEditar = true;
                stsBtnExcluir = true;
                stsBtnCancelar = false;
                stsBtnSalvar = false;

                stsBtnNovoItem = false;
                stsBtnEditarItem = false;
                stsBtnExcluirItem = false;

                // RESTAURA AS INFORMAÇÕES DA CATEGORIA
                resetaRegistro();
            }
            else if (status == VISUALIZAR)
            {
                // false -> ATIVO | true -> INATIVO

                this.status = status;

                stsCodPedido = false;
                stsBtnPesquisaPedido = false;
                stsTipoOperacao = true;
                stsData = true;
                stsCliente = true;
                stsBtnPesquisaCliente = true;
                stsFormaPgto = true;
                stsBtnPesquisaFormaPgto = true;

                stsBtnCancelar = true;
                stsBtnSalvar = true;

                stsBtnNovoItem = true;
                stsBtnEditarItem = true;
                stsBtnExcluirItem = true;

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

                // SE FOR NULO É O PRIMEIRO REGISTRO
                if (pedidoAtual is not null)
                {
                    resetaRegistro();
                }
            }
        }

        private async Task getCliente()
        {
            try
            {
                var cliente = await clienteService.GetById(pedido.clienteId);

                pedido.clienteNome = cliente.nome;
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    pedido.clienteNome = "[CLIENTE NÃO LOCALIZADO]";
                }
            }
        }

        private async Task getFormaPgto()
        {
            try
            {
                var pgto = await pgtoService.GetById(pedido.formaPgtoId);

                pedido.formaPgtoNome = pgto.nome;
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    pedido.formaPgtoNome = "[FORMA DE PAGAMENTO NÃO LOCALIZADA]";
                }
            }
        }

        private async Task pegarProximoRegistro()
        {
            try
            {
                nextRegistro = await registroService.GetProximoRegistro();

                nextRegistro.pedido += 1;
            }
            catch (Exception ex)
            {
                _toasterService.AddToast(Toast.NewToast("ERRO", $"Ocorreu um erro: {ex.Message}", MessageColour.Danger, 8));
                throw;
            }
        }

        private void resetaRegistro()
        {
            if (pedidoAtual is not null)
            {
                pedido.id = pedidoAtual.id;
                pedido.clienteId = pedidoAtual.clienteId;
                pedido.clienteNome = pedidoAtual.clienteNome;
                pedido.formaPgtoId = pedidoAtual.formaPgtoId;
                pedido.formaPgtoNome = pedidoAtual.formaPgtoNome;
                pedido.data = pedidoAtual.data;
                pedido.desconto = pedidoAtual.desconto;
                pedido.total = pedidoAtual.total;
                pedido.valor = pedidoAtual.valor;
                pedido.tipoOperacao = pedidoAtual.tipoOperacao;
            }
            else
            {
                pedido.id = nextRegistro.pedido;
                pedido.clienteId = 0;
                pedido.clienteNome = string.Empty;
                pedido.formaPgtoId = 0;
                pedido.formaPgtoNome = string.Empty;
                pedido.data = DateTime.Today;
                pedido.desconto = decimal.Zero;
                pedido.total = decimal.Zero;
                pedido.valor = decimal.Zero;
                pedido.tipoOperacao = tipoVenda;
            }
        }

        private async void OnKeyDownTxtID(KeyboardEventArgs e)
        {
            if (e.Code == "Enter" || e.Code == "NumpadEnter")
            {
                await getRegistro(pedido.id);
            }
        }

        private async Task validarCampos()
        {
            if(pedido.clienteId == 0)
            {
                throw new FormatException("Campo Cliente não pode estar vazio");
            }
            else
            {
                await getCliente();

                if(pedido.clienteNome.Equals("[CLIENTE NÃO LOCALIZADO]") || pedido.clienteNome.Equals(""))
                {
                    throw new FormatException("Cliente inválido");
                }
            }

            if (pedido.formaPgtoId == 0)
            {
                throw new FormatException("Campo Forma Pagamento não pode estar vazio");
            }
            else
            {
                await getFormaPgto();

                if (pedido.formaPgtoNome.Equals("[FORMA DE PAGAMENTO NÃO LOCALIZADA]") || pedido.formaPgtoNome.Equals(""))
                {
                    throw new FormatException("Forma de Pagamento inválida");
                }
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
                    pedidoAtual = await pedidoService.Insert(pedido);

                    // MENSAGEM DE SUCESSO
                    _toasterService.AddToast(Toast.NewToast(toastTitulo, $"Cadastro Realizado com sucesso!", MessageColour.Success, 8));

                    // ATUALIZA O ULTIMO REGISTRO CADASTRADO
                    await registroService.PatchProximoRegistro(nextRegistro);

                    // ALTERA O STATUS
                    validaStatus(EDITAR);
                }
                catch (HttpRequestException ex)
                {
                    _toasterService.AddToast(Toast.NewToast("Erro", $"Erro ao cadastrar: {ex.Message}", MessageColour.Danger, 8));
                }
                catch (FormatException ex)
                {
                    _toasterService.AddToast(Toast.NewToast(toastTitulo, $"Erro ao cadastrar: {ex.Message}", MessageColour.Warning, 8));
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
                    await pedidoService.Update(pedido);

                    // MENSAGEM DE SUCESSO
                    _toasterService.AddToast(Toast.NewToast(toastTitulo, $"Cadastro atualizado com sucesso!", MessageColour.Success, 8));

                    // BUSCA AS NOVA INFORMAÇÕES
                    await getRegistro(pedido.id);

                    // ALTERA O STATUS
                    validaStatus(VISUALIZAR);
                }
                catch (HttpRequestException ex)
                {
                    _toasterService.AddToast(Toast.NewToast("Erro", $"Erro ao alterar: {ex.Message}", MessageColour.Danger, 8));
                }
                catch (FormatException ex)
                {
                    _toasterService.AddToast(Toast.NewToast(toastTitulo, $"Erro ao alterar: {ex.Message}", MessageColour.Warning, 8));
                }
                catch (Exception ex)
                {
                    _toasterService.AddToast(Toast.NewToast("Erro", $"Erro ao alterar: {ex.Message}", MessageColour.Danger, 8));
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
                    await pedidoService.Delete(pedido.id);

                    // MENSAGEM DE SUCESSO
                    _toasterService.AddToast(Toast.NewToast(toastTitulo, $"Cadastro deletado com sucesso!", MessageColour.Success, 8));
                }

                DeleteDialogOpen = false;
                StateHasChanged();

                // PEGA O ID DO ULTIMO REGISTRO
                var ultimoId = await getLastRegistro();

                // BUSCA AS INFORMAÇÕES DO ULTIMO REGISTRO
                await getRegistro(ultimoId);
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
            mensagem = $"Deseja realmente excluir o Pedido {pedido.id} ?";

            DeleteDialogOpen = true;
            StateHasChanged();
        }

        private void OpenAddProdutoDialog(string tipo)
        {
            tipoOperacaoProd = tipo;

            if (tipoOperacaoProd.Equals("Adicionar"))
            {
                item = new PedidoItemDTO
                {
                    desconto = decimal.Zero,
                    pedidoId = 0,
                    produtoId = 0,
                    produtoNome = "",
                    quantidade = 0,
                    id = 0,
                    total = decimal.Zero,
                    valor = decimal.Zero
                };
            }

            produtoDialogOpen = true;
            StateHasChanged();
        }

        private async void OnAddProdutoDialogClose(bool accepted)
        {
            try
            {
                if (accepted)
                {
                    await getItens(pedido.id);

                   // MENSAGEM DE SUCESSO
                    _toasterService.AddToast(Toast.NewToast(toastTitulo, $"Deuy tudo certo!", MessageColour.Success, 8));
                }

                produtoDialogOpen = false;
                StateHasChanged();
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
    }
}
