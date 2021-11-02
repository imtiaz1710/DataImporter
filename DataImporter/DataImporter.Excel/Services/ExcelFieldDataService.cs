using AutoMapper;
using DataImporter.Excel.BusinessObjects;
using DataImporter.Excel.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImporter.Excel.Services
{
    public class ExcelFieldDataService : IExcelFieldDataService
    {
        private readonly IExcelUnitOfWork _excelUnitOfWork;
        private readonly IMapper _mapper;

        public ExcelFieldDataService(IExcelUnitOfWork excelUnitOfWork,
            IMapper mapper)
        {
            _excelUnitOfWork = excelUnitOfWork;
            _mapper = mapper;
        }

        public IList<ExcelFieldData> GetAllExcelFieldDatas()
        {
            var excelFieldDataEntities = _excelUnitOfWork.ExcelFieldDatas.GetAll();
            var excelFieldDatas = new List<ExcelFieldData>();

            foreach(var entity in excelFieldDataEntities)
            {
                var excelFieldData = _mapper.Map<ExcelFieldData>(entity);
                excelFieldDatas.Add(excelFieldData);
            }

            return excelFieldDatas;
        }

        public void CreateExcelFieldData(ExcelFieldData excelFieldData)
        {
            _excelUnitOfWork.ExcelFieldDatas.Add(
                //_mapper.Map<Entities.ExcelFieldData>(excelFieldData)
                new Entities.ExcelFieldData
                {
                    Id = excelFieldData.Id,
                    Field = excelFieldData.Field,
                    Value = excelFieldData.Value,
                    ExcelDataId = excelFieldData.ExcelDataId
                }
            );

            _excelUnitOfWork.Save();
        }

        public (IList<ExcelFieldData> records, int total, int totalDisplay) GetExcelFieldDatas(int pageIndex, int pageSize, 
            string searchText, string sortText)
        {
            var excelFieldDataData = _excelUnitOfWork.ExcelFieldDatas.GetDynamic(
                string.IsNullOrWhiteSpace(searchText)? null: x => x.Field.Contains(searchText), 
                sortText, string.Empty, pageIndex, pageSize);

            var resultData = (from excelFieldData in excelFieldDataData.data
                          select _mapper.Map<ExcelFieldData>(excelFieldData)).ToList();

            return (resultData, excelFieldDataData.total, excelFieldDataData.totalDisplay);
        }

        public ExcelFieldData GetExcelFieldData(int id)
        {
            var excelFieldData = _excelUnitOfWork.ExcelFieldDatas.GetById(id);

            if (excelFieldData == null) return null;

            return _mapper.Map<ExcelFieldData>(excelFieldData);
        }

        public void UpdateExcelFieldData(ExcelFieldData excelFieldData)
        {
            if (excelFieldData == null)
                throw new InvalidOperationException("ExcelFieldData is missing");

            var excelFieldDataEntity = _excelUnitOfWork.ExcelFieldDatas.GetById(excelFieldData.Id);

            if (excelFieldDataEntity != null)
            {
                _mapper.Map(excelFieldData, excelFieldDataEntity);
                _excelUnitOfWork.Save();
            }
            else
                throw new InvalidOperationException("Couldn't find excelFieldData");
        }

        public void DeleteExcelFieldData(int id)
        {
            _excelUnitOfWork.ExcelFieldDatas.Remove(id);
            _excelUnitOfWork.Save();
        }
    }
}
