using DataImporter.Excel.BusinessObjects;
using System.Collections.Generic;

namespace DataImporter.Excel.Services
{
    public interface ITableFieldService
    {
        void CreateTableField(TableField tableField);
        void DeleteTableField(int id);
        IList<TableField> GetAllTableFields();
        TableField GetTableField(int id);
        public IList<TableField> GetTableFieldsByGroup(int groupId);

        (IList<TableField> records, int total, int totalDisplay) GetTableFields(
            int pageIndex, int pageSize, string searchText, string sortText);

        public IList<TableField> GetTableFields(int GroupId);

        void UpdateTableField(TableField tableField);
    }
}