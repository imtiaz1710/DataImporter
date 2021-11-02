using Autofac;
using DataImporter.Excel.BusinessObjects;
using DataImporter.Excel.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DataImporter.Models.Dashboard
{
    public class DashboardListModel
    {
        public int GroupCount { get; set; }
        public int PendingImportCount { get; set; }
        public int PendingExportCount { get; set; }
        private readonly IGroupService _groupService;
        private readonly IExportService _exportService;
        private readonly IImportService _importService;
        private IHttpContextAccessor _httpContextAccessor;

        public DashboardListModel()
        {
            _groupService = Startup.AutofacContainer.Resolve<IGroupService>();
            _exportService = Startup.AutofacContainer.Resolve<IExportService>();
            _importService = Startup.AutofacContainer.Resolve<IImportService>();
            _httpContextAccessor = Startup.AutofacContainer.Resolve<IHttpContextAccessor>();
        }

        public DashboardListModel(IGroupService groupService, IExportService exportService, 
            IImportService importService, IHttpContextAccessor httpContextAccessor)
        {
            _groupService = groupService;
            _exportService = exportService;
            _importService = importService;
            _httpContextAccessor = httpContextAccessor;
        }

        public void LoadGroupData()
        {
            GroupCount = _groupService.GetAllGroupsByUserId(Guid.Parse(_httpContextAccessor.HttpContext.User
                        .FindFirst(ClaimTypes.NameIdentifier).Value)).Count();
        }

        public void LoadImportData()
        {
            PendingImportCount = _importService.GetPendingImports().Where(x => x.ApplicationUserId == Guid.Parse(_httpContextAccessor.HttpContext.User
                        .FindFirst(ClaimTypes.NameIdentifier).Value)).Count();
        }

        public void LoadExportData()
        {
            PendingExportCount = _exportService.GetPendingExports().Where(x => x.ApplicationUserId == Guid.Parse(_httpContextAccessor.HttpContext.User
                        .FindFirst(ClaimTypes.NameIdentifier).Value)).Count();
        }
    }
}
