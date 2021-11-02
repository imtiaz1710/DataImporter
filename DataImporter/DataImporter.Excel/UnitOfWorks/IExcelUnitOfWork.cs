using DataImporter.Data;
using DataImporter.Excel.Entities;
using DataImporter.Excel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImporter.Excel.UnitOfWorks
{
    public interface IExcelUnitOfWork : IUnitOfWork
    {
        IExcelDataRepository ExcelDatas { get; }
        IExcelFieldDataRepository  ExcelFieldDatas{ get; }
        IImportRepository Imports { get; }
        IExportRepository Exports { get; }
        IGroupRepository Groups { get; }
        ITableFieldRepository TableFields { get; }
    }
}
