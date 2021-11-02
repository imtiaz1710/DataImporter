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
    public class GroupRepository : Repository<Group, int>, IGroupRepository
    {
        public GroupRepository(IExcelContext context) : 
            base((DbContext)context)
        {
        }
    }
}
