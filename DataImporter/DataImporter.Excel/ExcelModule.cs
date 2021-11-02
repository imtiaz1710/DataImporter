using Autofac;
using DataImporter.Excel.Contexts;
using DataImporter.Excel.Repositories;
using DataImporter.Excel.Services;
using DataImporter.Excel.UnitOfWorks;

namespace DataImporter.Excel
{
    public class ExcelModule : Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public ExcelModule(string connectionStringName, string migrationAssemblyName)
        {
            _connectionString = connectionStringName;
            _migrationAssemblyName = migrationAssemblyName;
        }

        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterType<ExcelContext>().AsSelf()
               .WithParameter("connectionString", _connectionString)
               .WithParameter("migrationAssemblyName", _migrationAssemblyName)
               .InstancePerLifetimeScope();

            builder.RegisterType<ExcelContext>().As<IExcelContext>()
                .WithParameter("connectionString", _connectionString)
                .WithParameter("migrationAssemblyName", _migrationAssemblyName)
                .InstancePerLifetimeScope();

            builder.RegisterType<ExcelDataRepository>().As<IExcelDataRepository>()
                .InstancePerLifetimeScope(); 

            builder.RegisterType<ExcelFieldDataRepository>().As<IExcelFieldDataRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ImportRepository>().As<IImportRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ExportRepository>().As<IExportRepository>()
               .InstancePerLifetimeScope();

            builder.RegisterType<GroupRepository>().As<IGroupRepository>()
               .InstancePerLifetimeScope();

            builder.RegisterType<TableFieldRepository>().As<ITableFieldRepository>()
              .InstancePerLifetimeScope();

            builder.RegisterType<ExcelUnitOfWork>().As<IExcelUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ExportService>().As<IExportService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ImportService>().As<IImportService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<GroupService>().As<IGroupService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ExcelDataService>().As<IExcelDataService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ExcelFieldDataService>().As<IExcelFieldDataService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TableFieldService>().As<ITableFieldService>()
              .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
