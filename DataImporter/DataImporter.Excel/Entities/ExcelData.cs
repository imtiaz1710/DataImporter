using DataImporter.Data;
using DataImporter.Membership.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImporter.Excel.Entities
{
    public class ExcelData : IEntity<int>
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public Guid ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public DateTime CreateDate { get; set; }
        public List<ExcelFieldData> ExcelFieldDatas { get; set; }
        public Group Group { get; set; }
    }
}
