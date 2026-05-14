using Application.Interfaces;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ClosedXmlExcelReaderService : IExcelReaderService
    {
        public List<string> ExtractFoliosFromStream(Stream stream)
        {
            var folios = new List<string>();

            stream.Position = 0;

            using(var workbook = new XLWorkbook(stream))
            {
                var worksheet = workbook.Worksheet(1);
                var rows = worksheet.RangeUsed()!.RowsUsed();

                foreach(var row in rows)
                {
                    if (row.RowNumber() == 1) continue;

                    var folioValue = row.Cell(1).GetString()?.Trim();

                    if(!string.IsNullOrEmpty(folioValue))
                    {
                        folios.Add(folioValue);
                    }
                }
            }

            return folios;
        }
    }
}