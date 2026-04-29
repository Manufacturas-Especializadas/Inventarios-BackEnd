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

            bool isL12 = lineName.Contains("12");

            worksheet.Cell("A1").Value = $"Balance de Inventario - {lineName}";
            worksheet.Cell("A2").Value = $"Fecha de generación: {DateTime.Now:dd/MM/yyyy HH:mm}";

            List<string> headersList = new() { "No. Parte" };

            if (isL12)
            {
                headersList.Add("Folio");
                headersList.Add("SO Entrada");
            }

            headersList.Add("Entradas");

            if (isL12)
            {
                headersList.Add("Cajas");
            }

            headersList.Add("Salidas");

            if (isL12)
                headersList.Add("SO Salida");
            else
                headersList.Add("Restante");

            if (isL12) headersList.Add("Restante");

            headersList.Add("Última Entrada");
            headersList.Add("Última Salida");
            headersList.Add("Cliente");

            if (!isL12) headersList.Add("Shop Orders");

            var headers = headersList.ToArray();

            string lastCol = GetExcelColumnName(headers.Length);
            worksheet.Range($"A1:{lastCol}1").Merge().Style.Font.SetBold().Font.FontSize = 14;
            worksheet.Range($"A2:{lastCol}2").Merge();

            for (int i = 0; i < headers.Length; i++)
            {
                var cell = worksheet.Cell(4, i + 1);
                cell.Value = headers[i];
                cell.Style.Font.SetBold();
                cell.Style.Fill.BackgroundColor = XLColor.LightGray;
                cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            }

            int row = 5;
            foreach (var item in balances)
            {
                int col = 1;

                worksheet.Cell(row, col++).Value = item.PartNumber;

                if (isL12)
                {
                    worksheet.Cell(row, col++).Value = item.Folio ?? "---";
                    worksheet.Cell(row, col++).Value = item.EntryShopOrders;
                }

                worksheet.Cell(row, col++).Value = item.TotalEntries;

                if (isL12)
                {
                    worksheet.Cell(row, col++).Value = item.TotalBoxes?.ToString() ?? "---";
                }

                worksheet.Cell(row, col++).Value = item.TotalExits;

                if (isL12)
                {
                    worksheet.Cell(row, col++).Value = item.ExitShopOrders;
                }

                var stockCell = worksheet.Cell(row, col++);
                stockCell.Value = item.Stock;
                stockCell.Style.Font.SetBold();
                if (item.Stock <= 0) stockCell.Style.Font.FontColor = XLColor.Red;

                worksheet.Cell(row, col++).Value = item.LastEntryDate?.ToString("dd/MM/yyyy HH:mm") ?? "---";
                worksheet.Cell(row, col++).Value = item.LastExitDate?.ToString("dd/MM/yyyy HH:mm") ?? "---";
                worksheet.Cell(row, col++).Value = item.Client ?? "SIN ASIGNAR";

                if (!isL12)
                {
                    worksheet.Cell(row, col++).Value = item.ExitShopOrders;
                }

                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return stream.ToArray();
        }

        private string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }
    }
}