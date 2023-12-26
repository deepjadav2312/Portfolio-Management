


using PortfolioManagement_API.Models;
using PortfolioManagement_API.Repository.IRepostiory;

namespace PortfolioManagement_API.Repository.IRepository
{
	public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        Task<ApplicationUser> UpdateAsync(ApplicationUser entity);
    }
}