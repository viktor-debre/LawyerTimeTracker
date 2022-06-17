using System;
using OfficeOpenXml;

namespace LawyerTimeTracker.Utils
{
    public class ExcelGenerator
    {
        private static readonly ExcelPackage excelPackage = new ExcelPackage();
        public static readonly String CONTENT_TYPE = "APPLICATION/octet-stream";
        public static readonly String FILE_NAME = "Report.xlsx";
        
        private ExcelGenerator()
        {
            
        }

        public static ExcelWorksheet AddWorksheetsToWorkbook(String worksheetName)
        {
            return excelPackage.Workbook.Worksheets.Add(worksheetName);
        }
        
        public static byte[] GetWorkbookAsBytes()
        {
            return excelPackage.GetAsByteArray();
        }
        
    }
}