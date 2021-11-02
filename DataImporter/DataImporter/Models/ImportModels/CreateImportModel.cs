using Autofac;
using ClosedXML.Excel;
using DataImporter.Common.Utilities;
using DataImporter.Excel.BusinessObjects;
using DataImporter.Excel.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DataImporter.Models.ImportModels
{
    public class CreateImportModel
    {
        public string FilePath { get; set; }
        public IFormFile File { get; set; }
        public DataTable DataTable = new DataTable();
        public int GroupId { get; set; }
        public IList<Group> Groups { get; set; }
        public IList<string> TableFields { get; set; }
        public bool IsFieldMatch { get; set; }
        private readonly IGroupService _groupService;
        private readonly IImportService _importService;
        private readonly ITableFieldService _tableFieldService;
        private readonly IDateTimeUtility _dateTimeUtility;

        public CreateImportModel()
        {
            _groupService = Startup.AutofacContainer.Resolve<IGroupService>();
            _importService = Startup.AutofacContainer.Resolve<IImportService>();
            _dateTimeUtility = Startup.AutofacContainer.Resolve<IDateTimeUtility>();
            _tableFieldService = Startup.AutofacContainer.Resolve<ITableFieldService>();
        }

        public void SaveToApp()
        {
            using var fileStream = new FileStream(FilePath, FileMode.Create);
            File.CopyTo(fileStream);
        }

        public void LoadDataTableFromExcel()
        {
            using (XLWorkbook workbook = new XLWorkbook(FilePath))
            {
                IXLWorksheet worksheet = workbook.Worksheet(1);
                bool FirstRow = true;
                string readRange = "1:1";

                foreach (IXLRow row in worksheet.RowsUsed())
                {
                    if (FirstRow)
                    {
                        readRange = string.Format("{0}:{1}", 1, row.LastCellUsed().Address.ColumnNumber);
                        foreach (IXLCell cell in row.Cells(readRange))
                        {
                            string fieldName = cell.Value.ToString().Trim().ToLower();
                            DataTable.Columns.Add(fieldName);
                        }
                        FirstRow = false;
                    }
                    else
                    { 
                        DataTable.Rows.Add();
                        int cellIndex = 0;

                        foreach (IXLCell cell in row.Cells(readRange))
                        {
                            DataTable.Rows[DataTable.Rows.Count - 1][cellIndex] = cell.Value.ToString();
                            cellIndex++;
                        }
                    }
                }
                
                if (FirstRow)
                {
                    throw new InvalidOperationException("Empty Excel File!");
                }
            }
        }

        public void SendImportPendingList()
        {
            var group = _groupService.GetGroup(GroupId);

            var import = new Import
            {
                ApplicationUserId = group.ApplicationUserId,
                GroupId = GroupId,
                Path = FilePath,
                DateTime = _dateTimeUtility.Now,
                Status = "pending"
            };

            try
            {
                _importService.CreateImport(import);
            }
            catch
            {
                throw new InvalidOperationException("Failed to create import");
            }
            
        }

        public void LoadTableFields()
        {
            TableFields = _tableFieldService.GetTableFields(GroupId)
                .Select(x => x.FieldName).OrderBy(x => x).ToList();
        }

        public void LoadFieldMatchingStatus()
        {
            var tableFieldDataTable = new List<string>();

            foreach (DataColumn column in DataTable.Columns)
            {
                tableFieldDataTable.Add(column.Caption.ToString().Trim().ToLower());
            }

            var SortedTableFields = TableFields.OrderBy(x => x).ToList();
            var SortedtableFieldDataTable = tableFieldDataTable.OrderBy(x => x).ToList();
            bool isEqual = Enumerable.SequenceEqual(SortedTableFields, SortedtableFieldDataTable);

            if (isEqual)
            {
                IsFieldMatch = true;
            }
            else
            {
                IsFieldMatch = false;
            }
        }

        public void CreateTableFields()
        {
            var group = _groupService.GetGroup(GroupId);
                
            foreach (DataColumn column in DataTable.Columns)
            {
                _tableFieldService.CreateTableField(new TableField()
                {
                    GroupId = GroupId,
                    ApplicationUserId = group.ApplicationUserId,
                    FieldName = column.Caption.ToString().Trim().ToLower()
                });
            }
        }

        public void DeleteFile()
        {
            try
            {
                System.IO.File.Delete(FilePath);
            }
            catch
            {
                throw new NullReferenceException("Given Path does not Contain a file");
            }
        }
    }
}
