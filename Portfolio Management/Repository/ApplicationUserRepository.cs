

using PortfolioManagement_API.Data;
using PortfolioManagement_API.Models;
using PortfolioManagement_API.Repository.IRepository;

namespace PortfolioManagement_API.Repository
{
	public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;
        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<ApplicationUser> UpdateAsync(ApplicationUser entity)
        {
            _db.ApplicationUsers.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
