using Microsoft.EntityFrameworkCore;

namespace EmployeeMennage.AdDbContext
{
    public class AddBbContext: DbContext
    {
        public AddBbContext(DbContextOptions<AddBbContext> options) : base(options)
        {
        }
      public DbSet<DepertmentTBL> depertment  { get; set; }
        public DbSet<DesignationTBL> designation  { get; set; }
        public DbSet<EmployeeTBL> employee  { get; set; }
        public DbSet<RoleTBL> role { get; set; }
    }


}
