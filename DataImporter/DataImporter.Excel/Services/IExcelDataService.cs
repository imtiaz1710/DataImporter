using DataImporter.Excel.BusinessObjects;
using System.Collections.Generic;

namespace DataImporter.Excel.Services
{
    public interface IExcelDataService
    {
        void CreateExcelData(ExcelData excelData);
        void DeleteExcelData(int id);
        IList<ExcelData> GetAllExcelDatas();
        ExcelData GetExcelData(int id);
        public IList<ExcelData> GetExcelDatasByGroupId(int groupId);

        (IList<ExcelData> records, int total, int totalDisplay) GetExcelDatas(
            int pageIndex, int pageSize, string searchText, string sortText);

        void UpdateExcelData(ExcelData excelData);
    }
}