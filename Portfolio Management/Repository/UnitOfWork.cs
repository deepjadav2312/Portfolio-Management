
using Portfolio_Management.Models;
using PortfolioManagement_API.Data;
using PortfolioManagement_API.Repository;
using PortfolioManagement_API.Repository.IRepository;
using PortfolioManagement_API.Repository.IRepostiory;

namespace PortfolioManagement_API.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IApplicationRoleRepository ApplicationRole { get; private set; }
        public IApplicationUserRoleRepository ApplicationUserRole { get; private set; }
        public IUserRepository User { get; private set; }
  
        public IProjectXTechnologyRepository ProjectXTechnology { get; private set; }



        public IProjectTypeRepository ProjectType { get; private set; }

        public IProjectDetailsRepository ProjectDetails { get; private set; }

      public IProjectXProjectTypeRepository ProjectXProjectType { get; private set; }

        public ITechnologyRepository Technology { get; private set; }


      


        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;

            //Category = new CategoryRepository(_db);
        
            ApplicationUser = new ApplicationUserRepository(_db);
            ApplicationRole = new ApplicationRoleRepository(_db);
            ApplicationUserRole = new ApplicationUserRoleRepository(_db);

            ProjectType = new ProjectTypeRepository(_db);
            ProjectDetails = new ProjectDetailsRepository(_db);
            ProjectXTechnology = new ProjectXTechnologyRepository(_db);
            ProjectXProjectType = new ProjectXProjectTypeRepository(_db);
            Technology = new  TechnologyRepository(_db);
       


        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
