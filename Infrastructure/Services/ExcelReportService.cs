using Application.DTOs;
using Application.Interfaces;
using ClosedXML.Excel;

namespace Infrastructure.Services
{
    public class ExcelReportService : IExcelReportService
    {
        public byte[] GenerateBalanceReport(List<PartBalanceDto> balances, string lineName)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add($"Reporte {lineName}");

            worksheet.Cell("A1").Value = $"Balance de Inventario - {lineName}";
            worksheet.Range("A1:H1").Merge().Style.Font.SetBold().Font.FontSize = 14;
            worksheet.Cell("A2").Value = $"Fecha de generación: {DateTime.Now:dd/MM/yyyy HH:mm}";
            worksheet.Range("A2:H2").Merge();

            var headers = new[] { "No. Parte", "Entradas", "Salidas", "Restante", "Última Entrada", "Última Salida", "Cliente", "Shop Orders" };
            for (int i = 0; i < headers.Length; i++)
            {
                var cell = worksheet.Cell(4, i + 1);
                cell.Value = headers[i];
                cell.Style.Font.SetBold();
                cell.Style.Fill.BackgroundColor = XLColor.LightGray;
                cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }

            int row = 5;
            foreach (var item in balances)
            {
                worksheet.Cell(row, 1).Value = item.PartNumber;
                worksheet.Cell(row, 2).Value = item.TotalEntries;
                worksheet.Cell(row, 3).Value = item.TotalExits;

                var stockCell = worksheet.Cell(row, 4);
                stockCell.Value = item.Stock;
                stockCell.Style.Font.SetBold();
                if (item.Stock <= 0) stockCell.Style.Font.FontColor = XLColor.Red;

                worksheet.Cell(row, 5).Value = item.LastEntryDate?.ToString("dd/MM/yyyy HH:mm") ?? "---";
                worksheet.Cell(row, 6).Value = item.LastExitDate?.ToString("dd/MM/yyyy HH:mm") ?? "---";
                worksheet.Cell(row, 7).Value = item.Client ?? "SIN ASIGNAR";

                worksheet.Cell(row, 8).Value = item.ExitShopOrders;

                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return stream.ToArray();
        }

    }
}