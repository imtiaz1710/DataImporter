using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EO=DataImporter.Excel.Entities;
using BO = DataImporter.Excel.BusinessObjects;

namespace DataImporter.Excel.Profiles
{
    public class ExcelProfile : Profile
    {
        public ExcelProfile()
        {
            CreateMap<EO.ExcelData, BO.ExcelData>().ReverseMap();
            CreateMap<EO.ExcelFieldData, BO.ExcelFieldData>().ReverseMap();
            CreateMap<EO.Export, BO.Export>().ReverseMap();
            CreateMap<EO.Import, BO.Import>().ReverseMap();
            CreateMap<EO.Group, BO.Group>().ReverseMap();
            CreateMap<EO.TableField, BO.TableField>().ReverseMap();
        }
    }
}
