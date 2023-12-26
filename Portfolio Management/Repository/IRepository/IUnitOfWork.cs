

using PortfolioManagement_API.Repository.IRepostiory;

namespace PortfolioManagement_API.Repository.IRepository
{
    public interface IUnitOfWork
    {
        //ICategoryRepository Category { get; }
       

        IApplicationUserRepository ApplicationUser { get; }
        IApplicationRoleRepository ApplicationRole { get; }
        IApplicationUserRoleRepository ApplicationUserRole { get; }
        IUserRepository User { get; }
        IProjectTypeRepository ProjectType { get; }

        IProjectXTechnologyRepository ProjectXTechnology { get; }
        IProjectXProjectTypeRepository ProjectXProjectType { get; }
 
        IProjectDetailsRepository ProjectDetails { get; }

        ITechnologyRepository Technology { get; }
     


        void Save();
    }
}
