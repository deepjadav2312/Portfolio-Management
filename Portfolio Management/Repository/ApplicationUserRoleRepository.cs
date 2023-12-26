
using PortfolioManagement_API.Data;
using PortfolioManagement_API.Models;
using PortfolioManagement_API.Repository.IRepository;

namespace PortfolioManagement_API.Repository
{
	public class ApplicationUserRoleRepository : Repository<ApplicationUserRole>, IApplicationUserRoleRepository
    {
        private readonly ApplicationDbContext _db;
        public ApplicationUserRoleRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<ApplicationUserRole> UpdateAsync(ApplicationUserRole entity)
        {
           // _db.ApplicationUserRoles.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
