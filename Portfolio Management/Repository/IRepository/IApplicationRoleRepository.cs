


using PortfolioManagement_API.Models;
using PortfolioManagement_API.Repository.IRepostiory;

namespace PortfolioManagement_API.Repository.IRepository
{
	public interface IApplicationRoleRepository : IRepository<ApplicationRole>
    {
        Task<ApplicationRole> UpdateAsync(ApplicationRole entity);
    }
}

