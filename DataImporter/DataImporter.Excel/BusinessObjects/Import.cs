using DataImporter.Data;
using DataImporter.Membership.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImporter.Excel.BusinessObjects
{
    public class Import
    {
        public int Id { get; set; }
        public Guid ApplicationUserId { get; set; }
        public String ApplicationUserEmail { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string Path { get; set; }
        public DateTime DateTime { get; set; }
        public string Status { get; set; }
    }
}
