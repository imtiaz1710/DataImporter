using DataImporter.Excel.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImporter.Excel.Services
{
    public interface IExcelFieldDataService
    {
        IList<ExcelFieldData> GetAllExcelFieldDatas();
        void CreateExcelFieldData(ExcelFieldData excelFieldData);
        (IList<ExcelFieldData>records, int total, int totalDisplay) GetExcelFieldDatas(int pageIndex, int pageSize, 
            string searchText, string sortText);
        ExcelFieldData GetExcelFieldData(int id);
        void UpdateExcelFieldData(ExcelFieldData excelFieldData);
        void DeleteExcelFieldData(int id);
    }
}
