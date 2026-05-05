using GestaoDesignerDeMemorias.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace GestaoDesignerDeMemorias.Services
{
    public class PropostaPdfService
    {
        public byte[] GerarProposta(Projeto projeto, Cliente cliente)
        {
            // Configuração básica do QuestPDF
            QuestPDF.Settings.License = LicenseType.Community;

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    page.Header()
                        .Text("Jana Art Designer - Proposta")
                        .SemiBold().FontSize(20).AlignCenter().FontColor(Colors.Pink.Medium);

                    page.Content().Column(col =>
                    {
                        col.Item().PaddingBottom(20).Text($"Proposta para: {projeto.NomeEvento}")
                            .FontSize(18).SemiBold();

                        col.Item().Text($"Cliente: {cliente.Nome} | WhatsApp: {cliente.WhatsApp}")
                            .FontSize(12);

                        col.Item().PaddingTop(20).Text("Itens da Proposta:").SemiBold();

                        foreach (var item in projeto.Itens)
                        {
                            col.Item().Text($"- {item.Nome} → R$ {item.Valor:F2}").FontSize(12);
                        }

                        col.Item().PaddingTop(20).Text($"Valor Total: R$ {projeto.ValorTotal:F2}")
                            .FontSize(14).SemiBold().FontColor(Colors.Green.Darken2);

                        col.Item().PaddingTop(30).Text("Esta proposta foi gerada automaticamente pelo sistema Gestão Designer de Memórias.")
                            .FontSize(10).Italic();
                    });

                    page.Footer().AlignCenter().Text(x => x.CurrentPageNumber());
                });
            });

            return pdf.GeneratePdf();
        }
    }
}