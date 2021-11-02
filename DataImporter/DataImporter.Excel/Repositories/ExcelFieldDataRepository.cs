using DataImporter.Data;
using DataImporter.Excel.Contexts;
using DataImporter.Excel.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImporter.Excel.Repositories
{
    public class ExcelFieldDataRepository : Repository<ExcelFieldData, int>, IExcelFieldDataRepository
    {
        public ExcelFieldDataRepository(IExcelContext context) : 
            base((DbContext)context)
        {
        }
    }
}
