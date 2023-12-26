
using PortfolioManagement_API.Data;
using PortfolioManagement_API.Models;
using PortfolioManagement_API.Repository;
using PortfolioManagement_API.Repository.IRepostiory;
using System.Linq.Expressions;

namespace PortfolioManagement_API.Repository
{
    public class ProjectTypeRepository : Repository<ProjectType>, IProjectTypeRepository
    {
        private readonly ApplicationDbContext _db;
        public ProjectTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<ProjectType> UpdateAsync(ProjectType entity)
        {
            _db.ProjectTypes.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
