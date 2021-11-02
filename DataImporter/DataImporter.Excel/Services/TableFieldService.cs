using AutoMapper;
using DataImporter.Excel.BusinessObjects;
using DataImporter.Excel.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace DataImporter.Excel.Services
{
    public class TableFieldService : ITableFieldService
    {
        private readonly IExcelUnitOfWork _excelUnitOfWork;
        private readonly IMapper _mapper;

        public TableFieldService(IExcelUnitOfWork excelUnitOfWork, IMapper mapper)
        {
            _excelUnitOfWork = excelUnitOfWork;
            _mapper = mapper;
        }

        public IList<TableField> GetAllTableFields()
        {
            var tableFieldEntities = _excelUnitOfWork.TableFields.GetAll();
            var tableFields = new List<TableField>();

            foreach (var entity in tableFieldEntities)
            {
                tableFields.Add(_mapper.Map<TableField>(entity));
            }

            return tableFields;
        }

        public void CreateTableField(TableField tableField)
        {
            if (tableField == null)
                throw new NullReferenceException("TableField is Null");

            _excelUnitOfWork.TableFields.Add(
                _mapper.Map<Entities.TableField>(tableField)
            );

            _excelUnitOfWork.Save();
        }

        public IList<TableField> GetTableFieldsByGroup(int groupId)
        {
            var entityTableFields = _excelUnitOfWork.TableFields.Get(x => x.GroupId == groupId, string.Empty);
            var tableFields = new List<TableField>();

            foreach (var entityTableField in entityTableFields)
            {
                tableFields.Add(new TableField
                {
                   Id = entityTableField.Id,
                   GroupId = entityTableField.GroupId,
                   ApplicationUserId = entityTableField.ApplicationUserId,
                   FieldName = entityTableField.FieldName
                });
            }

            return tableFields;
        }

        public (IList<TableField> records, int total, int totalDisplay) GetTableFields(int pageIndex, int pageSize,
            string searchText, string sortText)
        {
            var tableFieldData = _excelUnitOfWork.TableFields.GetDynamic(
                string.IsNullOrWhiteSpace(searchText) ? null : x => x.FieldName.Contains(searchText),
                sortText, string.Empty, pageIndex, pageSize);

            var resultData = (from tableField in tableFieldData.data
                              select _mapper.Map<TableField>(tableField)).ToList();

            return (resultData, tableFieldData.total, tableFieldData.totalDisplay);
        }

        public IList<TableField> GetTableFields(int GroupId)
        {
            var entityTableFields = _excelUnitOfWork.TableFields.Get(x => x.GroupId == GroupId, string.Empty);

            var tableFields = (from tableField in entityTableFields
                               select _mapper.Map<TableField>(tableField)).ToList();

            return tableFields;
        }

        public TableField GetTableField(int id)
        {
            var tableField = _excelUnitOfWork.TableFields.GetById(id);

            if (tableField == null) return null;

            return _mapper.Map<TableField>(tableField);
        }

        public void UpdateTableField(TableField tableField)
        {
            if (tableField == null)
                throw new NullReferenceException("TableField is Null");

            var tableFieldEntity = _excelUnitOfWork.TableFields.GetById(tableField.Id);

            if (tableFieldEntity != null)
            {
                _mapper.Map(tableField, tableFieldEntity);
                _excelUnitOfWork.Save();
            }
            else
                throw new InvalidOperationException("Couldn't find tableField");
        }

        public void DeleteTableField(int id)
        {
            _excelUnitOfWork.TableFields.Remove(id);
            _excelUnitOfWork.Save();
        }

    }
}
