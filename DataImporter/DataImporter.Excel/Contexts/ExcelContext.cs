using DataImporter.Excel.Entities;
using DataImporter.Membership.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImporter.Excel.Contexts
{
    public class ExcelContext : DbContext, IExcelContext
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public ExcelContext(string connectionString, string migrationAssemblyName)
        {
            _connectionString = connectionString;
            _migrationAssemblyName = migrationAssemblyName;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            if (!dbContextOptionsBuilder.IsConfigured)
            {
                dbContextOptionsBuilder.UseSqlServer(
                    _connectionString,
                    m => m.MigrationsAssembly(_migrationAssemblyName));
            }

            base.OnConfiguring(dbContextOptionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ExcelData>()
                .HasMany(ed => ed.ExcelFieldDatas)
                .WithOne(efd => efd.ExcelData);

            builder.Entity<Group>()
                .HasMany(g => g.Imports)
                .WithOne(i => i.Group)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Group>()
                .HasMany(g => g.Exports)
                .WithOne(e => e.Group)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Group>()
                .HasMany(g => g.ExcelDatas)
                .WithOne(ed => ed.Group)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Group>()
                .HasMany(g => g.TableFields)
                .WithOne(tf => tf.Group)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ApplicationUser>()
                .ToTable("AspNetUsers", t => t.ExcludeFromMigrations())
                .HasMany<Export>()
                .WithOne(x => x.ApplicationUser);

            builder.Entity<ApplicationUser>()
                  .ToTable("AspNetUsers", t => t.ExcludeFromMigrations())
                  .HasMany<Group>()
                  .WithOne(x => x.ApplicationUser);

            builder.Entity<ApplicationUser>()
                 .ToTable("AspNetUsers", t => t.ExcludeFromMigrations())
                 .HasMany<Import>()
                 .WithOne(x => x.ApplicationUser);

            base.OnModelCreating(builder);
        }

        public DbSet<Group> Groups { get; set; }
        public DbSet<ExcelData> ExcelDatas { get; set; }
        public DbSet<ExcelFieldData> ExcelFieldDatas { get; set; }
        public DbSet<Export> Exports { get; set; }
        public DbSet<Import> Imports { get; set; }
        public DbSet<TableField> TableFields { get; set; }
        
    }
}
