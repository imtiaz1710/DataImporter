using AutoMapper;
using DataImporter.Excel.BusinessObjects;
using DataImporter.Excel.UnitOfWorks;
using DataImporter.Membership.Contexts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
namespace DataImporter.Excel.Services
{
    public class ImportService : IImportService
    {
        private readonly IExcelUnitOfWork _excelUnitOfWork;
        private readonly IMapper _mapper;

        public ImportService(IExcelUnitOfWork excelUnitOfWork, IMapper mapper)
        {
            _excelUnitOfWork = excelUnitOfWork;
            _mapper = mapper;

        }

        public IList<Import> GetAllImports()
        {
            var importEntities = _excelUnitOfWork.Imports.GetAll();
            var imports = new List<Import>();

            foreach (var entity in importEntities)
            {
                imports.Add(_mapper.Map<Import>(entity));
            }

            return imports;
        }

        public void CreateImport(Import import)
        {
            if (import == null)
                throw new NullReferenceException("Import is Null");

            var entityImport = new Entities.Import
            {
                ApplicationUserId = import.ApplicationUserId,
                GroupId = import.GroupId,
                Path = import.Path,
                DateTime = import.DateTime,
                Status = import.Status
            };

            _excelUnitOfWork.Imports.Add(
               entityImport
            );

            _excelUnitOfWork.Save();
        }

        public (IList<Import> records, int total, int totalDisplay) GetImports(int pageIndex, int pageSize,
            string searchText, string sortText, Guid userId)
        {
            var importData = _excelUnitOfWork.Imports.GetDynamic(
                string.IsNullOrWhiteSpace(searchText) ? x => x.ApplicationUserId == userId : 
                x => x.ApplicationUserId == userId && x.DateTime.ToString().Contains(searchText),
                sortText, "Group,ApplicationUser" , pageIndex, pageSize);

            var resultData = new List<Import>();

            foreach (var import in importData.data)
            {
                resultData.Add(new Import
                {
                    Id = import.Id,
                    ApplicationUserId = import.ApplicationUserId,
                    ApplicationUserEmail = import.ApplicationUser.Email,
                    GroupId = import.GroupId,
                    GroupName = import.Group.Name,
                    Path = import.Path,
                    DateTime = import.DateTime,
                    Status = import.Status
                });
            }

            return (resultData, importData.total, importData.totalDisplay);
        }

        public IList<Import> GetPendingImports()
        {
            var entityImports = _excelUnitOfWork.Imports.Get(x => x.Status == "pending", string.Empty);
            var imports = new List<Import>();

            foreach (Entities.Import import in entityImports)
            {
                imports.Add(new Import
                {
                    Id = import.Id,
                    ApplicationUserId = import.ApplicationUserId,
                    GroupId = import.GroupId,
                    Path = import.Path,
                    DateTime = import.DateTime,
                    Status = import.Status
                });
            }

            return imports;
        }

        public Import GetImport(int id)
        {
            var import = _excelUnitOfWork.Imports.GetById(id);

            if (import == null) return null;

            return _mapper.Map<Import>(import);
        }

        public void UpdateImport(Import import)
        {
            if (import == null)
                throw new NullReferenceException("Import is Null");

            var importEntity = _excelUnitOfWork.Imports.GetById(import.Id);

            if (importEntity != null)
            {
                importEntity.Status = import.Status;
                _excelUnitOfWork.Save();
            }
            else
                throw new InvalidOperationException("Couldn't find import");
        }

        public void DeleteImport(int id)
        {
            _excelUnitOfWork.Imports.Remove(id);
            _excelUnitOfWork.Save();
        }
    }
}
