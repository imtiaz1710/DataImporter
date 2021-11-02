using Autofac;
using DataImporter.Excel.BusinessObjects;
using DataImporter.Excel.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DataImporter.Models.ContactModels
{
    public class ContactListModel
    {
        public int GroupId { get; set; }
        public IList<TableField> TableFields { get; set; }
        public IList<ExcelData> ExcelDatas { get; set; }
        public IList<Group> Groups { get; set; }
        private readonly ITableFieldService _tableFieldService;
        private readonly IExcelDataService _excelDataService;
        private readonly IGroupService _groupService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ContactListModel()
        {
            _excelDataService = Startup.AutofacContainer.Resolve<IExcelDataService>();
            _tableFieldService = Startup.AutofacContainer.Resolve<ITableFieldService>();
            _groupService = Startup.AutofacContainer.Resolve<IGroupService>();
            _httpContextAccessor = Startup.AutofacContainer.Resolve<IHttpContextAccessor>();
        }
        
        public ContactListModel(ITableFieldService tableFieldService, IExcelDataService excelDataService, 
            IGroupService groupService, IHttpContextAccessor httpContextAccessor)
        {
            _tableFieldService = tableFieldService;
            _excelDataService = excelDataService;
            _groupService = groupService;
            _httpContextAccessor = httpContextAccessor;
        }

        public void LoadTableFields()
        {
            TableFields = _tableFieldService.GetTableFields(GroupId);
        }

        public void LoadExcelDatas()
        {
            ExcelDatas = _excelDataService.GetExcelDatasByGroupId(GroupId);
        }

        public void LoadGroups()
        {
            Groups = _groupService.GetAllGroupsByUserId(Guid.Parse(_httpContextAccessor.HttpContext.User
                        .FindFirst(ClaimTypes.NameIdentifier).Value));
        }
    }
}
