using DataImporter.Data;
using DataImporter.Membership.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImporter.Excel.BusinessObjects
{
    public class Export
    {
        public int Id { get; set; }
        public Guid ApplicationUserId { get; set; }
        public int GroupId { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string TextBody { get; set; }
        public string FilePath { get; set; }
        public string Status { get; set; }
        public DateTime DateTime { get; set; }
        public Group Group { get; set; }
    }
}
