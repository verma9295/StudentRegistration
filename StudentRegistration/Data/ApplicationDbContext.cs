using Microsoft.EntityFrameworkCore;
using StudentRegistration.Models;

namespace StudentRegistration.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<RegForm> regForms { get; set; }
        public DbSet<tblstate> tblstates { get; set; }
        public DbSet<tblcity> tblcities { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<tblgender> tblgenders { get; set; }
        public DbSet<StudentRegistration.Models.ChangePassword>? ChangePassword { get; set; }
    }

}
