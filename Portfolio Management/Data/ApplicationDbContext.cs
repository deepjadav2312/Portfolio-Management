
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Portfolio_Management.Models;
using PortfolioManagement_API.Models;

namespace PortfolioManagement_API.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>   /* DbContext*/  /*IdentityDbContext<ApplicationUser>*/    /*IdentityDbContext<ApplicationUser, IdentityRole, string>*/
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }

        public DbSet<ProjectType> ProjectTypes { get; set; }

        public DbSet<ProjectDetails> ProjectDetailses { get; set; }
        public DbSet<ProjectXProjectType> ProjectXProjectTypes { get; set; }
        public DbSet<ProjectXTechnology> ProjectXTechnologies { get; set; }
        public DbSet<Technology> Technologies { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
