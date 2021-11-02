using DataImporter.Excel.BusinessObjects;
using System;
using System.Collections.Generic;

namespace DataImporter.Excel.Services
{
    public interface IImportService
    {
        void CreateImport(Import import);
        void DeleteImport(int id);
        IList<Import> GetAllImports();
        Import GetImport(int id);

        (IList<Import> records, int total, int totalDisplay) GetImports(
            int pageIndex, int pageSize, string searchText, string sortText, Guid userId);

        public IList<Import> GetPendingImports();

        void UpdateImport(Import import);
    }
}