using DataImporter.Data;
using DataImporter.Membership.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImporter.Excel.Entities
{
    public class Group : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Guid ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public List<Import> Imports { get; set; }
        public List<Export> Exports { get; set; }
        public List<ExcelData> ExcelDatas { get; set; }
        public List<TableField> TableFields { get; set; }
    }
}
