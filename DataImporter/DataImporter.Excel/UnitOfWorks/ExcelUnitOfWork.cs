using DataImporter.Data;
using DataImporter.Excel.Contexts;
using DataImporter.Excel.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DataImporter.Excel.UnitOfWorks
{
    public class ExcelUnitOfWork : UnitOfWork, IExcelUnitOfWork
    {
        public IExcelDataRepository ExcelDatas { get; private set; }
        public IExcelFieldDataRepository ExcelFieldDatas { get; private set; }
        public IImportRepository Imports { get; private set; }
        public IExportRepository Exports { get; private set; }
        public IGroupRepository Groups { get; private set; }
        public ITableFieldRepository TableFields { get; private set; }

        public ExcelUnitOfWork(IExcelContext context, IExcelDataRepository excelDatas, 
            IExcelFieldDataRepository excelFieldDatas, IImportRepository imports,
            IExportRepository exports, IGroupRepository groups, ITableFieldRepository tableFields) : 
            base((DbContext)context)
        {
            ExcelDatas = excelDatas;
            ExcelFieldDatas = excelFieldDatas;
            Imports = imports;
            Groups = groups;
            Exports = exports;
            TableFields = tableFields;
        }
    }
}
