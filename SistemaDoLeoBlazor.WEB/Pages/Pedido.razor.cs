using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using SistemaDoLeoBlazor.MODELS.OperadorDTOs;
using SistemaDoLeoBlazor.MODELS.PedidoDTO;
using SistemaDoLeoBlazor.MODELS.ProximoRegistroDTO;
using SistemaDoLeoBlazor.WEB.Toaster;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Drawing;
using SistemaDoLeoBlazor.WEB.Services.ProdutoService;
using SistemaDoLeoBlazor.MODELS.ClienteDTO;
using SistemaDoLeoBlazor.MODELS.FormaPgtoDTO;
using SistemaDoLeoBlazor.WEB.Relatorios;
using Microsoft.JSInterop;
using System;
using iTextSharp.text.rtf.document;

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
        private bool stsValor { get; set; } 
        private bool stsDesconto { get; set; }
        private bool stsTotal { get; set; } = true;

        private bool stsBtnNovo { get; set; } = true;
        private bool stsBtnEditar { get; set; } = true;
        private bool stsBtnExcluir { get; set; } = true;
        private bool stsBtnCancelar { get; set; }
        private bool stsBtnSalvar { get; set; }

        private bool stsBtnNovoItem { get; set; } = true;
        private bool stsBtnEditarItem { get; set; } = true;
        private bool stsBtnExcluirItem { get; set; } = true;
        private bool stsBtnAttValor { get; set; } = true;
        private bool stsBtnPdf { get; set; }

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
        private int tipoPesquisa { get; set; }
        private readonly int pesquisaPedido = 0;
        private readonly int pesquisaCliente = 1;
        private readonly int pesquisaPgto = 2;


        private int selecaoDelete { get; set; } = 0;
        private int PEDIDO = 1;
        private int ITEM = 2;

        // ADD PRODUTO
        private bool produtoDialogOpen { get; set; }
        private string tipoOperacaoProd { get; set; }
        private string addProduto { get; set; } = "Adicionar";
        private string editProduto { get; set; } = "Alterar";
        private PedidoItemDTO itemSelecionado { get; set; }

        // CLIENTE e PGTO
        private ClienteDTO cliente { get; set; }
        private FormaPgtoDTO pgto { get; set; }

        // TOAST
        private string toastTitulo { get; set; } = "Pedido";

        // TIPO OPERAÇÃO
        private string tipoVenda { get; } = "Venda";
        private string tipoCompra { get; } = "Compra";

        private string tipoCliente { get; set; }


        private void OpenNewTab(string filename)
        {
            var pdf = new PDFGenerator();
            pdf.gerarPedidoPDF(js, filename, pedido, itens.ToList());
        }

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
                pedido.tipoOperacao = tipoVenda;

                validaStatus(CADASTRAR);
            }
            else
            {
                // PEGA AS INFORMAÇÕES DO ULTIMO OPERADOR
                await getRegistro(ultimoId);

                // ALTERA O STATUS
                validaStatus(VISUALIZAR);
            }

            StateHasChanged();
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
                NavigationManager.NavigateTo("/Login", true); // ALTERAR PARA TELA DE LOGIN
            }
            else
            {
                if (operadorLogadoTela is not null && operadorLogadoTela.ativo == false)
                {
                    NavigationManager.NavigateTo("/", true);
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
                itemSelecionado = null;

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
                stsValor = true;
                stsDesconto = true;
                stsTotal = true;

                stsBtnNovo = true;
                stsBtnEditar = true;
                stsBtnExcluir = true;
                stsBtnCancelar = false;
                stsBtnSalvar = false;

                stsBtnNovoItem = true;
                stsBtnEditarItem = true;
                stsBtnExcluirItem = true;
                stsBtnAttValor = true;
                stsBtnPdf = true;

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

                // LIMPA A LISTA DE ITENS
                itens = null;

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
                stsValor = false;
                stsDesconto = false;
                stsTotal = false;

                stsBtnNovo = true;
                stsBtnEditar = true;
                stsBtnExcluir = true;
                stsBtnCancelar = false;
                stsBtnSalvar = false;

                stsBtnNovoItem = false;
                stsBtnEditarItem = false;
                stsBtnExcluirItem = false;
                stsBtnAttValor = false;
                stsBtnPdf = true;

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
                stsValor = true;
                stsDesconto = true;
                stsTotal = true;

                stsBtnCancelar = true;
                stsBtnSalvar = true;

                stsBtnNovoItem = true;
                stsBtnEditarItem = true;
                stsBtnExcluirItem = true;
                stsBtnAttValor = true;
                stsBtnPdf = false;

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
                cliente = await clienteService.GetById(pedido.clienteId);

                if(cliente.inativo)
                {
                    pedido.clienteNome = "[Cliente Inativo]";
                }
                else if(pedido.tipoOperacao.Equals(tipoVenda) && cliente.tipoCliente == false 
                    || pedido.tipoOperacao.Equals(tipoCompra) && cliente.tipoFornecedor == false)
                {
                    pedido.clienteNome = $"[Tipo inválido para a Operação {pedido.tipoOperacao}]";
                }
                else
                {
                    pedido.clienteNome = cliente.nome;
                }
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
                pgto = await pgtoService.GetById(pedido.formaPgtoId);

                if (pgto.inativo)
                {
                    pedido.formaPgtoNome = "[Forma de Pagamento Inativa]";
                }
                else
                {
                    pedido.formaPgtoNome = pgto.nome;
                }
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

        private async void resetaRegistro()
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

                await getItens(pedidoAtual.id);

                StateHasChanged();
            }
            else
            {
                pedido.id = 1;
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

        private void resetarValorPedido()
        {
            pedido.valor = itens.Sum(i => i.total);

            _toasterService.AddToast(Toast.NewToast(toastTitulo, $"Valor do Pedido Atualizado", MessageColour.Info, 8));
        }

        private async void OnKeyDownTxtID(KeyboardEventArgs e)
        {
            if (e.Code == "Enter" || e.Code == "NumpadEnter")
            {
                await getRegistro(pedido.id);
            }
            else if(e.Code == "F4")
            {
                openPesquisaDialog();
            }
        }
        
        private async void OnKeyDownTxtCliente(KeyboardEventArgs e)
        {
            if(e.Code == "F4")
            {
                openPesquisaDialogCliente();
            }
        }
        
        private async void OnKeyDownTxtPgto(KeyboardEventArgs e)
        {
            if(e.Code == "F4")
            {
                openPesquisaDialogPgto();
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

                if(!pedido.clienteNome.Equals(cliente.nome) || pedido.clienteNome.Equals(""))
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

                if (!pedido.formaPgtoNome.Equals(pgto.nome) || pedido.formaPgtoNome.Equals(""))
                {
                    throw new FormatException("Forma de Pagamento inválida");
                }
            }
        }

        private async void salvarCadastro()
        {
            await recalcularValores();

            if (status == CADASTRAR)
            {
                try
                {
                    await validarCampos();

                    // ADD OPERADOR
                    pedidoAtual = await pedidoService.Insert(pedido);

                        // MENSAGEM DE SUCESSO
                    if(itens is not null && pedido.valor != itens.Sum(i => i.total))
                    {
                        _toasterService.AddToast(Toast.NewToast(toastTitulo, $"Cadastro Realizado com Valor do Pedido diferente do Total de Itens", MessageColour.Warning, 8));
                    }
                    else
                    {
                        _toasterService.AddToast(Toast.NewToast(toastTitulo, $"Cadastro Realizado com sucesso!", MessageColour.Success, 8));
                    }

                    // ATUALIZA O ULTIMO REGISTRO CADASTRADO
                    await registroService.PatchProximoRegistro(nextRegistro);

                    await getRegistro(pedidoAtual.id);

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
                    await validarCampos();

                    // ATUALIZA O OPERADOR
                    await pedidoService.Update(pedido);

                    // MENSAGEM DE SUCESSO
                    if (pedido.valor != itens.Sum(i => i.total))
                    {
                        _toasterService.AddToast(Toast.NewToast(toastTitulo, $"Cadastro Atualizado com Valor do Pedido diferente do Total de Itens", MessageColour.Warning, 8));
                    }
                    else
                    {
                        _toasterService.AddToast(Toast.NewToast(toastTitulo, $"Cadastro atualizado com sucesso!", MessageColour.Success, 8));
                    }
                    

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

        private async Task recalcularValores()
        {
            var valor = pedido.valor;

            if (pedido.desconto > 100)
            {
                pedido.desconto = 100;
            }
            else if(pedido.desconto < 0)
            {
                pedido.desconto = 0;
            }

            var desconto = pedido.desconto / 100;

            var total = valor - (valor * desconto);

            pedido.total = total;
        }

        private async void OnDeleteDialogClose(bool accepted)
        {
            try
            {
                if (accepted)
                {
                    if (selecaoDelete == PEDIDO)
                    {
                        await pedidoService.Delete(pedido.id);

                        // MENSAGEM DE SUCESSO
                        _toasterService.AddToast(Toast.NewToast(toastTitulo, $"Cadastro deletado com sucesso!", MessageColour.Success, 8));

                        var id = await getLastRegistro();

                        await getRegistro(id);
                    }
                    else if (selecaoDelete == ITEM)
                    {
                        await pedidoService.DeleteItem(itemSelecionado.id);

                        // MENSAGEM DE SUCESSO
                        _toasterService.AddToast(Toast.NewToast(toastTitulo, $"Item deletado com sucesso!", MessageColour.Success, 8));

                        await getItens(pedido.id);
                    }
                }

                DeleteDialogOpen = false;
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

        private void OpenDeleteDialog(int selecao)
        {
            if(selecao == PEDIDO)
            {
                selecaoDelete = selecao;
                mensagem = $"Deseja realmente excluir o Pedido {pedido.id} ?";
            }
            else if(selecao == ITEM)
            {
                selecaoDelete = selecao;
                mensagem = $"Deseja relamente excluir o Produto {itemSelecionado.produtoId} - {itemSelecionado.produtoNome} ?";
            }

            DeleteDialogOpen = true;
            StateHasChanged();
        }

        private void OpenAddProdutoDialog(string tipo)
        {
            tipoOperacaoProd = tipo;

            produtoDialogOpen = true;
            StateHasChanged();
        }

        private void selectItem(PedidoItemDTO item)
        {
            if (status.Equals(EDITAR))
            {
                itemSelecionado = item;
            }
        }

        private void adicionarItem()
        {
            itemSelecionado = new PedidoItemDTO
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

            OpenAddProdutoDialog(addProduto);
        }

        private async Task atualizaTotalPedido()
        {
            var total = itens.Sum(i => i.total);

            pedido.valor = total;

            await recalcularValores();
        }

        private void editarItem()
        {
            if (itemSelecionado is null)
            {
                _toasterService.AddToast(Toast.NewToast(toastTitulo, $"Necessário selecionar um Item!", MessageColour.Warning, 8));
            }
            else
            {
                OpenAddProdutoDialog(editProduto);
            }
        }

        private void deletarItem()
        {
            if (itemSelecionado is null)
            {
                _toasterService.AddToast(Toast.NewToast(toastTitulo, $"Necessário selecionar um Item!", MessageColour.Warning, 8));
            }
            else
            {
                OpenDeleteDialog(ITEM);
            }
        }

        private async void OnAddProdutoDialogClose(bool accepted)
        {
            try
            {
                if (accepted)
                {
                    await getItens(pedido.id);

                    if (tipoOperacaoProd.Equals(addProduto))
                    {
                        // MENSAGEM DE SUCESSO
                        _toasterService.AddToast(Toast.NewToast(toastTitulo, $"Produto Adicionado com sucesso!", MessageColour.Success, 8));
                    }
                    else
                    {
                        // MENSAGEM DE SUCESSO
                        _toasterService.AddToast(Toast.NewToast(toastTitulo, $"Produto Alterado com sucesso!", MessageColour.Success, 8));
                    }

                    await atualizaTotalPedido();
                }
                
                // RESETA O ITEM SELECIONADO
                itemSelecionado = null;

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

        private void openPesquisaDialog()
        {
            tipoPesquisa = pesquisaPedido;
            pesquisaInativos = true;
            pesquisaDialogOpen = true;
            StateHasChanged();
        }

        private void openPesquisaDialogCliente()
        {
            if (pedido.tipoOperacao.Equals(tipoVenda))
            {
                tipoCliente = "Cliente";
            }
            else
            {
                tipoCliente = "Fornecedor";
            }

            tipoPesquisa = pesquisaCliente;
            pesquisaInativos = false;
            pesquisaDialogOpen = true;
            StateHasChanged();
        }

        private void openPesquisaDialogPgto()
        {
            tipoPesquisa = pesquisaPgto;
            pesquisaInativos = false;
            pesquisaDialogOpen = true;
            StateHasChanged();
        }

        private async void onPesquisaDialogClose(int id)
        {
            if (id != -1)
            {
                if(tipoPesquisa == pesquisaPedido)
                {
                    await getRegistro(id);
                }
                else if(tipoPesquisa == pesquisaCliente)
                {
                    pedido.clienteId = id;
                    await getCliente();
                }
                else if(tipoPesquisa == pesquisaPgto)
                {
                    pedido.formaPgtoId = id;
                    await getFormaPgto();
                }
            }

            pesquisaDialogOpen = false;

            StateHasChanged();
        }
    }
}
