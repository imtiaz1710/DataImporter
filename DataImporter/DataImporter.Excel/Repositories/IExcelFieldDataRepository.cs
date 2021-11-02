﻿using DataImporter.Data;
using DataImporter.Excel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImporter.Excel.Repositories
{
    public interface IExcelFieldDataRepository : IRepository<ExcelFieldData, int>
    {
    }
}
