using Autofac;
using DataImporter;
using DataImporter.Excel.BusinessObjects;
using DataImporter.Excel.Services;
using DataImporter.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace DataImporter.Models.GroupModels
{
    public class GroupListModel
    {
        private IGroupService _groupService;
        public IList<Group> Groups { get; private set; }
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GroupListModel()
        {
            _groupService = Startup.AutofacContainer.Resolve<IGroupService>();
            _httpContextAccessor = Startup.AutofacContainer.Resolve<IHttpContextAccessor>();
        }

        public GroupListModel(IGroupService groupService, IHttpContextAccessor httpContextAccessor)
        {
            _groupService = groupService;
            _httpContextAccessor = httpContextAccessor;

        }

        public void GroupListByUserId(Guid userId)
        {
            Groups = _groupService.GetAllGroupsByUserId(userId);
        }

        internal object GetGroups(DataTablesAjaxRequestModel tableModel)
        {
            var groupId = Guid.Parse(_httpContextAccessor.HttpContext.User
                        .FindFirst(ClaimTypes.NameIdentifier).Value);

            var data = _groupService.GetGroups(
                tableModel.PageIndex,
                tableModel.PageSize,
                tableModel.SearchText,
                tableModel.GetSortText(new string[] {"Id", "Name"}), groupId);

            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {
                                record.Id.ToString(),
                                record.Name,
                                record.Id.ToString()
                        }
                    ).ToArray()
            };
        }

        internal void Delete(int id)
        {
            _groupService.DeleteGroup(id);
        }
    }
}
