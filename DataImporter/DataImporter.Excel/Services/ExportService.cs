using AutoMapper;
using DataImporter.Excel.BusinessObjects;
using DataImporter.Excel.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
namespace DataImporter.Excel.Services
{
    public class ExportService : IExportService
    {
        private readonly IExcelUnitOfWork _excelUnitOfWork;
        private readonly IMapper _mapper;

        public ExportService(IExcelUnitOfWork excelUnitOfWork, IMapper mapper)
        {
            _excelUnitOfWork = excelUnitOfWork;
            _mapper = mapper;
        }

        public IList<Export> GetAllExports()
        {
            var exportEntities = _excelUnitOfWork.Exports.GetAll();
            var exports = new List<Export>();

            foreach (var entity in exportEntities)
            {
                exports.Add(_mapper.Map<Export>(entity));
            }

            return exports;
        }

        public void CreateExport(Export export)
        {
            _excelUnitOfWork.Exports.Add(
                _mapper.Map<Entities.Export>(export)
            );

            _excelUnitOfWork.Save();
        }

        public (IList<Export> records, int total, int totalDisplay) GetExports(int pageIndex, int pageSize,
            string searchText, string sortText, Guid userId)
        {
            var exportData = _excelUnitOfWork.Exports.GetDynamic(string.IsNullOrWhiteSpace(searchText) ? x => x.ApplicationUserId == userId && x.Status == "completed" : 
                x => x.ApplicationUserId == userId && x.Status == "completed" && x.Group.Name.Contains(searchText),
                sortText, "Group", pageIndex, pageSize);

            var resultData = new List<Export>();

            foreach (Entities.Export export in exportData.data)
            {
                var group = new Group
                {
                    Id = export.Group.Id,
                    Name = export.Group.Name,
                    ApplicationUserId = export.Group.ApplicationUserId
                };

                resultData.Add(new Export
                {
                    Id = export.Id,
                    ApplicationUserId = export.ApplicationUserId,
                    GroupId = export.GroupId,
                    To = export.To,
                    Subject = export.Subject,
                    TextBody = export.TextBody,
                    FilePath = export.FilePath,
                    Status = export.Status,
                    DateTime = export.DateTime,
                    Group = group
                });
            }

            return (resultData, exportData.total, exportData.totalDisplay);
        }

        public IList<Export> GetExports(Guid userId)
        {
            var entityExports = _excelUnitOfWork.Exports.Get(
                x => x.Status == "completed" && x.ApplicationUserId == userId, "Group");
            var exports = new List<Export>();

            foreach (Entities.Export export in entityExports)
            {
                exports.Add(new Export
                {
                    Id = export.Id,
                    ApplicationUserId = export.ApplicationUserId,
                    GroupId = export.GroupId,
                    To = export.To,
                    Subject = export.Subject,
                    TextBody = export.TextBody,
                    FilePath = export.FilePath,
                    Status = export.Status,
                    DateTime = export.DateTime
                });
            }

            return exports;
        }

        public Export GetExport(int id)
        {
            var export = _excelUnitOfWork.Exports.GetById(id);

            if (export == null) return null;

            return _mapper.Map<Export>(export);
        }

        public IList<Export> GetPendingExports()
        {
            var entityExports = _excelUnitOfWork.Exports.Get(x => x.Status == "pending", string.Empty);
            var exports = new List<Export>();

            foreach (Entities.Export export in entityExports)
            {
                exports.Add(new Export
                {
                    Id = export.Id,
                    ApplicationUserId = export.ApplicationUserId,
                    GroupId = export.GroupId,
                    To = export.To,
                    Subject = export.Subject,
                    TextBody = export.TextBody,
                    FilePath = export.FilePath,
                    Status = export.Status,
                    DateTime = export.DateTime
                });
            }

            return exports;
        }

        public void UpdateExport(Export export)
        {
            if (export == null)
                throw new NullReferenceException("Export is Null");

            var exportEntity = _excelUnitOfWork.Exports.GetById(export.Id);

            if (exportEntity != null)
            {
                exportEntity.Id = export.Id;
                exportEntity.ApplicationUserId = export.ApplicationUserId;
                exportEntity.GroupId = export.GroupId;
                exportEntity.To = export.To;
                exportEntity.Subject = export.Subject;
                exportEntity.TextBody = export.TextBody;
                exportEntity.FilePath = export.FilePath;
                exportEntity.Status = export.Status;
                exportEntity.DateTime = export.DateTime;

                _excelUnitOfWork.Save();
            }
            else
                throw new InvalidOperationException("Couldn't find export");
        }

        public void DeleteExport(int id)
        {
            _excelUnitOfWork.Exports.Remove(id);
            _excelUnitOfWork.Save();
        }
    }
}
