using ClosedXML.Excel;
using DataImporter.Models;
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
using System.Web;

namespace DataImporter.Controllers
{
    [Authorize(Roles = "User")]
    public class ImportController : Controller
    {
        private readonly ILogger<ImportController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ImportController(ILogger<ImportController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            var model = new ImportListModel();
            return View(model);
        }

        public ActionResult Upload()
        {
            var groupModel = new GroupListModel();
            groupModel.GroupListByUserId(Guid.Parse(_httpContextAccessor.HttpContext.User
                        .FindFirst(ClaimTypes.NameIdentifier).Value));

            var model = new CreateImportModel();
            model.Groups = groupModel.Groups;
            return View(model); 
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Preview(CreateImportModel model)
        {
            if (model.File != null && model.File.Length > 0 && 
                System.IO.Path.GetExtension(model.File.FileName).ToLower() == ".xlsx")
            {
                model.FilePath = Path.Combine("../UploadFile", Guid.NewGuid().ToString()
                    + "_" + model.File.FileName);

                model.SaveToApp();
                model.LoadDataTableFromExcel();
                model.LoadTableFields();

                if(model.TableFields.Count != 0)
                {
                    model.LoadFieldMatchingStatus();
                    if (!model.IsFieldMatch)
                    {
                        TempData["ErrorMessage"] = "Given field mismatch with group field";
                        return RedirectToAction(nameof(Upload));
                    }
                }

                return View(model);
            }
            else
            {
                TempData["ErrorMessage"] = "Please select file with .xlsx extension!";
                return RedirectToAction(nameof(Upload));
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult SendForPending(CreateImportModel model)
        {
            model.LoadTableFields();
            model.LoadDataTableFromExcel();

            if (model.TableFields.Count == 0)
            {
                model.CreateTableFields();
            }

            model.SendImportPendingList();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult  Delete(CreateImportModel model)
        {
            model.DeleteFile();
            return RedirectToAction(nameof(Upload));
        }

        public JsonResult GetImportHistory()
        {
            var dataTablesModel = new DataTablesAjaxRequestModel(Request);
            var model = new ImportListModel();
            var data = model.GetImports(dataTablesModel);
            return Json(data);
        }
    }
}
