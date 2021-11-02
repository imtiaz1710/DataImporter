using AutoMapper;
using DataImporter.Excel.BusinessObjects;
using DataImporter.Excel.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
namespace DataImporter.Excel.Services
{
    public class ExcelDataService : IExcelDataService
    {
        private readonly IExcelUnitOfWork _excelDataUnitOfWork;
        private readonly IMapper _mapper;

        public ExcelDataService(IExcelUnitOfWork excelDataUnitOfWork, IMapper mapper)
        {
            _excelDataUnitOfWork = excelDataUnitOfWork;
            _mapper = mapper;
        }

        public IList<ExcelData> GetAllExcelDatas()
        {
            var excelDataEntities = _excelDataUnitOfWork.ExcelDatas.GetAll();
            var excelDatas = new List<ExcelData>();

            foreach (var entity in excelDataEntities)
            {
                excelDatas.Add(_mapper.Map<ExcelData>(entity));
            }

            return excelDatas;
        }

        public IList<ExcelData> GetExcelDatasByGroupId(int groupId)
        {
            var entityExcelDatas = _excelDataUnitOfWork.ExcelDatas.Get(x => x.GroupId == groupId, "ExcelFieldDatas");
            var excelDatas = new List<ExcelData>();

            foreach (var entity in entityExcelDatas)
            {
                var excelFieldDatas = new List<ExcelFieldData>();

                foreach (var entityExcelFieldData in entity.ExcelFieldDatas)
                {
                    excelFieldDatas.Add(new ExcelFieldData
                    {
                        ExcelDataId = entityExcelFieldData.ExcelDataId,
                        Id = entityExcelFieldData.Id,
                        Value = entityExcelFieldData.Value,
                        Field = entityExcelFieldData.Field
                    });
                }

                var excelData = new ExcelData
                {
                    ExcelFieldDatas = excelFieldDatas,
                    ApplicationUserId = entity.ApplicationUserId,
                    Id = entity.Id,
                    GroupId = entity.GroupId,
                    CreateDate = entity.CreateDate
                };

                excelDatas.Add(excelData);
            }

            return excelDatas;
        }

        public void CreateExcelData(ExcelData excelData)
        {
            if (excelData == null)
                throw new NullReferenceException("ExcelData is Null");

            var entityExcelFieldDatas = new List<Entities.ExcelFieldData>();

            foreach (var excelFieldData in excelData.ExcelFieldDatas)
            {
                entityExcelFieldDatas.Add(new Entities.ExcelFieldData
                {
                    Field = excelFieldData.Field,
                    Value = excelFieldData.Value
                });
            }

            _excelDataUnitOfWork.ExcelDatas.Add(
                new Entities.ExcelData
                {
                    GroupId = excelData.GroupId,
                    ApplicationUserId = excelData.ApplicationUserId,
                    CreateDate = excelData.CreateDate,
                    ExcelFieldDatas = entityExcelFieldDatas
                }
            );

            _excelDataUnitOfWork.Save();
        }

        public (IList<ExcelData> records, int total, int totalDisplay) GetExcelDatas(int pageIndex, int pageSize,
            string searchText, string sortText)
        {
            var excelDataData = _excelDataUnitOfWork.ExcelDatas.GetDynamic(
                string.IsNullOrWhiteSpace(searchText) ? null : x => x.CreateDate.ToString().Contains(searchText),
                sortText, string.Empty, pageIndex, pageSize);

            var resultData = (from excelData in excelDataData.data
                              select _mapper.Map<ExcelData>(excelData)).ToList();

            return (resultData, excelDataData.total, excelDataData.totalDisplay);
        }

        public ExcelData GetExcelData(int id)
        {
            var excelData = _excelDataUnitOfWork.ExcelDatas.GetById(id);

            if (excelData == null) return null;

            return _mapper.Map<ExcelData>(excelData);
        }

        public void UpdateExcelData(ExcelData excelData)
        {
            if (excelData == null)
                throw new NullReferenceException("ExcelData is Null");

            var excelDataEntity = _excelDataUnitOfWork.ExcelDatas.GetById(excelData.Id);

            if (excelDataEntity != null)
            {
                _mapper.Map(excelData, excelDataEntity);
                _excelDataUnitOfWork.Save();
            }
            else
                throw new InvalidOperationException("Couldn't find excelData");
        }

        public void DeleteExcelData(int id)
        {
            _excelDataUnitOfWork.ExcelDatas.Remove(id);
            _excelDataUnitOfWork.Save();
        }
    }
}
