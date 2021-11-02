using Autofac;
using DataImporter;
using DataImporter.Excel.BusinessObjects;
using DataImporter.Excel.Services;
using System;
using System.ComponentModel.DataAnnotations;

namespace DataImporter.Models.GroupModels
{
    public class EditGroupModel
    {
        public int? Id { get; set; }
        [Required, MaxLength(200, ErrorMessage = "Group Name should be less than 200 charcaters")]
        public string Name { get; set; }

        private readonly IGroupService _groupService;

        public EditGroupModel()
        {
            _groupService = Startup.AutofacContainer.Resolve<IGroupService>();
        }
        
        public EditGroupModel(IGroupService groupService)
        {
            _groupService = groupService;
        }

        public void LoadModelData(int id)
        {
            var group = _groupService.GetGroup(id);
            Id = group?.Id;
            Name = group.Name;
        }

        internal void Update()
        {
            var group = new Group
            {
                Id = Id.HasValue ? Id.Value : 0,
                Name = Name
            };

            _groupService.UpdateGroup(group);
        }
    }
}
