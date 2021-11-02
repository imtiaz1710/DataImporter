using DataImporter.Data;
using DataImporter.Membership.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImporter.Excel.Entities
{
    public class TableField : IEntity<int>
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public Guid ApplicationUserId { get; set; }
        public string FieldName { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Group Group { get; set; }
    }
}
