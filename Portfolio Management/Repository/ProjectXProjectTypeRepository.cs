
using Portfolio_Management.Models;
using PortfolioManagement_API.Data;
using PortfolioManagement_API.Models;
using PortfolioManagement_API.Repository;
using PortfolioManagement_API.Repository.IRepository;
using PortfolioManagement_API.Repository.IRepostiory;
using System.Linq.Expressions;

namespace PortfolioManagement_API.Repository
{
    public class ProjectXProjectTypeRepository : Repository<ProjectXProjectType>, IProjectXProjectTypeRepository
    {
        private readonly ApplicationDbContext _db;
        public ProjectXProjectTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<ProjectXProjectType> UpdateAsync(ProjectXProjectType entity)
        {
            _db.ProjectXProjectTypes.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
