

using PortfolioManagement_API.Data;
using PortfolioManagement_API.Models;
using PortfolioManagement_API.Repository.IRepository;

namespace PortfolioManagement_API.Repository
{
	public class ApplicationRoleRepository : Repository<ApplicationRole>, IApplicationRoleRepository
	{
        private readonly ApplicationDbContext _db;
        public ApplicationRoleRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<ApplicationRole> UpdateAsync(ApplicationRole entity)
        {
            _db.ApplicationRoles.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
