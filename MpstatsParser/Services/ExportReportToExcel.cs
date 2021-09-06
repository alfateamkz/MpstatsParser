using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MpstatsParser.Models.Excel;
using OfficeOpenXml;
using System.IO;
namespace MpstatsParser.Services
{
    public static class ExportReportToExcel
    {
        public static async Task Export(string filepath,List<ExcelRowModel> rows)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            FileInfo file = new FileInfo(Environment.CurrentDirectory+@"\Report.xlsx");
            ExcelPackage pck = new ExcelPackage(file);
            var sheet = pck.Workbook.Worksheets[0];

            for (int i = 0; i < rows.Count; i++)
            {
                sheet.Cells[i + 2, 1].Value = rows[i].Category;
                sheet.Cells[i + 2, 2].Value = rows[i].Revenue6_20;
                sheet.Cells[i + 2, 3].Value = rows[i].Revenue7_20;
                sheet.Cells[i + 2, 4].Value = rows[i].Revenue8_20;
                sheet.Cells[i + 2, 5].Value = rows[i].Revenue9_20;
                sheet.Cells[i + 2, 6].Value = rows[i].Revenue10_20;
                sheet.Cells[i + 2, 7].Value = rows[i].Revenue11_20;
                sheet.Cells[i + 2, 8].Value = rows[i].Revenue12_20;
                sheet.Cells[i + 2, 9].Value = rows[i].Revenue1_21;
                sheet.Cells[i + 2, 10].Value = rows[i].Revenue2_21;
                sheet.Cells[i + 2, 11].Value = rows[i].Revenue3_21;
                sheet.Cells[i + 2, 12].Value = rows[i].Revenue4_21;
                sheet.Cells[i + 2, 13].Value = rows[i].Revenue5_21;
                sheet.Cells[i + 2, 14].Value = rows[i].Revenue6_21;
                sheet.Cells[i + 2, 15].Value = rows[i].Revenue6_21_Top1;
                sheet.Cells[i + 2, 16].Value = rows[i].Revenue6_21_Top2;
                sheet.Cells[i + 2, 17].Value = rows[i].Revenue6_21_Top3;
                sheet.Cells[i + 2, 18].Value = rows[i].Revenue6_21_Top4;
                sheet.Cells[i + 2, 19].Value = rows[i].Revenue6_21_Other;
                sheet.Cells[i + 2, 20].Value = rows[i].SKU6_21;
                sheet.Cells[i + 2, 21].Value = rows[i].SKU6_21_X;
                sheet.Cells[i + 2, 22].Value = rows[i].ProductWithSalesQuantity13_19july20;
                sheet.Cells[i + 2, 23].Value = rows[i].ProductWithSalesQuantity11_17january21;
                sheet.Cells[i + 2, 24].Value = rows[i].ProductWithSalesQuantity5_11july21;
                sheet.Cells[i + 2, 25].Value = rows[i].ProductWithSalesRevenue13_19july20;
                sheet.Cells[i + 2, 26].Value = rows[i].ProductWithSalesRevenue11_17january21;
                sheet.Cells[i + 2, 27].Value = rows[i].ProductWithSalesRevenue5_11july21;
            }
            pck.SaveAs(new FileInfo(filepath + @"\Report.xlsx"));
        }
    }
}
