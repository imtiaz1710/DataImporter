using DataImporter.Models.ContactModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DataImporter.Controllers
{
    [Authorize(Roles = "User")]
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            var model = new ContactListModel();
            model.LoadGroups();
            return View(model);
        }

        [HttpPost]
        public IActionResult Load(ContactListModel model)
        {
            model.LoadGroups();
            model.LoadTableFields();
            model.LoadExcelDatas();
            return View(model);
        }
    }
}
