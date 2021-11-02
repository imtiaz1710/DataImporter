using Autofac;
using DataImporter.Excel.BusinessObjects;
using DataImporter.Excel.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;

namespace DataImporter.Models.ExportModels
{
    public class ExportHistoryModel
    {
        private readonly IExportService _exportService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ExportHistoryModel()
        {
            _exportService = Startup.AutofacContainer.Resolve<IExportService>();
            _httpContextAccessor = Startup.AutofacContainer.Resolve<IHttpContextAccessor>();
        }

        public ExportHistoryModel(IExportService exportService, IHttpContextAccessor httpContextAccessor)
        {
            _exportService = exportService;
            _httpContextAccessor = httpContextAccessor;
        }

        internal Export GetExport(int id)
        {
           return  _exportService.GetExport(id);
        }

        internal object GetExports(DataTablesAjaxRequestModel tableModel)
        {
            var data = _exportService.GetExports(
                tableModel.PageIndex,
                tableModel.PageSize,
                tableModel.SearchText,
                tableModel.GetSortText(new string[] { "Id","DateTime" }),
                Guid.Parse(_httpContextAccessor.HttpContext.User
                        .FindFirst(ClaimTypes.NameIdentifier).Value)
                );

            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {
                            record.Group.Name,
                            record.DateTime.ToString(),
                            record.Id.ToString()
                        }
                    ).ToArray()
            };
        }

    }
}
