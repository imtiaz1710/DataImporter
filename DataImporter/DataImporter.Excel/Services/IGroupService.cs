using DataImporter.Excel.BusinessObjects;
using System;
using System.Collections.Generic;

namespace DataImporter.Excel.Services
{
    public interface IGroupService
    {
        void CreateGroup(Group group);
        void DeleteGroup(int id);
        IList<Group> GetAllGroupsByUserId(Guid UserId);
        Group GetGroup(int id);

        (IList<Group> records, int total, int totalDisplay) GetGroups(
            int pageIndex, int pageSize, string searchText, string sortText, Guid userId);

        void UpdateGroup(Group group);
        bool IsNameAlreadyUsed(string name);

    }
}