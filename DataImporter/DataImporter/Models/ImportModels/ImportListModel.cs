using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Autofac;
using DataImporter.Excel.Services;
using Microsoft.AspNetCore.Http;

namespace DataImporter.Models.ImportModels
{
    public class ImportListModel
    {
        private IImportService _importService;
        private IHttpContextAccessor _httpContextAccessor;

        public ImportListModel()
        {
            _importService = Startup.AutofacContainer.Resolve<IImportService>();
            _httpContextAccessor = Startup.AutofacContainer.Resolve<IHttpContextAccessor>();
        }

        public ImportListModel(IImportService importService, IHttpContextAccessor httpContextAccessor)
        {
            _importService = importService;
            _httpContextAccessor = httpContextAccessor;
        }

        internal object GetImports(DataTablesAjaxRequestModel tableModel)
        {
            var data = _importService.GetImports(
                tableModel.PageIndex,
                tableModel.PageSize,
                tableModel.SearchText,
                tableModel.GetSortText(new string[] { "Id", "ApplicationUserEmail", "GroupName", "Path", "DateTime", "Status" }),
                Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value));

            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {
                                record.Id.ToString(),
                                record.ApplicationUserEmail,
                                record.GroupName.ToString(),
                                Path.GetFileName(record.Path).Split('_')[1],
                                record.DateTime.ToString(),
                                record.Status
                        }
                    ).ToArray()
            };
        }

        internal void Delete(int id)
        {
            _importService.DeleteImport(id);
        }
    }
}
