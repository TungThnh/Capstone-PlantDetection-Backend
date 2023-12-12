using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.ComponentModel;
using System.Data;

namespace Common.Helpers
{
    public static class ExcelHelper
    {
        public static void ExportToExcel(DataTable dataTable, string sheetName, string filePath)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using (ExcelPackage excelPackage = new ExcelPackage())
            {

                // Tạo một worksheet mới
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add(sheetName);

                // Đổ dữ liệu từ DataTable vào worksheet
                worksheet.Cells["A1"].LoadFromDataTable(dataTable, true);

                excelPackage.SaveAs(new System.IO.FileInfo(filePath));
            }
        }
    }
}
