
using Portfolio_Management.Models;
using PortfolioManagement_API.Data;
using PortfolioManagement_API.Models;
using PortfolioManagement_API.Repository;
using PortfolioManagement_API.Repository.IRepository;
using PortfolioManagement_API.Repository.IRepostiory;
using System.Linq.Expressions;

namespace PortfolioManagement_API.Repository
{
    public class ProjectXTechnologyRepository : Repository<ProjectXTechnology>, IProjectXTechnologyRepository
    {
        private readonly ApplicationDbContext _db;
        public ProjectXTechnologyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<ProjectXTechnology> UpdateAsync(ProjectXTechnology entity)
        {
            _db.ProjectXTechnologies.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
