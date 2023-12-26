


using Portfolio_Management.Models;
using PortfolioManagement_API.Repository.IRepostiory;

namespace PortfolioManagement_API.Repository.IRepository
{
	public interface IProjectXTechnologyRepository : IRepository<ProjectXTechnology>
    {
        Task<ProjectXTechnology> UpdateAsync(ProjectXTechnology entity);
    }
}

