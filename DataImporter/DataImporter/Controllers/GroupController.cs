using DataImporter.Models;
using DataImporter.Models.GroupModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DataImporter.Controllers
{
    [Authorize(Roles = "User")]
    public class GroupController : Controller
    {
        private readonly ILogger<GroupController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GroupController(ILogger<GroupController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            var model = new GroupListModel();
            return View(model);
        }

        public JsonResult GetGroupData()
        {
            var dataTablesModel = new DataTablesAjaxRequestModel(Request);
            var model = new GroupListModel();
            var data = model.GetGroups(dataTablesModel);
            return Json(data);
        }

        public IActionResult Create()
        {
            var model = new CreateGroupModel();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(CreateGroupModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.UserId = Guid.Parse( _httpContextAccessor.HttpContext.User
                        .FindFirst(ClaimTypes.NameIdentifier).Value);

                    model.CreateGroup();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Failed to create group");
                    _logger.LogError(ex, "Create Group Failed");
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var model = new EditGroupModel();
            model.LoadModelData(id);

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(EditGroupModel model)
        {
            if (ModelState.IsValid)
            {
               /* model.UserId = Guid.Parse(_httpContextAccessor.HttpContext.User
                        .FindFirst(ClaimTypes.NameIdentifier).Value);*/

                model.Update();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var model = new GroupListModel();
            model.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
