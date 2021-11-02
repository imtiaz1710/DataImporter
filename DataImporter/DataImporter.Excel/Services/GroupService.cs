using AutoMapper;
using DataImporter.Excel.BusinessObjects;
using DataImporter.Excel.UnitOfWorks;
using DataImporter.Membership.Contexts;
using DataImporter.Membership.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
namespace DataImporter.Excel.Services
{
    public class GroupService : IGroupService
    {
        private readonly IExcelUnitOfWork _excelUnitOfWork;
        private readonly IMapper _mapper;

        public GroupService(IExcelUnitOfWork excelUnitOfWork, IMapper mapper)
        {
            _excelUnitOfWork = excelUnitOfWork;
            _mapper = mapper;
        }

        public IList<Group> GetAllGroupsByUserId(Guid userId)
        {
            var groupEntities = _excelUnitOfWork.Groups.Get(x => x.ApplicationUserId == userId, "ApplicationUser");
            var groups = new List<Group>();

            foreach (var entity in groupEntities)
            {
                groups.Add(new Group
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    ApplicationUserId = entity.ApplicationUserId,
                    ApplicationUser = entity.ApplicationUser
                });
            }

            return groups;
        }

        public void CreateGroup(Group group)
        {
            if (group == null)
                throw new NullReferenceException("Group is Null");

            if (IsNameAlreadyUsed(group.Name))
                throw new DuplicateNameException("Group title already exists");

            _excelUnitOfWork.Groups.Add(
                _mapper.Map<Entities.Group>(group)
            );

            _excelUnitOfWork.Save();
        }

        public (IList<Group> records, int total, int totalDisplay) GetGroups(int pageIndex, int pageSize,
            string searchText, string sortText, Guid userId)
        {
            var groupData = _excelUnitOfWork.Groups.GetDynamic(
                string.IsNullOrWhiteSpace(searchText) ? x => x.ApplicationUserId == userId : x => x.ApplicationUserId == userId && x.Name.Contains(searchText),
                sortText, string.Empty, pageIndex, pageSize);

            var resultData = (from groupp in groupData.data
                              select _mapper.Map<Group>(groupp)).ToList();

            return (resultData, groupData.total, groupData.totalDisplay);
        }

        public Group GetGroup(int id)
        {
            var group = _excelUnitOfWork.Groups.GetById(id);

            if (group == null) return null;

            return _mapper.Map<Group>(group);
        }

        public void UpdateGroup(Group group)
        {
            if (group == null)
                throw new NullReferenceException("Group is Null");

            if (IsNameAlreadyUsed(group.Name, group.Id))
                throw new DuplicateNameException("Group title already used in other group.");

            var groupEntity = _excelUnitOfWork.Groups.GetById(group.Id);

            if (groupEntity != null)
            {
                _mapper.Map(group, groupEntity);
                _excelUnitOfWork.Save();
            }
            else
                throw new InvalidOperationException("Couldn't find group");
        }

        public void DeleteGroup(int id)
        {
            _excelUnitOfWork.Groups.Remove(id);
            _excelUnitOfWork.Save();
        }

        public bool IsNameAlreadyUsed(string name) =>
            _excelUnitOfWork.Groups.GetCount(x => x.Name == name) > 0;

        public bool IsNameAlreadyUsed(string name, int id) =>
            _excelUnitOfWork.Groups.GetCount(x => x.Name == name && x.Id != id) > 0;
    }
}
