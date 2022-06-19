using System;
using System.Collections.Generic;
using System.IO;
using LawyerTimeTracker.Models;
using LawyerTimeTracker.Utils;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace LawyerTimeTracker.Services
{
    public class ReportService
    {
        private readonly TaskService _taskService;
        private readonly ExcelGenerator _excelGenerator;

        public ReportService(ApplicationContext context)
        {
            _taskService = new TaskService(context);
            _excelGenerator = new ExcelGenerator();
        }

        public void FillOrganizationTasksWorksheet(int organizationId)
        {
            List<Issue> tasks = _taskService.GetAllOrganizationTasks(organizationId);
            ExcelWorksheet worksheet = _excelGenerator.FindOrAddWorksheet(ExcelGenerator.TASKS_WORKSHEET);

            FillTitlesForWorksheet(worksheet, new List<string>{"Id", "Title", "Description", "Type", "Start Time", "End Time"});
            FillTasksForWorksheet(worksheet, tasks);
        }

        public byte[] GetReportFileAsBytes()
        {
            return _excelGenerator.GetWorkbookAsBytes();
        }

        private void FillTitlesForWorksheet(ExcelWorksheet worksheet, List<string> titles)
        {
            for (int i = 0; i < titles.Count; i++)
            {
                worksheet.Cells[1, i + 1].Value = titles[i];
            }
        }
        
        private void FillTasksForWorksheet(ExcelWorksheet worksheet, List<Issue> tasks)
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = tasks[i].Id;
                worksheet.Cells[i + 2, 2].Value = tasks[i].Title;
                worksheet.Cells[i + 2, 3].Value = tasks[i].Description;
                worksheet.Cells[i + 2, 4].Value = tasks[i].TypeOfTask;
                worksheet.Cells[i + 2, 5].Style.Numberformat.Format = "mm/dd/yyyy hh:mm:ss";
                worksheet.Cells[i + 2, 5].Value = tasks[i].StartTime;
                worksheet.Cells[i + 2, 6].Style.Numberformat.Format = "mm/dd/yyyy hh:mm:ss";
                worksheet.Cells[i + 2, 6].Value = tasks[i].EndTime;
            }
        }
    }
}