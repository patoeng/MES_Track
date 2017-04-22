

using System.Data.Entity.Migrations;
using System.Linq;

namespace MES.Data.Migrations
{
    public class Configuration: DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

       
    }
}
