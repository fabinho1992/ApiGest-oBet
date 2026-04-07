using App_Bets.Domain.Modelos;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace App_Bets.Application.Documents
{
    public class RelatorioBilhetesPdfDocument : IDocument
    {
        private readonly List<Bilhete> _bilhetes;
        private readonly string _usuarioEmail;
        private readonly string _titulo;
        private readonly string _filtros;

        public RelatorioBilhetesPdfDocument(
            List<Bilhete> bilhetes,
            string usuarioEmail,
            string titulo,
            string filtros)
        {
            _bilhetes = bilhetes;
            _usuarioEmail = usuarioEmail;
            _titulo = titulo;
            _filtros = filtros;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(20);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header().Column(column =>
                {
                    column.Item().Text(_titulo).FontSize(18).Bold();
                    column.Item().Text($"Usuário: {_usuarioEmail}");
                    column.Item().Text($"Emitido em: {FormatarDataHoraBrasil(DateTime.UtcNow)}");
                    column.Item().Text($"Filtros: {_filtros}");
                    column.Item().PaddingTop(8).LineHorizontal(1);
                });

                page.Content().PaddingVertical(10).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(2);    // mercado
                        columns.RelativeColumn(2);    // casa
                        columns.RelativeColumn(2);    // tipo
                        columns.RelativeColumn(1);    // odd
                        columns.RelativeColumn(1.5f); // valor
                        columns.RelativeColumn(1.5f); // retorno
                        columns.RelativeColumn(1.5f); // status
                        columns.RelativeColumn(2);    // data
                    });

                    void HeaderCell(string text) =>
                        table.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text(text).Bold();

                    HeaderCell("Mercado");
                    HeaderCell("Casa");
                    HeaderCell("Tipo");
                    HeaderCell("Odd");
                    HeaderCell("Valor");
                    HeaderCell("Retorno");
                    HeaderCell("Status");
                    HeaderCell("Data");

                    foreach (var item in _bilhetes)
                    {
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(item.Mercado.ToString());
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(item.CasaAposta.ToString());
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(item.TipoBanca.ToString());
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(item.Odd.ToString("0.00"));
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text($"R$ {item.ValorApostado:0.00}");
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text($"R$ {item.ValorRetornado:0.00}");
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(item.Status.ToString());
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(FormatarDataHoraBrasil(item.DataAposta));
                    }
                });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Página ");
                        x.CurrentPageNumber();
                        x.Span(" de ");
                        x.TotalPages();
                    });
            });
        }

        private static string FormatarDataHoraBrasil(DateTime? dataUtc)
        {
            if (!dataUtc.HasValue)
                return string.Empty;

            var dataBrasil = ConverterUtcParaBrasil(dataUtc.Value);
            return dataBrasil.ToString("dd/MM/yyyy HH:mm");
        }

        private static DateTime ConverterUtcParaBrasil(DateTime dataUtc)
        {
            var dataComKindUtc = dataUtc.Kind == DateTimeKind.Utc
                ? dataUtc
                : DateTime.SpecifyKind(dataUtc, DateTimeKind.Utc);

            try
            {
                var tz = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");
                return TimeZoneInfo.ConvertTimeFromUtc(dataComKindUtc, tz);
            }
            catch
            {
                var tz = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
                return TimeZoneInfo.ConvertTimeFromUtc(dataComKindUtc, tz);
            }
        }
    }
}