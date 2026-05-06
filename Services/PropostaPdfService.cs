using GestaoDesignerDeMemorias.Models;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;

namespace GestaoDesignerDeMemorias.Services
{
    public class PropostaPdfService
    {
        private readonly string _logoPath = "wwwroot/logo.png";

        public byte[] GerarProposta(Projeto projeto, Cliente cliente)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            // Registrar fontes da Jana
            FontManager.RegisterFont(new MemoryStream(File.ReadAllBytes("wwwroot/fonts/Montserrat-Regular.ttf")));
            FontManager.RegisterFont(new MemoryStream(File.ReadAllBytes("wwwroot/fonts/Montserrat-Bold.ttf")));
            FontManager.RegisterFont(new MemoryStream(File.ReadAllBytes("wwwroot/fonts/Lora-Regular.ttf")));
            FontManager.RegisterFont(new MemoryStream(File.ReadAllBytes("wwwroot/fonts/Lora-Bold.ttf")));


            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(35);
                    page.DefaultTextStyle(x => x.FontFamily("Montserrat").FontSize(11));

                    // Header
                    page.Header().Height(100).Background(Color.FromHex("#593ac2")).Padding(15).Row(row =>
                    {
                        if (File.Exists(_logoPath))
                            row.ConstantItem(75).Image(_logoPath).FitHeight();

                        row.RelativeItem().AlignMiddle().PaddingLeft(15).Column(col =>
                        {
                            col.Item().Text("Jana Art Designer").FontSize(22).SemiBold().FontColor(Colors.White);
                            col.Item().Text("Designer de Memórias").FontSize(12).FontColor(Colors.White);
                        });
                    });

                    page.Content().Column(col =>
                    {
                        col.Item().PaddingTop(25).Text("FICHA DE PEDIDO")
                            .FontFamily("Lora").FontSize(18).SemiBold().AlignCenter().FontColor(Color.FromHex("#593ac2"));

                        col.Item().PaddingTop(12).Text(projeto.NomeEvento.ToUpper())
                            .FontFamily("Lora").FontSize(19).SemiBold().AlignCenter().FontColor(Color.FromHex("#593ac2"));

                        col.Item().PaddingTop(10).Text($"Cliente: {cliente.Nome} • WhatsApp: {cliente.WhatsApp}")
                            .FontSize(12).AlignCenter();

                        // Tabela
                        col.Item().PaddingTop(30).Text("ITENS CONTRATADOS").SemiBold().FontSize(14).FontColor(Color.FromHex("#593ac2"));

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(5);
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(2);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background(Color.FromHex("#593ac2")).Padding(8).Text("Item").FontColor(Colors.White).SemiBold();
                                header.Cell().Background(Color.FromHex("#593ac2")).Padding(8).Text("Detalhe").FontColor(Colors.White).SemiBold();
                                header.Cell().Background(Color.FromHex("#593ac2")).AlignRight().Padding(8).Text("Valor").FontColor(Colors.White).SemiBold();
                            });

                            foreach (var item in projeto.Itens)
                            {
                                table.Cell().Padding(8).Text(item.Nome);
                                table.Cell().Padding(8).Text(item.Descricao ?? "-");
                                table.Cell().AlignRight().Padding(8).Text($"R$ {item.Valor:F2}").FontColor(Color.FromHex("#f7db15"));
                            }
                        });

                        col.Item().PaddingTop(20).AlignRight().Text($"TOTAL: R$ {projeto.ValorTotal:F2}")
                            .FontSize(16).SemiBold().FontColor(Color.FromHex("#593ac2"));

                        col.Item().PaddingTop(35).Text("INFORMAÇÕES IMPORTANTES").SemiBold().FontSize(13).FontColor(Color.FromHex("#593ac2"));
                        col.Item().Text("• Prazo médio: 3 a 7 dias úteis após briefing completo\n• Revisões ilimitadas até 10 ajustes antes da aprovação final\n• Após aprovação, alterações serão cobradas em R$ 19,00 por modificação\n• 50% de sinal para reserva de agenda\n• Entrega via Google Drive")
                            .FontSize(10.8f).LineHeight(1.45f);
                    });

                    page.Footer().AlignCenter().Text("Jana Art Designer • Cada detalhe pensado com carinho ⭐")
                        .FontSize(10).FontColor(Color.FromHex("#593ac2"));
                });
            });

            return document.GeneratePdf();
        }
    }
}