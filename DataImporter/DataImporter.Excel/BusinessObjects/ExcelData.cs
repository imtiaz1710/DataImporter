using DataImporter.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImporter.Excel.BusinessObjects
{
    public class ExcelData
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public Guid ApplicationUserId { get; set; }
        public DateTime CreateDate { get; set; }
        public List<ExcelFieldData> ExcelFieldDatas = new List<ExcelFieldData>();
    }
}
