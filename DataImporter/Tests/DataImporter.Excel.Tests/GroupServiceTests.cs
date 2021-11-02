using Autofac.Extras.Moq;
using AutoMapper;
using DataImporter.Excel.BusinessObjects;
using DataImporter.Excel.Repositories;
using DataImporter.Excel.Services;
using DataImporter.Excel.UnitOfWorks;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Data;

namespace DataImporter.Excel.Tests
{
    public class Tests
    {
        private AutoMock _mock;
        private Mock<IMapper> _mapperMock;
        private Mock<IGroupService> _groupServiceMock;
        private Mock<IExcelUnitOfWork> _excelUnitOfWorkMock;
        private Mock<IGroupRepository> _groupRepository;
        private IGroupService _groupService;

        [OneTimeSetUp]
        public void ClassSetup()
        {
            _mock = AutoMock.GetLoose();
        }

        [OneTimeTearDown]
        public void ClassCleanup()
        {
            _mock?.Dispose();
        }

        [SetUp]
        public void TestSetup()
        {
            _groupService = _mock.Create<GroupService>();
            _mapperMock = _mock.Mock<IMapper>();
            _groupServiceMock = _mock.Mock<IGroupService>();
            _excelUnitOfWorkMock = _mock.Mock<IExcelUnitOfWork>();
            _groupRepository = _mock.Mock<IGroupRepository>();
        }

        [TearDown]
        public void TestCleanup()
        {
            _mapperMock?.Reset();
        }

        [Test]
        public void CreateGroup_GroupExists_ThrowsException()
        {
            // Arrange
            Group group = null;

            // Assert
            Should.Throw<NullReferenceException>(
                () => _groupService.CreateGroup(group)
            );
        }

        [Test]
        public void DeleteGroup_GroupId_DeleteGroup()
        {
            // Arrange
            var groupId = 1;
            _excelUnitOfWorkMock.Setup(x => x.Groups)
                .Returns(_groupRepository.Object);

            _groupRepository.Setup(x => x.Remove(groupId)).Verifiable();

            _excelUnitOfWorkMock.Setup(x => x.Save()).Verifiable();

            _groupService.DeleteGroup(groupId);

            this.ShouldSatisfyAllConditions(
                () => _groupRepository.VerifyAll(),
                () => _excelUnitOfWorkMock.VerifyAll()
            ) ; 
        }
    }
}