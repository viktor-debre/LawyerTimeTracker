using System;
using OfficeOpenXml;

namespace LawyerTimeTracker.Utils
{
    public class ExcelGenerator
    {
        private readonly ExcelPackage excelPackage = new();
        public static readonly String CONTENT_TYPE = "APPLICATION/octet-stream";
        public static readonly String FILE_NAME = "Report.xlsx";
        public static readonly String TASKS_WORKSHEET = "Tasks";

        public ExcelWorksheet AddWorksheetsToWorkbook(String worksheetName)
        {
            return excelPackage.Workbook.Worksheets.Add(worksheetName);
        }

        public ExcelWorksheet FindOrAddWorksheet(String worksheetName)
        {
            try
            {
                foreach (var worksheet in excelPackage.Workbook.Worksheets)
                {
                    if (worksheet.Name.Equals(worksheetName))
                    {
                        return worksheet;
                    }
                }
                
                return AddWorksheetsToWorkbook(TASKS_WORKSHEET);
            }
            catch(Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public byte[] GetWorkbookAsBytes()
        {
            return excelPackage.GetAsByteArray();
        }
    }
}