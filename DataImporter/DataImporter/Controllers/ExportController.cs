using ClosedXML.Excel;
using DataImporter.Models;
using DataImporter.Models.ExportModels;
using DataImporter.Models.GroupModels;
using DataImporter.Models.ImportModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.IO;
using System.Security.Claims;

namespace DataImporter.Controllers
{
    [Authorize(Roles = "User")]
    public class ExportController : Controller
    {
        private readonly ILogger<GroupController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ExportController(ILogger<GroupController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            var model = new CreateExportModel();
            model.LoadGroupListByUserId(Guid.Parse(_httpContextAccessor.HttpContext.User
                        .FindFirst(ClaimTypes.NameIdentifier).Value));

            return View(model);
        }

        public ActionResult DownloadExcel(CreateExportModel model)
        {
            model.LoadGroup();
            model.LoadTableFields();
            model.LoadExcelDatas();

            DataTable dt = model.getData();
            string fileName = model.Group.Name + ".xlsx";

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        public IActionResult ExportAsEmail(CreateExportModel model)
        {
            model.LoadTableFields();
            model.LoadExcelDatas();
            model.LoadGroup();
            model.LoadFileNameAndPath();

            model.SaveFileToApp();
            model.SaveDataInExportTableForEmail();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult History()
        {
            var model = new ExportHistoryModel();
            return View(model);
        }

        public JsonResult GetExportData()
        {
            var dataTablesModel = new DataTablesAjaxRequestModel(Request);
            var model = new ExportHistoryModel();
            var data = model.GetExports(dataTablesModel);
            return Json(data);
        }

        public FileResult Download(int id)
        {
            var model = new ExportHistoryModel();
            var export = model.GetExport(id);

            byte[] bytes = System.IO.File.ReadAllBytes(export.FilePath);
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Path.GetFileName(export.FilePath));
        }
    }
}
