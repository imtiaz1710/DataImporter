using Autofac;
using DataImporter;
using DataImporter.Excel.BusinessObjects;
using DataImporter.Excel.Services;
using DataImporter.Membership.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace DataImporter.Models.GroupModels
{
    public class CreateGroupModel
    {
        [Required, MaxLength(200, ErrorMessage = "Group Name should be less than 200 charcaters")]
        public string Name { get; set; }
        public Guid UserId { get; set; }

        private readonly IGroupService _groupService;

        public CreateGroupModel()
        {
            _groupService = Startup.AutofacContainer.Resolve<IGroupService>();
        }

        public CreateGroupModel(IGroupService groupService)
        {
            _groupService = groupService;
        }

        internal void CreateGroup()
        {
            var group = new Group
            {
                Name = Name,
                ApplicationUserId = UserId
            };

            _groupService.CreateGroup(group);
        }
    }
}
