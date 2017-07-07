using System.Data.Entity;
using MES.Data.Migrations;



namespace MES.Data
{
    public class ApplicationDbContext : DbContext { 
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
        }
        public ApplicationDbContext(string connection)
            : base(connection)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public static ApplicationDbContext Create(string connection)
        {
            return new ApplicationDbContext(connection);
        }

        public DbSet<MES.Models.Product> Products { get; set; }

        public DbSet<MES.Models.ProductSequence> ProductSequences { get; set; }

        public DbSet<MES.Models.ProductionLine> ProductionLines { get; set; }

        public DbSet<MES.Models.MachineFamily> MachineFamilies { get; set; }

        public DbSet<MES.Models.Department> Departments { get; set; }

        public DbSet<MES.Models.Machine> Machines { get; set; }

        public DbSet<MES.Models.ProductSequenceItem> ProductSequenceItems { get; set; }

        public DbSet<MES.Models.ProductProcess> ProductProcesses { get; set; }

        public DbSet<MES.Models.Workorder> Workorders { get; set; }

        public System.Data.Entity.DbSet<MES.Models.ProductProcessList> ProductProcessLists { get; set; }
    }
}
