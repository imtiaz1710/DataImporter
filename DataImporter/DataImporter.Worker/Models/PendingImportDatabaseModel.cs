using Autofac;
using ClosedXML.Excel;
using DataImporter.Excel.BusinessObjects;
using DataImporter.Excel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImporter.Worker.Models
{
    public class PendingImportDatabaseModel
    {
        private readonly IImportService _importService;
        private readonly IExcelFieldDataService _excelFieldDataService;
        private readonly IExcelDataService _excelDataService;
        private IList<Import> _pendingImports { get; set; }

        public PendingImportDatabaseModel()
        {
            _importService = Worker.AutofacContainer.Resolve<IImportService>();
            _excelFieldDataService = Worker.AutofacContainer.Resolve<IExcelFieldDataService>();
            _excelDataService = Worker.AutofacContainer.Resolve<IExcelDataService>();
        }

        public PendingImportDatabaseModel(IImportService importService, IExcelFieldDataService excelFieldDataService,
            IExcelDataService excelDataService)
        {
            _importService = importService;
            _excelFieldDataService = excelFieldDataService;
            _excelDataService = excelDataService;
        }

        public void LoadPendingImports()
        {
            _pendingImports = _importService.GetPendingImports();
        }

        public void UploadPendingFileToDb()
        {
            foreach (var import in _pendingImports)
            {
                var fields = new List<string>();

                using (XLWorkbook workbook = new XLWorkbook(import.Path))
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
                                fields.Add(fieldName);
                            }
                            FirstRow = false;
                        }
                        else
                        {
                            var excelData = new ExcelData
                            {
                                GroupId = import.GroupId,
                                ApplicationUserId = import.ApplicationUserId,
                                CreateDate = import.DateTime
                            };

                            int cellIndex = 0;
                            var rowCount = 0;

                            foreach (IXLCell cell in row.Cells(readRange))
                            {
                                var excelFieldData = new ExcelFieldData
                                {
                                    Field = fields[rowCount],
                                    Value = cell.Value.ToString(),
                                    ExcelDataId = excelData.Id
                                };

                                excelData.ExcelFieldDatas.Add(excelFieldData);

                                cellIndex++;
                                rowCount++;
                            }

                            _excelDataService.CreateExcelData(excelData);
                        }
                    }
                }
            }
        }

        public void ChangePandingStatus()
        {
            foreach (var import in _pendingImports)
            {
                import.Status = "completed";
                _importService.UpdateImport(import);
            }
        }

        public void DeletePendingFiles()
        {
            foreach (var import in _pendingImports)
            {
                try
                {
                    System.IO.File.Delete(import.Path);
                }
                catch
                {
                    throw new NullReferenceException("Given Path does not Contain a file");
                }
            }
        }
    }
}
