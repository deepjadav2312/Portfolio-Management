

using PortfolioManagement_API.Models;
using PortfolioManagement_API.Repository.IRepostiory;

namespace PortfolioManagement_API.Repository.IRepository
{
	public interface IProjectXProjectTypeRepository : IRepository<ProjectXProjectType>
    {
        Task<ProjectXProjectType> UpdateAsync(ProjectXProjectType entity);
    }
}

