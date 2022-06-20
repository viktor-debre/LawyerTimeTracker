using System.Threading.Tasks;
using LawyerTimeTracker.Models;
using LawyerTimeTracker.Services;
using LawyerTimeTracker.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LawyerTimeTracker.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly AccountService _accountService;
        private readonly ReportService _reportService;

        public StatisticsController(ApplicationContext context)
        {
            _accountService = new AccountService(context);
            _reportService = new ReportService(context);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Statistics()
        {
            ViewBag.AuthorizedUser = await _accountService.GetUserByEmail(User.Identity.Name);
            return View();
        }

        public IActionResult DownloadOrganizationTasksFile(int organizationId)
        {
            _reportService.FillOrganizationTasksWorksheet(organizationId);
            return File(_reportService.GetReportFileAsBytes(), ExcelGenerator.CONTENT_TYPE, ExcelGenerator.FILE_NAME);
        }
    }
}