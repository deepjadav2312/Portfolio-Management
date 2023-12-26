

using PortfolioManagement_API.Models;
using PortfolioManagement_API.Repository.IRepostiory;

namespace PortfolioManagement_API.Repository.IRepository
{
	public interface IApplicationUserRoleRepository : IRepository<ApplicationUserRole>
    {
        Task<ApplicationUserRole> UpdateAsync(ApplicationUserRole entity);
    }
}