using Autofac;
using ClosedXML.Excel;
using DataImporter.Common.Utilities;
using DataImporter.Excel.BusinessObjects;
using DataImporter.Excel.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DataImporter.Models.ExportModels
{
    public class CreateExportModel
    {
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public IList<Group> Groups { get; set; }
        public IList<TableField> TableFields { get; set; }
        public IList<ExcelData> ExcelDatas { get; set; }
        private readonly IGroupService _groupService;
        private readonly IExcelDataService _excelDataService;
        private readonly ITableFieldService _tableFieldService;
        private readonly IExportService _exportService;
        private readonly IDateTimeUtility _dateTimeUtility;

        public CreateExportModel()
        {
            _groupService = Startup.AutofacContainer.Resolve<IGroupService>();
            _excelDataService = Startup.AutofacContainer.Resolve<IExcelDataService>();
            _tableFieldService = Startup.AutofacContainer.Resolve<ITableFieldService>();
            _exportService = Startup.AutofacContainer.Resolve<IExportService>();
            _dateTimeUtility = Startup.AutofacContainer.Resolve<IDateTimeUtility>();
        }

        public CreateExportModel(IGroupService groupService, ITableFieldService tableFieldService, 
            IExcelDataService excelDataService, IExportService exportService, IDateTimeUtility dateTimeUtility)
        {
            _groupService = groupService;
            _tableFieldService = tableFieldService;
            _excelDataService = excelDataService;
            _exportService = exportService;
            _dateTimeUtility = dateTimeUtility;
        }

        public void LoadGroupListByUserId(Guid userId)
        {
            Groups = _groupService.GetAllGroupsByUserId(userId);
        }

        public void LoadGroup()
        {
            Group = _groupService.GetGroup(GroupId);
        }

        public void LoadTableFields()
        {
            TableFields = _tableFieldService.GetTableFieldsByGroup(GroupId);
        }

        public void LoadExcelDatas()
        {
            ExcelDatas = _excelDataService.GetExcelDatasByGroupId(GroupId);
        }

        public void LoadFileNameAndPath()
        {
            FileName = Guid.NewGuid().ToString() + "_" + Group.Name + ".xlsx";
            FilePath = Path.Combine("../UploadFile", FileName);
        }

        public DataTable getData()
        {
            DataTable dt = new DataTable();
            dt.TableName = Group.Name;

            foreach (var tableField in TableFields)
            {
                dt.Columns.Add(tableField.FieldName, typeof(string));
            }

            foreach (var excelData in ExcelDatas)
            {
                var row = dt.NewRow();
                foreach (var excelFieldData in excelData.ExcelFieldDatas)
                {
                    row[excelFieldData.Field] = excelFieldData.Value;
                }
                dt.Rows.Add(row);
            }

            dt.AcceptChanges();
            return dt;
        }

        public void SaveFileToApp()
        {
            var wb = new XLWorkbook();
            DataTable dt = getData();
            wb.Worksheets.Add(dt, Group.Name);
            wb.SaveAs(FilePath);
        }

        public void SaveDataInExportTableForEmail()
        {
            _exportService.CreateExport(
                new Export
                {
                    ApplicationUserId = Group.ApplicationUserId,
                    GroupId = GroupId,
                    To = Group.ApplicationUser.Email,
                    Subject = "Export Excel",
                    TextBody = "Thanks for Using DataImporter!",
                    FilePath = FilePath/*Path.Combine(FilePath,FileName)*/,
                    Status = "pending",
                    DateTime = _dateTimeUtility.Now
                }
            );
        }
    }
}
