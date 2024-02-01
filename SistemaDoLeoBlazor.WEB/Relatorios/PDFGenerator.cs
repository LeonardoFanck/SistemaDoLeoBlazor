using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.JSInterop;
using SistemaDoLeoBlazor.MODELS.PedidoDTO;

namespace SistemaDoLeoBlazor.WEB.Relatorios
{
    public class PDFGenerator
    {
        private PedidoDTO pedido;
        private List<PedidoItemDTO> itens;

        public async void gerarPedidoPDF(IJSRuntime js, string filename, PedidoDTO pedido, List<PedidoItemDTO> itens)
        {
            this.pedido = pedido;
            this.itens = itens;

            await js.InvokeVoidAsync("OpenPdfNewTab", filename, Convert.ToBase64String(PDFReport()));   
        }

        public async void gerarRelatorioPedidos(IJSRuntime js, string filename, List<PedidoDTO> listaPedidos, string tipoOperacao)
        {
            await js.InvokeVoidAsync("OpenPdfNewTab", filename, Convert.ToBase64String(relatorioPedidos(listaPedidos, tipoOperacao)));
        }

        // itextShar Report
        private byte[] PDFReport()
        {
            var memoryStream = new MemoryStream();
            float margeLeft = 1.5f;
            float margeRight = 1.5f;
            float margeTop = 2.0f;
            float margeBottom = 1.0f;

            Document pdf = new Document(
                PageSize.A4,
                margeLeft.ToDpi(),
                margeRight.ToDpi(),
                margeTop.ToDpi(),
                margeBottom.ToDpi()
                );

            pdf.AddTitle("Sistema do Leo - Pedido");
            pdf.AddAuthor("Leonardo da Silva Fanck");
            pdf.AddCreationDate();
            pdf.AddKeywords("SistemaDoLeo");
            pdf.AddSubject("Blazor PDF");

            PdfWriter writer = PdfWriter.GetInstance(pdf, memoryStream);

            // CRIAR CABEÇALHO
            var fontStyle = FontFactory.GetFont("Arial", 26, BaseColor.White);
            var labelHeader = new Chunk($"Pedido de {pedido.tipoOperacao}", fontStyle);

            HeaderFooter header = new HeaderFooter(new Phrase(labelHeader), false)
            {
                BackgroundColor = new BaseColor(50, 20, 120),
                Alignment = Element.ALIGN_CENTER,
                Border = Rectangle.NO_BORDER
            };

            pdf.Header = header;

            // CRIANDO RODA PE
            fontStyle = FontFactory.GetFont("Arial", 13, BaseColor.White);
            var labelFooter = new Chunk($"Página: ", fontStyle);

            HeaderFooter footer = new HeaderFooter(new Phrase(labelFooter), true)
            {
                BackgroundColor = new BaseColor(120, 3, 120),
                Alignment = Element.ALIGN_CENTER,
                Border = Rectangle.NO_BORDER
            };
            pdf.Footer = footer;

            // CORPO
            pdf.Open();

            // Criar uma tabela com duas colunas
            PdfPTable infoTable = new PdfPTable(4);
            infoTable.WidthPercentage = 100;
            infoTable.SpacingBefore = 10f;
            infoTable.SpacingAfter = 20f;
            //infoTable.DefaultCell.Border = PdfCell.NO_BORDER;

            float tamanho = 13f;

            // Adicionar campos desejados (em duas colunas)
            infoTable.AddCell(new PdfPCell(new Phrase("Pedido:", new Font(Font.HELVETICA, tamanho, Font.BOLD))) { Border = PdfPCell.NO_BORDER, PaddingLeft = 40f });
            infoTable.AddCell(new PdfPCell(new Phrase($"{pedido.id}", new Font(Font.HELVETICA, tamanho))) { Border = PdfPCell.NO_BORDER, PaddingLeft = -30f });
            infoTable.AddCell(new PdfPCell(new Phrase("Valor:", new Font(Font.HELVETICA, tamanho, Font.BOLD))) { Border = PdfPCell.NO_BORDER, PaddingLeft = 40f });
            infoTable.AddCell(new PdfPCell(new Phrase($"R$ {pedido.valor}", new Font(Font.HELVETICA, tamanho))) { Border = PdfPCell.NO_BORDER, PaddingLeft = -20f });
            if (pedido.tipoOperacao.Equals("Venda"))
            {
                infoTable.AddCell(new PdfPCell(new Phrase("Cliente:", new Font(Font.HELVETICA, tamanho, Font.BOLD))) { Border = PdfPCell.NO_BORDER, PaddingLeft = 40f, PaddingTop = 7f });
                infoTable.AddCell(new PdfPCell(new Phrase(pedido.clienteNome, new Font(Font.HELVETICA, tamanho))) { Border = PdfPCell.NO_BORDER, PaddingLeft = -30f, PaddingTop = 8f });
            }
            else
            {
                infoTable.AddCell(new PdfPCell(new Phrase("Fornecedor:", new Font(Font.HELVETICA, tamanho, Font.BOLD))) { Border = PdfPCell.NO_BORDER, PaddingLeft = 40f, PaddingTop = 7f });
                infoTable.AddCell(new PdfPCell(new Phrase(pedido.clienteNome, new Font(Font.HELVETICA, tamanho))) { Border = PdfPCell.NO_BORDER, PaddingLeft = -10f, PaddingTop = 8f });
            }
            infoTable.AddCell(new PdfPCell(new Phrase("Desconto:", new Font(Font.HELVETICA, tamanho, Font.BOLD))) { Border = PdfPCell.NO_BORDER, PaddingLeft = 40f, PaddingTop = 7f });
            infoTable.AddCell(new PdfPCell(new Phrase($"{pedido.desconto}%", new Font(Font.HELVETICA, tamanho))) { Border = PdfPCell.NO_BORDER, PaddingLeft = -21f, PaddingTop = 7f });
            infoTable.AddCell(new PdfPCell(new Phrase("Forma de Pagamento:", new Font(Font.HELVETICA, tamanho, Font.BOLD))) { Border = PdfPCell.NO_BORDER, PaddingLeft = 40f, PaddingTop = 7f });
            infoTable.AddCell(new PdfPCell(new Phrase(pedido.formaPgtoNome, new Font(Font.HELVETICA, tamanho))) { Border = PdfPCell.NO_BORDER, PaddingLeft = -10f, PaddingTop = 9f });
            infoTable.AddCell(new PdfPCell(new Phrase("Total:", new Font(Font.HELVETICA, tamanho, Font.BOLD))) { Border = PdfPCell.NO_BORDER, PaddingLeft = 40f, PaddingTop = 7f });
            infoTable.AddCell(new PdfPCell(new Phrase($"R$ {pedido.total}", new Font(Font.HELVETICA, tamanho))) { Border = PdfPCell.NO_BORDER, PaddingLeft = -20f, PaddingTop = 7f });

            // Adicionar a tabela ao documento
            pdf.Add(infoTable);

            // ITENS
            PdfPTable table = new PdfPTable(6);
            table.WidthPercentage = 100;
            float[] tamanhoColunas = { 1f, 5f, 3f, 2.3f, 2.3f, 3f };
            table.SetWidths(tamanhoColunas);

            table.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

            table.AddCell(new PdfPCell(new Phrase("Cod", new Font(Font.HELVETICA, 12f, Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase("Produto", new Font(Font.HELVETICA, 12f, Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase("Valor", new Font(Font.HELVETICA, 12f, Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase("Quantidade", new Font(Font.HELVETICA, 12f, Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase("Desconto", new Font(Font.HELVETICA, 12f, Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase("Total", new Font(Font.HELVETICA, 12f, Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });

            // Adicionar linhas de dados (exemplo, ajuste conforme necessário)
            foreach (var item in itens)
            {
                table.AddCell($"{item.produtoId}");
                table.AddCell(new PdfPCell(new Phrase($"{item.produtoNome}")) { HorizontalAlignment = Element.ALIGN_LEFT });
                table.AddCell($"R$ {item.valor}");
                table.AddCell($"{item.quantidade}");
                table.AddCell($"{item.desconto}%");
                table.AddCell($"R$ {item.total}");
            }
            pdf.Add(table);

            pdf.Close();

            return memoryStream.ToArray();
        }


        private byte[] relatorioPedidos(List<PedidoDTO> listaPedidos, string tipoOperacao)
        {
            var memoryStream = new MemoryStream();
            float margeLeft = 1.5f;
            float margeRight = 1.5f;
            float margeTop = 2.0f;
            float margeBottom = 1.0f;

            Document pdf = new Document(
                PageSize.A4,
                margeLeft.ToDpi(),
                margeRight.ToDpi(),
                margeTop.ToDpi(),
                margeBottom.ToDpi()
                );

            pdf.AddTitle("Sistema do Leo - Relatório de Pedidos");
            pdf.AddAuthor("Leonardo da Silva Fanck");
            pdf.AddCreationDate();
            pdf.AddKeywords("SistemaDoLeo");
            pdf.AddSubject("Blazor PDF");

            PdfWriter writer = PdfWriter.GetInstance(pdf, memoryStream);

            // CRIAR CABEÇALHO
            var fontStyle = FontFactory.GetFont("Arial", 26, BaseColor.White);
            var labelHeader = new Chunk($"Relatório de Pedidos de {tipoOperacao}", fontStyle);

            HeaderFooter header = new HeaderFooter(new Phrase(labelHeader), false)
            {
                BackgroundColor = new BaseColor(50, 20, 120),
                Alignment = Element.ALIGN_CENTER,
                Border = Rectangle.NO_BORDER
            };

            pdf.Header = header;

            // CRIANDO RODA PE
            fontStyle = FontFactory.GetFont("Arial", 13, BaseColor.White);
            var labelFooter = new Chunk($"Página: ", fontStyle);

            HeaderFooter footer = new HeaderFooter(new Phrase(labelFooter), true)
            {
                BackgroundColor = new BaseColor(120, 3, 120),
                Alignment = Element.ALIGN_CENTER,
                Border = Rectangle.NO_BORDER
            };
            pdf.Footer = footer;

            // CORPO
            pdf.Open();

            // GRID
            PdfPTable table = new PdfPTable(6);
            table.WidthPercentage = 100;
            float[] tamanhoColunas = { 1.55f, 5f, 3f, 2.3f, 3f, 2.3f };
            table.SetWidths(tamanhoColunas);

            table.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

            table.AddCell(new PdfPCell(new Phrase("Pedido", new Font(Font.HELVETICA, 12f, Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });
            if (tipoOperacao.Equals("Venda"))
            {
                table.AddCell(new PdfPCell(new Phrase("Cliente", new Font(Font.HELVETICA, 12f, Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });
            }
            else
            {
                table.AddCell(new PdfPCell(new Phrase("Fornecedor", new Font(Font.HELVETICA, 12f, Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });
            }
            table.AddCell(new PdfPCell(new Phrase("Valor", new Font(Font.HELVETICA, 12f, Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase("Desconto", new Font(Font.HELVETICA, 12f, Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase("Total", new Font(Font.HELVETICA, 12f, Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase("Data", new Font(Font.HELVETICA, 12f, Font.BOLD))) { HorizontalAlignment = Element.ALIGN_CENTER });

            // Adicionar linhas de dados (exemplo, ajuste conforme necessário)
            foreach (var pedido in listaPedidos)
            {
                table.AddCell($"{pedido.id}");
                table.AddCell(new PdfPCell(new Phrase($"{pedido.clienteNome}")) { HorizontalAlignment = Element.ALIGN_LEFT });
                table.AddCell($"R$ {pedido.valor}");
                table.AddCell($"{pedido.desconto}%");
                table.AddCell($"R$ {pedido.total}");
                table.AddCell($"{pedido.data.ToString("dd/MM/yyyy")}");
            }
            pdf.Add(table);

            pdf.Close();

            return memoryStream.ToArray();
        }

    }
}
