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
        private TaskService _taskService;

        public ReportService(ApplicationContext context)
        {
            _taskService = new TaskService(context);
        }

        public void FillOrganizationTasksWorksheet(int organizationId)
        {
            List<Issue> tasks = _taskService.GetAllOrganizationTasks(organizationId);
            ExcelWorksheet worksheet = ExcelGenerator.FindOrAddWorksheet(ExcelGenerator.TASKS_WORKSHEET);

            for (int i = 0; i < tasks.Count; i++)
            {
                worksheet.Cells[i+1, 1].Value = tasks[i].Id;
                worksheet.Cells[i+1, 2].Value = tasks[i].Title;
                worksheet.Cells[i+1, 3].Value = tasks[i].Description;
                worksheet.Cells[i+1, 4].Value = tasks[i].TypeOfTask;
                worksheet.Cells[i+1, 5].Style.Numberformat.Format = "mm/dd/yyyy hh:mm:ss";
                worksheet.Cells[i+1, 5].Value = tasks[i].StartTime;
                worksheet.Cells[i+1, 6].Style.Numberformat.Format = "mm/dd/yyyy hh:mm:ss";
                worksheet.Cells[i+1, 6].Value = tasks[i].EndTime;
            }
        }
    }
}