using DataImporter.Excel.BusinessObjects;
using System;
using System.Collections.Generic;

namespace DataImporter.Excel.Services
{
    public interface IExportService
    {
        void CreateExport(Export export);
        void DeleteExport(int id);
        IList<Export> GetAllExports();
        Export GetExport(int id);
        public IList<Export> GetPendingExports();

        public (IList<Export> records, int total, int totalDisplay) GetExports(int pageIndex, int pageSize,
            string searchText, string sortText, Guid userId);

        void UpdateExport(Export export);
    }
}