using DataImporter.Models.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataImporter.Controllers
{
    [Authorize(Roles = "User")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            var model = new DashboardListModel();
            model.LoadGroupData();
            model.LoadImportData();
            model.LoadExportData();

            return View(model);
        }


    }
}
